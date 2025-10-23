using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    public class ModuleResource
    {
        [Key]
        public int ResourceID { get; set; }

        [ForeignKey("TopicModule")]
        public int ModuleID { get; set; }
        public TopicModule? TopicModule { get; set; }

        [MaxLength(40)]
        public string? ResourceType { get; set; }

        [Required]
        public string ResourceURL { get; set; }
    }
}
