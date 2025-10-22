using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CampusLearn.Models
{
    public class Degree
    {
        public int DegreeID { get; set; }
        public string DegreeName { get; set; }
        public int FacultyID { get; set; }
        public Faculty Faculty { get; set; }

        public ICollection<DegreeModule> DegreeModules { get; set; } = new List<DegreeModule>();
        public ICollection<EnrollmentDegree> EnrollmentDegrees { get; set; } = new List<EnrollmentDegree>(); // <-- add this
    }
}
