using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    public class StudentTutor
    {
        public long StudentID { get; set; }
        public Student Student { get; set; }

        public long TutorID { get; set; }
        public Tutors Tutor { get; set; }
    }
}
