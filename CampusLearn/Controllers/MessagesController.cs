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

            return View("Chat", model);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(long sessionId, string messageText)
        {
            var currentUserId = HttpContext.Session.GetInt32("LoggedInUserID");
            if (!currentUserId.HasValue)
                return RedirectToAction("Index", "LogIn");

            var message = new ChatMessage
            {
                ChatSessionID = sessionId,
                IsFromStudent = true,
                MessageText = messageText,
                SentAt = DateTime.Now
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { sessionId });
        }

    }
}