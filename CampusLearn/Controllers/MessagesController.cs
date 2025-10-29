using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // Only groups for a specific student (use if you want per-user groups instead)
        private Task<List<string>> GetGroupsForStudentAsync(long studentId)
        {
            // If you actually want module names for the student, join via EnrollmentDegree -> DegreeModule -> TopicModule
            return _context.EnrollmentDegrees
                .Where(ed => ed.Enrollment.StudentID == studentId)
                .Select(ed => ed.Degree.DegreeName) // or .Select(ed => ed.Module.ModuleName) if you change the projection
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        }

        // ---------------------------
        // Build user list (Students + Tutors + Admins)
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
                    Groups = new List<string>()
                })
                .ToListAsync();

            var tutors = await _context.Tutors
                .Select(t => new MessageUser
                {
                    Id = t.TutorID,
                    FirstName = t.TutorName,
                    LastName = t.TutorSurname,
                    Email = null,            // add if you have tutor email column
                    PhoneNumber = null,      // add if you have tutor phone column
                    Role = "Tutor",
                    Groups = new List<string>()
                })
                .ToListAsync();

            var admins = await _context.Admins
                .Select(a => new MessageUser
                {
                    Id = a.AdminID,
                    FirstName = a.AdminName,
                    LastName = a.AdminSurname,
                    Email = a.AdminEmail,
                    PhoneNumber = a.AdminPhone,
                    Role = "Admin",
                    Groups = new List<string>()
                })
                .ToListAsync();

            return students.Concat(tutors).Concat(admins)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToList();
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
                    || (u.Email ?? "").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                    || (u.PhoneNumber ?? "").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                    || u.Id.ToString().Contains(searchQuery)
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                allUsers = allUsers.Where(u => u.Role == roleFilter).ToList();
            }

            var groups = await GetAllGroupsAsync(); // or GetGroupsForStudentAsync(CurrentUserId)

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

            return View("Index", model); // Views/Messages/Index.cshtml
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
                    Email = null,
                    PhoneNumber = null,
                    Role = "Tutor",
                    Groups = new List<string>()
                };

            var admin = _context.Admins.AsNoTracking().FirstOrDefault(a => a.AdminID == id);
            if (admin != null)
                return new MessageUser
                {
                    Id = admin.AdminID,
                    FirstName = admin.AdminName,
                    LastName = admin.AdminSurname,
                    Email = admin.AdminEmail,
                    PhoneNumber = admin.AdminPhone,
                    Role = "Admin",
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
        // Full-page chat (existing)
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

            return View(viewModel); // Views/Messages/ChatWith.cshtml
        }

        // ---------------------------
        // Ajax: get messages (legacy partial if you still use it)
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
        // Send (full-page)
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
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            return RedirectToAction("ChatWith", new { id = ResolveOtherUserId(session.Topic, currentUserId) });
        }

        // ---------------------------
        // Inline chat: load partial
        // ---------------------------
        [HttpGet]
        public IActionResult Thread(long id)
        {
            var currentUserId = CurrentUserId;
            var target = GetUserById(id);
            if (target == null) return NotFound();

            var session = GetOrCreateChatSession(currentUserId, id);

            var messages = _context.ChatMessages
                .Where(m => m.ChatSessionID == session.ChatSessionID)
                .OrderBy(m => m.SentAt)
                .ToList();

            var vm = new ChatViewModel
            {
                TargetUser = target,
                Messages = messages,
                CurrentUserID = currentUserId,
                ChatSessionID = session.ChatSessionID
            };

            return PartialView("_ChatThread", vm);
        }


        // ---------------------------
        // Inline chat: send + refresh partial
        // ---------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInline(long sessionId, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return BadRequest("Empty message.");

            var session = await _context.ChatSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(cs => cs.ChatSessionID == sessionId);

            if (session == null) return NotFound();

            var me = CurrentUserId;
            var isStudent = await _context.Students.AnyAsync(s => s.StudentID == me);

            var msg = new ChatMessage
            {
                ChatSessionID = sessionId,
                MessageText = text.Trim(),
                SentAt = DateTime.UtcNow,
                SenderStudentID = isStudent ? me : null,
                SenderTutorID = isStudent ? null : me
            };

            _context.ChatMessages.Add(msg);
            await _context.SaveChangesAsync();

            // fresh messages from DB (no duplicates)
            var messages = await _context.ChatMessages
                .Where(m => m.ChatSessionID == sessionId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            var targetId = ResolveOtherUserId(session.Topic, me);
            var target = GetUserById(targetId)!;

            var vm = new ChatViewModel
            {
                TargetUser = target,
                Messages = messages,
                CurrentUserID = me,
                ChatSessionID = sessionId
            };

            return PartialView("_ChatThread", vm);
        }

    }
}
