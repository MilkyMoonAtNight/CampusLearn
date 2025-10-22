using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class ModuleCluster
    {
        [Key]
        public int ClusterID { get; set; }

        [Required, MaxLength(100)]
        public string ClusterName { get; set; }

        public ICollection<TopicModule> Modules { get; set; }
    }
}
