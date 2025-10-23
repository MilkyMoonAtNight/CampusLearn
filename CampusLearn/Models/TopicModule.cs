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

        [ForeignKey("ModuleCluster")]
        public int? ClusterID { get; set; }
        public ModuleCluster? ModuleCluster { get; set; }

        [ForeignKey("ModuleHead")]
        public long? ModuleHeadID { get; set; }
        public Tutors? ModuleHead { get; set; }

        public ICollection<DegreeModule>? DegreeModules { get; set; }
        public ICollection<ModuleResource>? ModuleResources { get; set; }






    }
}
