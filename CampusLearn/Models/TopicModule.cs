using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CampusLearn.Models;
using System;


namespace CampusLearn.Models
{
    public class TopicModule
    {
        [Key]
        public int ModuleID { get; set; }

        [Required, MaxLength(255)]
        public string ModuleName { get; set; }

        public int? ClusterID { get; set; }
        public ModuleCluster? ModuleCluster { get; set; }

        public long? ModuleHeadID { get; set; }
        public Tutor? ModuleHead { get; set; }



        public ModulePlan? ModulePlan { get; set; }
        public ICollection<ModuleResource> ModuleResources { get; set; } = new List<ModuleResource>();
        public ICollection<ModuleAssignment> ModuleAssignments { get; set; } = new List<ModuleAssignment>();
        public ICollection<ModuleProject> ModuleProjects { get; set; } = new List<ModuleProject>();
        public ICollection<ModuleTest> ModuleTests { get; set; } = new List<ModuleTest>();
        public ICollection<ModuleWeek> ModuleWeeks { get; set; } = new List<ModuleWeek>();
        public ICollection<DegreeModule>? DegreeModules { get; set; } = new List<DegreeModule>();

    }
}
