using Microsoft.AspNetCore.Mvc;
using CampusLearn.Models; 

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
            return View();
        }
        private static int _nextId = 3;

        [HttpPost]
        public IActionResult Add(ForumTopic topic)
        {
            if (ModelState.IsValid)
            {
                topic.Id = _nextId++;
                _topics.Add(topic);
                return RedirectToAction("Index");
            }

            return View(topic);
        }
        public IActionResult Details(int id)
        {
            if (id < 0 || id >= _topics.Count)
                return NotFound();

            var topic = _topics[id];
            return View(topic);
        }
        [HttpPost]
        public IActionResult AddReply(int id, string author, string message)
        {
            if (id < 0 || id >= _topics.Count)
                return NotFound();

            var reply = new Reply { Author = author, Message = message };
            _topics[id].Replies.Add(reply);
            _topics[id].Contributions++;

            return RedirectToAction("Details", new { id });
        }


    }
}
