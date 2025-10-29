using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("moduletests")]
    public class ModuleTest
    {
        [Key]
        [Column("testid")]
        public int TestID { get; set; }

        [Required]
        [Column("moduleid")]
        public int ModuleID { get; set; }

        [Required]
        [StringLength(255)]
        [Column("testtitle")]
        public string TestTitle { get; set; } = string.Empty;

        [Required]
        [Column("testweek")]
        public int TestWeek { get; set; }

        [Required]
        [Column("testdate")]
        public DateTime TestDate { get; set; }

        // Navigation
        public TopicModule? Module { get; set; }
    }
}
