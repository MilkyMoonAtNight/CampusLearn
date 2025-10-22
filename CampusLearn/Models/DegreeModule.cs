using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CampusLearn.Models
{
    public class DegreeModule
    {
        public int DegreeID { get; set; }
        public Degree Degree { get; set; }

        public int ModuleID { get; set; }
        public TopicModule Module { get; set; }
    }
}
