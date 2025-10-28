using Microsoft.AspNetCore.Mvc;
using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Test()
        {
            return View("Test");
        }

        private readonly CampusLearnContext _context;

        public MessagesController(CampusLearnContext context)
        {
            _context = context;
        }
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
                Groups = new List<string>() // or assign based on subject
            }).ToList();

            return (students ?? new List<MessageUser>())
                .Concat(tutors ?? new List<MessageUser>())
                .ToList();

        }


        public async Task<IActionResult> Index(string searchQuery, string roleFilter)
        {
            var currentUserId = HttpContext.Session.GetInt32("LoggedInUserID");
            if (!currentUserId.HasValue)
                return RedirectToAction("Index", "LogIn");

            // Combine all user types into a unified list
            var students = await _context.Students
                .Where(s => s.StudentID != currentUserId.Value)
                .Select(s => new MessageUser
                {
                    ID = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Role = "Student"
                }).ToListAsync();

            var tutors = await _context.Tutors
                .Select(tu => new MessageUser
                {
                    ID = tu.TutorID,
                    FirstName = tu.TutorName,
                    LastName = tu.TutorSurname,
                    Role = "Tutor"
                }).ToListAsync();

            var allUsers = students.Concat(tutors).ToList();

            // Apply search
            if (!string.IsNullOrEmpty(searchQuery))
            {
                allUsers = allUsers.Where(u =>
                    u.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.ID.ToString().Contains(searchQuery)).ToList();
            }

            // Apply role filter
            if (!string.IsNullOrEmpty(roleFilter))
            {
                allUsers = allUsers.Where(u => u.Role == roleFilter).ToList();
            }

            var model = new MessageView
            {
                CurrentUserID = currentUserId.Value,
                SelectedRecipientID = 0,
                AllUsers = allUsers,
                Messages = new List<Message>(),
                SearchQuery = searchQuery,
                RoleFilter = roleFilter
            };

            return View("Index", model);
        }
        private long ExtractTutorIdFromTopic(string topic)
        {
            if (topic.StartsWith("ChatWith:") && long.TryParse(topic.Substring(9), out var id))
                return id;
            throw new Exception("Invalid topic format");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(long sessionId, string messageText)
        {
            var session = await _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => cs.ChatSessionID == sessionId);

            if (session == null)
                return NotFound();

            var tutorId = ExtractTutorIdFromTopic(session.Topic);

            var currentUserId = HttpContext.Session.GetInt32("LoggedInUserID");
            if (!currentUserId.HasValue)
                return RedirectToAction("Index", "LogIn");

            if (string.IsNullOrWhiteSpace(messageText))
            {
                ModelState.AddModelError("messageText", "Message cannot be empty.");
                return RedirectToAction("ChatWith", new { id = tutorId }); // ✅ correct redirect
            }

            var message = new ChatMessage
            {
                ChatSessionID = sessionId,
                IsFromStudent = true,
                MessageText = messageText,
                SentAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok(); // or return Json(new { success = true });

            return RedirectToAction("ChatWith", new { id = tutorId });

        }
        private long GetCurrentStudentId()
        {
            var id = HttpContext.Session.GetInt32("LoggedInUserID");
            if (!id.HasValue)
                throw new Exception("User not logged in");
            return id.Value;
        }

        private MessageUser GetUserById(long id)
        {
            var allUsers = GetAllUsers(); // returns List<MessageUser>
            if (allUsers == null)
                throw new Exception("User list is null");

            return allUsers?.FirstOrDefault(u => u.ID == id);
        }
        public IActionResult ChatWith(long id) // id = tutorId
        {
            var currentStudentId = GetCurrentStudentId(); // however you track logged-in student
            var targetUser = GetUserById(id); // returns MessageUser or Tutor

            if (targetUser == null)
                return NotFound();

            var session = GetOrCreateChatSession(currentStudentId, id);

            var viewModel = new ChatViewModel
            {
                TargetUser = targetUser,
                Messages = session.Messages.OrderBy(m => m.SentAt).ToList(),
                CurrentUserID = currentStudentId,
                ChatSessionID = session.ChatSessionID
            };

            return View(viewModel);
        }
        private ChatSession GetOrCreateChatSession(long studentId, long tutorId)
        {
            var session = _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefault(cs => cs.StudentID == studentId && cs.Topic == $"ChatWith:{tutorId}");

            if (session == null)
            {
                session = new ChatSession
                {
                    StudentID = studentId,
                    Topic = $"ChatWith:{tutorId}",
                    StartedAt = DateTime.UtcNow
                };
                _context.ChatSessions.Add(session);
                _context.SaveChanges();
            }

            return session;
        }


    }
}