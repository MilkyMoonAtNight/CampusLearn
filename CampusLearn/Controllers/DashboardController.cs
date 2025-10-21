using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusLearn.Controllers
{
    /// <summary>
    /// Simple demo Dashboard controller to support black-box UI testing.
    /// Uses in-memory data for Announcements/Assignments/Tests.
    /// Replace with DB when ready.
    /// </summary>
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var vm = new DashboardViewModel
            {
                UserName = "John Doe",
                LastAccess = DateTime.Now.AddMinutes(-37),
                Announcements = DemoData.Announcements.Take(3).ToList(),
                UpcomingTests = DemoData.Tests
                    .Where(t => t.DueDate >= DateTime.Today).OrderBy(t => t.DueDate).Take(3).ToList(),
                UpcomingAssignments = DemoData.Assignments
                    .Where(a => a.DueDate >= DateTime.Today).OrderBy(a => a.DueDate).Take(3).ToList(),
                QuickLinks = new List<QuickLink>
                {
                    new QuickLink("Topics", "Topics", "Index"),
                    new QuickLink("Announcements", "Home", "Privacy"), // swap to your real page later
                    new QuickLink("Grades", "Home", "Privacy")
                }
            };
            return View(vm);
        }

        // ---- ViewModels ----
        public class DashboardViewModel
        {
            public string UserName { get; set; } = "Student";
            public DateTime LastAccess { get; set; }
            public List<Announcement> Announcements { get; set; } = new();
            public List<PlannedItem> UpcomingTests { get; set; } = new();
            public List<PlannedItem> UpcomingAssignments { get; set; } = new();
            public List<QuickLink> QuickLinks { get; set; } = new();
        }

        public record QuickLink(string Label, string Controller, string Action);

        public class Announcement
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Title { get; set; } = "";
            public string Module { get; set; } = "";
            public string Body { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }

        public class PlannedItem
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Title { get; set; } = "";
            public string Module { get; set; } = "";
            public DateTime DueDate { get; set; }
            public string Type { get; set; } = ""; // Test/Assignment
        }

        /// <summary>
        /// Demo seed data (in-memory)
        /// </summary>
        static class DemoData
        {
            public static readonly List<Announcement> Announcements = new()
            {
                new Announcement { Title="New submission deadline for PRG381", Module="PRG381", Body="Deadline extended to Friday due to maintenance.", CreatedAt= DateTime.Now.AddHours(-2) },
                new Announcement { Title="AI webinar registration open", Module="Campus", Body="Sign up for AI webinar this weekend.", CreatedAt= DateTime.Now.AddHours(-7) },
                new Announcement { Title="Assignment 3 feedback available", Module="SEN381", Body="Feedback for A3 has been posted.", CreatedAt= DateTime.Now.AddDays(-1) },
                new Announcement { Title="Network outage notice", Module="Campus", Body="Brief outage scheduled tonight 23:00–23:30.", CreatedAt= DateTime.Now.AddHours(-20) },
            };

            public static readonly List<PlannedItem> Tests = new()
            {
                new PlannedItem { Title="SEN_Week3", Module="SEN381", DueDate=DateTime.Today.AddDays(1).AddHours(10), Type="Test" },
                new PlannedItem { Title="SEN_Week4", Module="SEN381", DueDate=DateTime.Today.AddDays(7).AddHours(9), Type="Test" },
            };

            public static readonly List<PlannedItem> Assignments = new()
            {
                new PlannedItem { Title="SEN_Assignment5", Module="SEN381", DueDate=DateTime.Today.AddDays(3), Type="Assignment" },
                new PlannedItem { Title="SEN_Project_Task2", Module="SEN381", DueDate=DateTime.Today.AddDays(5), Type="Assignment" },
            };
        }
    }
}

