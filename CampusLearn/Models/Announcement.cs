using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("Announcements")]
    public class Announcement
    {
        [Key]
        [Column("AnnouncementID")]
        public int AnnouncementID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        public string Discussion { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Progress { get; set; } = "Fresh";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? AdminID { get; set; }

        // optional: link to admin for name display
        [ForeignKey("AdminID")]
        public Admin? Admin { get; set; }
        public ICollection<ReplyAnnouncement> Replies { get; set; } = new List<ReplyAnnouncement>();
    }
}


