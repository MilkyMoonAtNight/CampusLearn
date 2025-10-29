using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("moduleweek")]
    public class ModuleWeek
    {
        [Key]
        [Column("weekid")]
        public int WeekID { get; set; }

        [Required]
        [Column("moduleid")]
        public int ModuleID { get; set; }

        [Required]
        [Column("weeknumber")]
        public int WeekNumber { get; set; }

        // Navigation
        public TopicModule? Module { get; set; }
        public ICollection<WeekContent>? Contents { get; set; }
    }
}

