using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    public class ModuleResource
    {
        [Key]
        public int ResourceID { get; set; }

        [ForeignKey("Module")]
        public int ModuleID { get; set; }
        public TopicModule Module { get; set; }

        [MaxLength(40)]
        public string ResourceType { get; set; }

        [Required]
        public string ResourceURL { get; set; }
    }
}
