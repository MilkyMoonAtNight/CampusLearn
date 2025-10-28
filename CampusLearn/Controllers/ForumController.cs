using Microsoft.AspNetCore.Mvc;
using CampusLearn.Models; 
using Microsoft.AspNetCore.Http;

namespace CampusLearn.Controllers
{
    public class ForumController : Controller
    {
        private static List<ForumTopic> _topics = new List<ForumTopic>
        {
            new ForumTopic { Id = 0, Title = "PRG162", Description = "Will we use C# ever again?", Contributions = 24, Progress = "Solved" },
            new ForumTopic { Id = 1, Title = "Databases", Description = "Databases moving to NoSQL", Contributions = 46, Progress = "Pending" },
            new ForumTopic { Id = 2, Title = "4th Year internships", Description = "What is the starting salary", Contributions = 12, Progress = "Fresh" }
        };

        public IActionResult Index()
        {
            return View(_topics);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedInUser")))
            {
                return RedirectToAction("Index", "LogIn");
            }
            return View();
        }

        private static int _nextId = 3;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ForumTopic topic)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("Validation errors: " + string.Join(", ", errors));
                return View(topic);
            }

            topic.Id = _nextId++;
            topic.Replies = new List<Reply>();
            topic.Author = HttpContext.Session.GetString("LoggedInUser") ?? "Anonymous";
            _topics.Add(topic);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            if (id < 0 || id >= _topics.Count)
                return NotFound();

            var topic = _topics[id];
            return View(topic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReply(int id, string message)
        {
            if (id < 0 || id >= _topics.Count)
                return NotFound();

            if(string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("Message", "Message cannot be empty.");
                return View("Details", _topics[id]);
            }

            var author = HttpContext.Session.GetString("LoggedInUser") ?? "Anonymous";

            var reply = new Reply
            {
                ForumTopicId = id,
                Author = author,
                Message = message,
                PostedAt = DateTime.Now
            };

            _topics[id].Replies.Add(reply);
            _topics[id].Contributions++;

            return RedirectToAction("Details", new { id });
        }
    }
}
