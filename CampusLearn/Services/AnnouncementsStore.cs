using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusLearn.Services
{
    // Simple in-memory store for demo/black-box testing
    public class AnnouncementsStore : IAnnouncementsStore
    {
        private readonly List<Announcement> _items = new()
        {
            new Announcement { Title="New submission deadline for PRG381", ModuleTag="PRG381", Body="Deadline extended to Friday due to maintenance.", CreatedAt=DateTime.Now.AddHours(-2) },
            new Announcement { Title="AI webinar registration open", ModuleTag="Campus", Body="Sign up for the upcoming AI webinar.", CreatedAt=DateTime.Now.AddHours(-7) },
            new Announcement { Title="Assignment 3 feedback available", ModuleTag="SEN381", Body="Feedback has been posted.", CreatedAt=DateTime.Now.AddDays(-1) },
        };

        public IReadOnlyList<Announcement> GetAll() => _items.OrderByDescending(a => a.CreatedAt).ToList();

        public void Add(string title, string moduleTag, string body)
        {
            _items.Insert(0, new Announcement
            {
                Title = title.Trim(),
                ModuleTag = string.IsNullOrWhiteSpace(moduleTag) ? "General" : moduleTag.Trim(),
                Body = body.Trim(),
                CreatedAt = DateTime.Now
            });
        }
    }
}

