using Microsoft.EntityFrameworkCore;

namespace CampusLearn.ViewModels
{
    [Keyless]
    public class ModuleItemCountDto
    {
        public string ModuleName { get; set; } = "";
        public int Items { get; set; }
    }

    [Keyless]
    public class DashboardStatsDto
    {
        public int TotalModules { get; set; }
        public int ActiveEnrollments { get; set; }
        public int PendingAssignments { get; set; }
        public int UpcomingTests { get; set; }
    }

    [Keyless]
    public class AnnouncementItemDto
    {
        public int AnnouncementId { get; set; }
        public string Topic { get; set; } = "";
        public string Discussion { get; set; } = "";
        public string Progress { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
    // For the monthly calendar
    [Keyless]
    public class CalendarEventDto
    {
        public DateTime EventDate { get; set; }
        public string Title { get; set; } = "";
        public string ModuleName { get; set; } = "";
        public string Type { get; set; } = ""; // "Assignment" | "Test"
    }

    [Keyless]
    public class StudentOptionDto
    {
        public long StudentId { get; set; }
        public string DisplayName { get; set; } = "";
    }
}
