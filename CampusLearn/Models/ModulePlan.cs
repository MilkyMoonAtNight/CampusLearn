using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("moduleplan")]
    public class ModulePlan
    {
        [Key]
        [Column("moduleid")]
        public int ModuleID { get; set; }

        [Required]
        [Column("totalweeks")]
        public int TotalWeeks { get; set; }

        [Required]
        [Column("testsallowed")]
        public int TestsAllowed { get; set; }

        [Required]
        [Column("assignmentsrequired")]
        public int AssignmentsRequired { get; set; }

        [Required]
        [Column("projectsrequired")]
        public int ProjectsRequired { get; set; }

        // Navigation
        public TopicModule? Module { get; set; }
    }
}

