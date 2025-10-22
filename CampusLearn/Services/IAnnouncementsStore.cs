using System;
using System.Collections.Generic;

namespace CampusLearn.Services
{
    public interface IAnnouncementsStore
    {
        IReadOnlyList<Announcement> GetAll();
        void Add(string title, string moduleTag, string body);
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
