using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CampusLearn.Models
{
    public class TopicModule
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }

        public int? ClusterID { get; set; }
        public ModuleCluster ModuleCluster { get; set; }

        public long? ModuleHeadID { get; set; }
        public Tutors ModuleHead { get; set; }

        // <-- Add this collection property to fix the error
        public ICollection<DegreeModule> DegreeModules { get; set; } = new List<DegreeModule>();

        public ICollection<ModuleResource> ModuleResources { get; set; } = new List<ModuleResource>();
    }

 
}
