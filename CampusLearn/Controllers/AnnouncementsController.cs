using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusLearn.Controllers
{
    /// Simple Announcements page for black-box testing.
    /// In-memory data
    public class AnnouncementsController : Controller
    {
        // In-memory store for demo
        private static readonly List<Announcement> Announcements = new()
        {
            new Announcement { Title="New submission deadline for PRG381", ModuleTag="PRG381", Body="Deadline extended to Friday due to maintenance.", CreatedAt=DateTime.Now.AddHours(-2) },
            new Announcement { Title="AI webinar registration open", ModuleTag="Campus", Body="Sign up for the upcoming AI webinar.", CreatedAt=DateTime.Now.AddHours(-7) },
            new Announcement { Title="Assignment 3 feedback available", ModuleTag="SEN381", Body="Feedback has been posted.", CreatedAt=DateTime.Now.AddDays(-1) },
        };

        public IActionResult Index(string sort = "newest")
        {
            IEnumerable<Announcement> items = Announcements;
            items = sort == "oldest" ? items.OrderBy(a => a.CreatedAt) : items.OrderByDescending(a => a.CreatedAt);
            ViewBag.Sort = sort;
            return View(items.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string moduleTag, string body)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                TempData["Error"] = "Title and Body are required.";
                return RedirectToAction("Index");
            }
            Announcements.Insert(0, new Announcement
            {
                Title = title.Trim(),
                ModuleTag = string.IsNullOrWhiteSpace(moduleTag) ? "General" : moduleTag.Trim(),
                Body = body.Trim(),
                CreatedAt = DateTime.Now
            });
            TempData["Message"] = "Announcement posted.";
            return RedirectToAction("Index");
        }

        public class Announcement
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Title { get; set; } = "";
            public string ModuleTag { get; set; } = "";
            public string Body { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }
    }
}
