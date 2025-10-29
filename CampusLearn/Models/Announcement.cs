using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("announcements")]
    public class Announcement
    {
        [Key]
        [Column("announcementid")]
        public int AnnouncementID { get; set; }

        [Required]
        [StringLength(255)]
        [Column("topic")]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [Column("discussion")]
        public string Discussion { get; set; } = string.Empty;

        // Enum replaced by CHECK constraint in PostgreSQL
        [Required]
        [Column("progress")]
        [StringLength(50)]
        public string Progress { get; set; } = "Fresh";

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Admin")]
        [Column("adminid")]
        public int AdminID { get; set; }

        // Navigation property
        public Admin Admin { get; set; }

        // Linked replies (1 announcement → many replies)
        public ICollection<ReplyAnnouncement>? Replies { get; set; }
    }
}

