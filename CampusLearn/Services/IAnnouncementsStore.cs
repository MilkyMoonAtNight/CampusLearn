using System;
using System.Collections.Generic;

namespace CampusLearn.Services
{
    public interface IAnnouncementsStore
    {
        IReadOnlyList<Announcement> GetAll();
        void Add(string title, string moduleTag, string body);
    }

    // View-model used by the sidebar
    public class Announcement
    {
        public int Id { get; set; }              // maps to Announcements.AnnouncementID
        public string Title { get; set; } = "";  // maps to Topic
        public string ModuleTag { get; set; } = ""; // optional badge (derived from Title)
        public string Body { get; set; } = "";   // maps to Discussion
        public DateTime CreatedAt { get; set; }  // maps to CreatedAt
    }
}
