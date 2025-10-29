using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    public class AuthenticatedController : Controller
    {
        protected long CurrentUserId =>
            HttpContext.Session.GetInt32("LoggedInUserID") is int v
                ? v
                : throw new UnauthorizedAccessException("User not logged in");
    }

    public class MessagesController : AuthenticatedController
    {
        private readonly CampusLearnContext _context;

        public MessagesController(CampusLearnContext context)
        {
            _context = context;
        }

        public IActionResult Test() => View("Test");

        // ---------------------------
        // Helpers: Groups (Option A)
        // ---------------------------

        // All module names as "groups"
        private Task<List<string>> GetAllGroupsAsync()
        {
            return _context.Modules
                .Select(m => m.ModuleName)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        }

        // Only groups for a specific student (if you prefer per-user groups)
        private Task<List<string>> GetGroupsForStudentAsync(long studentId)
        {
            return _context.EnrollmentDegrees
                .Where(ed => ed.Enrollment.StudentID == studentId)
                .Select(ed => ed.Degree.DegreeName)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        }

        // ---------------------------
        // Build user list
        // ---------------------------
        private async Task<List<MessageUser>> GetAllUsersAsync(long currentUserId)
        {
            var students = await _context.Students
                .Where(s => s.StudentID != currentUserId)
                .Select(s => new MessageUser
                {
                    Id = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.PersonalEmail,
                    PhoneNumber = s.Phone,
                    Role = "Student",
                    // If you want per-student groups, populate later (see optional block below)
                    Groups = new List<string>()
                })
                .ToListAsync();

            var tutors = await _context.Tutors
                .Select(t => new MessageUser
                {
                    Id = t.TutorID,
                    FirstName = t.TutorName,
                    LastName = t.TutorSurname,
                    Role = "Tutor",
                    Groups = new List<string>()
                })
                .ToListAsync();

            var all = students.Concat(tutors)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToList();

            return all;
        }

        // ---------------------------
        // Index: user list + groups + filters
        // ---------------------------
        public async Task<IActionResult> Index(string? searchQuery, string? roleFilter)
        {
            var currentUserId = CurrentUserId;

            var allUsers = await GetAllUsersAsync(currentUserId);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                allUsers = allUsers.Where(u =>
                       (u.FirstName ?? "").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                    || (u.LastName ?? "").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                    || u.Id.ToString().Contains(searchQuery)
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                allUsers = allUsers.Where(u => u.Role == roleFilter).ToList();
            }

            // Option A: all module names as groups
            var groups = await GetAllGroupsAsync();

            var model = new MessageView
            {
                CurrentUserID = currentUserId,
                SelectedRecipientID = 0,
                AllUsers = allUsers,
                Messages = new List<ChatMessage>(),
                SearchQuery = searchQuery,
                RoleFilter = roleFilter,
                Groups = groups
            };

            return View("Index", model);
        }

        // ---------------------------
        // Chat helpers
        // ---------------------------
        private long ResolveOtherUserId(string topic, long currentUserId)
        {
            // Topic = "ChatBetween:<smallId>:<bigId>"
            if (topic.StartsWith("ChatBetween:", StringComparison.Ordinal))
            {
                var parts = topic.Substring("ChatBetween:".Length).Split(':');
                if (parts.Length == 2 &&
                    long.TryParse(parts[0], out var id1) &&
                    long.TryParse(parts[1], out var id2))
                {
                    return currentUserId == id1 ? id2 : id1;
                }
            }
            throw new Exception("Invalid topic format");
        }

        private MessageUser? GetUserById(long id)
        {
            var student = _context.Students.AsNoTracking().FirstOrDefault(s => s.StudentID == id);
            if (student != null)
                return new MessageUser
                {
                    Id = student.StudentID,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.PersonalEmail,
                    PhoneNumber = student.Phone,
                    Role = "Student",
                    Groups = new List<string>()
                };

            var tutor = _context.Tutors.AsNoTracking().FirstOrDefault(t => t.TutorID == id);
            if (tutor != null)
                return new MessageUser
                {
                    Id = tutor.TutorID,
                    FirstName = tutor.TutorName,
                    LastName = tutor.TutorSurname,
                    Role = "Tutor",
                    Groups = new List<string>()
                };

            return null;
        }

        // Ensures ChatSession.StudentID is a real student ID since DB requires NOT NULL
        private long RequireAnyStudentId(long userAId, long userBId)
        {
            var aIsStudent = _context.Students.Any(s => s.StudentID == userAId);
            if (aIsStudent) return userAId;

            var bIsStudent = _context.Students.Any(s => s.StudentID == userBId);
            if (bIsStudent) return userBId;

            throw new InvalidOperationException("At least one participant must be a student to create a chat session.");
        }

        private ChatSession GetOrCreateChatSession(long userAId, long userBId)
        {
            var small = Math.Min(userAId, userBId);
            var big = Math.Max(userAId, userBId);
            var normalizedTopic = $"ChatBetween:{small}:{big}";

            var session = _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefault(cs => cs.Topic == normalizedTopic);

            if (session == null)
            {
                session = new ChatSession
                {
                    StudentID = RequireAnyStudentId(userAId, userBId),
                    Topic = normalizedTopic,
                    StartedAt = DateTime.UtcNow
                };
                _context.ChatSessions.Add(session);
                _context.SaveChanges();
            }

            return session;
        }

        // ---------------------------
        // Open chat with another user
        // ---------------------------
        public IActionResult ChatWith(long id)
        {
            var currentUserId = CurrentUserId;
            var targetUser = GetUserById(id);
            if (targetUser == null) return NotFound();

            var session = GetOrCreateChatSession(currentUserId, id);

            var viewModel = new ChatViewModel
            {
                TargetUser = targetUser,
                Messages = session.Messages.OrderBy(m => m.SentAt).ToList(),
                CurrentUserID = currentUserId,
                ChatSessionID = session.ChatSessionID
            };

            return View(viewModel);
        }

        // ---------------------------
        // Ajax: get messages in a session
        // ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetMessages(long sessionId)
        {
            var currentUserId = CurrentUserId;
            var isStudent = await _context.Students.AnyAsync(s => s.StudentID == currentUserId);

            var messages = await _context.ChatMessages
                .Where(m => m.ChatSessionID == sessionId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            ViewBag.IsCurrentUserStudent = isStudent;
            return PartialView("_ChatMessagesPartial", messages);
        }

        // ---------------------------
        // Send message
        // ---------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(long sessionId, string messageText)
        {
            var session = await _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => cs.ChatSessionID == sessionId);

            if (session == null) return NotFound();

            var currentUserId = CurrentUserId;

            if (string.IsNullOrWhiteSpace(messageText))
                return RedirectToAction("ChatWith", new { id = ResolveOtherUserId(session.Topic, currentUserId) });

            var isStudent = await _context.Students.AnyAsync(s => s.StudentID == currentUserId);

            var message = new ChatMessage
            {
                ChatSessionID = sessionId,
                MessageText = messageText,
                SentAt = DateTime.UtcNow,
                SenderStudentID = isStudent ? currentUserId : null,
                SenderTutorID = isStudent ? null : currentUserId
                // Optionally set Receiver* if you add direct addressing in UI
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            return RedirectToAction("ChatWith", new { id = ResolveOtherUserId(session.Topic, currentUserId) });
        }
    }

    // ============================
    // ViewModels (kept in this file)
    // ============================
    public class MessageView
    {
        public long CurrentUserID { get; set; }
        public long SelectedRecipientID { get; set; }

        public List<MessageUser> AllUsers { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();

        public string? SearchQuery { get; set; }
        public string? RoleFilter { get; set; }

        // Populated from DB (Option A)
        public IReadOnlyList<string> Groups { get; set; } = Array.Empty<string>();

        public MessageUser? TargetUser =>
            AllUsers?.FirstOrDefault(u => u.Id == SelectedRecipientID);
    }

    public class ChatViewModel
    {
        public MessageUser TargetUser { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();
        public long CurrentUserID { get; set; }
        public long ChatSessionID { get; set; }
    }
}
