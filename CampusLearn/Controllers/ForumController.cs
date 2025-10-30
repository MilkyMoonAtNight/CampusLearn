using Microsoft.AspNetCore.Mvc;
using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CampusLearn.Controllers
{
    public class ForumController : Controller
    {
        private readonly CampusLearnContext _context;

        public ForumController(CampusLearnContext context)
        {
            _context = context;
        }

        // =========================
        // Index - list all forum topics
        // =========================
        public async Task<IActionResult> Index()
        {
            var topics = await _context.ForumTopics
                .Include(t => t.Replies)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(topics);
        }

        // =========================
        // Add (GET)
        // =========================
        [HttpGet]
        public IActionResult Add()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedInUser")))
                return RedirectToAction("Index", "LogIn");

            return View();
        }

        // =========================
        // Add (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Title,Subject,Description")] ForumTopic topic)
        {
            if (!ModelState.IsValid)
                return View(topic);

            topic.CreatedAt = DateTime.UtcNow;
            topic.Progress = "Fresh";
            topic.Contributions = 0;

            try
            {
                _context.ForumTopics.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while creating the topic.");
                return View(topic);
            }
        }

        // =========================
        // Details (Topic with Replies)
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var topic = await _context.ForumTopics
                .Include(t => t.Replies)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
                return NotFound();

            return View(topic);
        }

        // =========================
        // Add Reply
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReply(int id, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ModelState.AddModelError("Message", "Message cannot be empty.");
                var topic = await _context.ForumTopics
                    .Include(t => t.Replies)
                    .FirstOrDefaultAsync(t => t.Id == id);
                return View("Details", topic);
            }

            var author = HttpContext.Session.GetString("LoggedInUser") ?? "Anonymous";

            var reply = new Reply
            {
                ForumTopicId = id,
                Author = author,
                Message = message,
                PostedAt = DateTime.UtcNow // ✅ Always use UTC for PostgreSQL
            };

            try
            {
                _context.Replies.Add(reply);

                // Update contribution count
                var topic = await _context.ForumTopics.FirstOrDefaultAsync(t => t.Id == id);
                if (topic != null)
                {
                    topic.Contributions++;
                    _context.ForumTopics.Update(topic);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding reply: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving your reply.");
                var topicForError = await _context.ForumTopics
                    .Include(t => t.Replies)
                    .FirstOrDefaultAsync(t => t.Id == id);
                return View("Details", topicForError);
            }
        }
    }
}
