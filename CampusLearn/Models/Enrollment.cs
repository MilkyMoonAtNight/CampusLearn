using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CampusLearn.Models
{
    public class Enrollment
    {
        public long EnrollmentID { get; set; }
        public long StudentID { get; set; }
        public Student Student { get; set; }
        public DateTime EnrollmentDate { get; set; }

        // Add this navigation property for the join table
        public ICollection<EnrollmentDegree> EnrollmentDegrees { get; set; } = new List<EnrollmentDegree>();
    }
}
