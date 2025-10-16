using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CampusLearn.Controllers
{
    public class TopicsController : Controller
    {
        //This is an in-memory store only for demo purposes.
        private static readonly List<Topic> Topics = new();
        private static readonly List<Post> Posts = new();

        public IActionResult Index() => View(Topics.OrderByDescending(t => t.CreatedAt));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string content, string author)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content)) //not empty
            {
                TempData["Error"] = "Title and Content are required.";
                return RedirectToAction("Index");
            }


            var t = new Topic //object
            {
                TopicId = Guid.NewGuid().ToString(),
                Title = title.Trim(),
                Content = content.Trim(),
                Author = string.IsNullOrWhiteSpace(author) ? "Student123" : author.Trim(),
                CreatedAt = DateTime.Now
            };

            if (Topics.Any(t => t.Title.Trim().ToLower() == title.Trim().ToLower())) //checks if there is a topic with the same title, reduces redundancy 
            {
                TempData["Error"] = "There is a topic that has the same Topic Title";
                return RedirectToAction("Index");
            }
            Topics.Add(t);
            TempData["Message"] = "Topic created successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Detail(string id)
        {
            var topic = Topics.FirstOrDefault(t => t.TopicId == id);
            if (topic == null) return NotFound();

            var posts = Posts.Where(p => p.TopicId == id).OrderBy(p => p.CreatedAt).ToList();
            ViewBag.Posts = posts;
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReply(string topicId, string message, bool anonymous)
        {
            var topic = Topics.FirstOrDefault(t => t.TopicId == topicId);
            if (topic == null) return NotFound();

            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Message is required.";
                return RedirectToAction("Detail", new { id = topicId });
            }

            var p = new Post
            {
                PostId = Guid.NewGuid().ToString(),
                TopicId = topicId,
                Body = message.Trim(),
                Author = anonymous ? "Anonymous" : "Student123",
                CreatedAt = DateTime.Now
            };
            Posts.Add(p);
            TempData["Message"] = "Reply posted.";
            return RedirectToAction("Detail", new { id = topicId });
        }

        //simple models for demo
        public class Topic
        {
            public string TopicId { get; set; } = "";
            public string Title { get; set; } = "";
            public string Content { get; set; } = "";
            public string Author { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }

        public class Post
        {
            public string PostId { get; set; } = "";
            public string TopicId { get; set; } = "";
            public string Body { get; set; } = "";
            public string Author { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }

    }
}
