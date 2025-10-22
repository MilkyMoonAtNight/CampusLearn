namespace CampusLearn.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructor { get; set; }
        public int WeeksRemaining { get; set; }
        public int Completion { get; set; }
        public string ImagePath { get; set; }

        public List<WeekContent> WeeklyContent { get; set; } = new();
        public List<Announcement> Announcements { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
    }

    public class WeekContent
    {
        public int WeekNumber { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
    }

    public class Announcement
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.Now;
    }

    public class Assignment
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public DateTime DueDate { get; set; }
    }
}
