using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("replyannouncements")]
    public class ReplyAnnouncement
    {
        [Key]
        [Column("replyid")]
        public int ReplyID { get; set; }

        [ForeignKey("Announcement")]
        [Column("announcementid")]
        public int AnnouncementID { get; set; }

        [Required]
        [Column("replytext")]
        public string ReplyText { get; set; } = string.Empty;

        [Column("postedat")]
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Announcement? Announcement { get; set; }
    }
}
