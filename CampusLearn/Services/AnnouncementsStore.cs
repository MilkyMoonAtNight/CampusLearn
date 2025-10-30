using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CampusLearn.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Services
{
    // EF-backed store: reads from your Postgres Announcements table
    public class AnnouncementsStore : IAnnouncementsStore
    {
        private readonly CampusLearnContext _db;
        public AnnouncementsStore(CampusLearnContext db) => _db = db;

        public IReadOnlyList<Announcement> GetAll()
        {
            // Pull newest first; map DB -> sidebar VM
            var items = _db.Announcements
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new Announcement
                {
                    Id = a.AnnouncementID,
                    Title = a.Topic,
                    Body = a.Discussion,
                    CreatedAt = a.CreatedAt,
                    // Optional badge derived from the title like "PRG381"
                    ModuleTag = TryBadgeFromTitle(a.Topic)
                })
                .ToList();

            return items;
        }

        public void Add(string title, string moduleTag, string body)
        {
            // Create a DB row; ModuleTag is just a visual badge so we don’t store it.
            _db.Announcements.Add(new Models.Announcement
            {
                Topic = title?.Trim() ?? "",
                Discussion = body?.Trim() ?? "",
                Progress = "Fresh",
                CreatedAt = DateTime.UtcNow,
                AdminID = null
            });
            _db.SaveChanges();
        }

        private static string TryBadgeFromTitle(string? title)
        {
            // e.g. "PRG381: New deadline" => PRG381
            if (string.IsNullOrWhiteSpace(title)) return "";
            var m = Regex.Match(title, @"^(?<tag>[A-Z]{3,6}\d{0,3})\b");
            return m.Success ? m.Groups["tag"].Value : "";
        }
    }
}
