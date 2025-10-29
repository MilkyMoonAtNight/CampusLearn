using Microsoft.AspNetCore.Mvc;
using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    public class AuthenticatedController : Controller
    {
        protected int CurrentUserId =>
            HttpContext.Session.GetInt32("LoggedInUserID")
            ?? throw new UnauthorizedAccessException("User not logged in");
    }

    public class MessagesController : AuthenticatedController
    {
        private readonly CampusLearnContext _context;

        public MessagesController(CampusLearnContext context)
        {
            _context = context;
        }

        public IActionResult Test()
        {
            return View("Test");
        }

        // Combine all users (students + tutors)
        private List<MessageUser> GetAllUsers()
        {
            var students = _context.Students.Select(s => new MessageUser
            {
                ID = s.StudentID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.PersonalEmail,
                PhoneNumber = s.Phone,
                Role = "Student",
                Groups = new List<string> { "PRG381", "LPR381", "INF281", "AOT300" }
            }).ToList();

            var tutors = _context.Tutors.Select(t => new MessageUser
            {
                ID = t.TutorID,
                FirstName = t.TutorName,
                LastName = t.TutorSurname,
                Role = "Tutor",
                Groups = new List<string>() // optional if needed
            }).ToList();

            return (students ?? new List<MessageUser>())
                .Concat(tutors ?? new List<MessageUser>())
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToList();
        }

        // User list with search and filters
        public async Task<IActionResult> Index(string searchQuery, string roleFilter)
        {
            var currentUserId = CurrentUserId;

            var students = await _context.Students
                .Where(s => s.StudentID != currentUserId)
                .Select(s => new MessageUser
                {
                    ID = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Role = "Student"
                }).ToListAsync();

            var tutors = await _context.Tutors
                .Select(t => new MessageUser
                {
                    ID = t.TutorID,
                    FirstName = t.TutorName,
                    LastName = t.TutorSurname,
                    Role = "Tutor"
                }).ToListAsync();

            var allUsers = students.Concat(tutors).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                allUsers = allUsers.Where(u =>
                    (u.FirstName ?? "").Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    (u.LastName ?? "").Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.ID.ToString().Contains(searchQuery)).ToList();
            }

            if (!string.IsNullOrEmpty(roleFilter))
            {
                allUsers = allUsers.Where(u => u.Role == roleFilter).ToList();
            }

            var model = new MessageView
            {
                CurrentUserID = currentUserId,
                SelectedRecipientID = 0,
                AllUsers = allUsers,
                Messages = new List<Message>(),
                SearchQuery = searchQuery,
                RoleFilter = roleFilter
            };

            return View("Index", model);
        }

        private long ResolveOtherUserId(string topic, long currentUserId)
        {
            if (topic.StartsWith("ChatBetween:"))
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

        // Send message
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(long sessionId, string messageText)
        {
            var session = await _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => cs.ChatSessionID == sessionId);

            if (session == null)
                return NotFound();

            var currentUserId = CurrentUserId;

            if (string.IsNullOrWhiteSpace(messageText))
                return RedirectToAction("ChatWith", new { id = ResolveOtherUserId(session.Topic, currentUserId) });

            bool isStudent = await _context.Students.AnyAsync(s => s.StudentID == currentUserId);

            var message = new ChatMessage
            {
                ChatSessionID = sessionId,
                IsFromStudent = isStudent,
                MessageText = messageText,
                SentAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            return RedirectToAction("ChatWith", new { id = ResolveOtherUserId(session.Topic, currentUserId) });
        }

        // Get user directly
        private MessageUser GetUserById(long id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id);
            if (student != null)
                return new MessageUser
                {
                    ID = student.StudentID,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.PersonalEmail,
                    PhoneNumber = student.Phone,
                    Role = "Student"
                };

            var tutor = _context.Tutors.FirstOrDefault(t => t.TutorID == id);
            if (tutor != null)
                return new MessageUser
                {
                    ID = tutor.TutorID,
                    FirstName = tutor.TutorName,
                    LastName = tutor.TutorSurname,
                    Role = "Tutor"
                };

            return null;
        }

        // Open chat with another user
        public IActionResult ChatWith(long id)
        {
            var currentUserId = CurrentUserId;
            var targetUser = GetUserById(id);

            if (targetUser == null)
                return NotFound();

            var session = GetOrCreateChatSession(currentUserId, id);
            bool isStudent = _context.Students.Any(s => s.StudentID == currentUserId);

            var viewModel = new ChatViewModel
            {
                TargetUser = targetUser,
                Messages = session.Messages.OrderBy(m => m.SentAt).ToList(),
                CurrentUserID = currentUserId,
                ChatSessionID = session.ChatSessionID
            };

            return View(viewModel);
        }

        private ChatSession GetOrCreateChatSession(long userAId, long userBId)
        {
            var normalizedTopic = $"ChatBetween:{Math.Min(userAId, userBId)}:{Math.Max(userAId, userBId)}";

            var session = _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefault(cs => cs.Topic == normalizedTopic);

            if (session == null)
            {
                session = new ChatSession
                {
                    StudentID = userAId,
                    //TutorID = userBId, // Requires DB column
                    Topic = normalizedTopic,
                    StartedAt = DateTime.UtcNow
                };
                _context.ChatSessions.Add(session);
                _context.SaveChanges();
            }

            return session;
        }

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
    }
}
