using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CampusLearn.Models
{
    public class EnrollmentDegree
    {
        public long EnrollmentID { get; set; }
        public int DegreeID { get; set; }

        public Enrollment Enrollment { get; set; }  // <-- points back to Enrollment
        public Degree Degree { get; set; }
    }
}
