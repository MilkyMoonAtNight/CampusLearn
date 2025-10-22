using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    public class Tutors
    {
        public long TutorID { get; set; }
        public string TutorName { get; set; }
        public string TutorSurname { get; set; }

        public int? SpecialityID { get; set; }
        public Speciality Speciality { get; set; }

        public ICollection<StudentTutor> StudentTutors { get; set; } = new List<StudentTutor>();
        public ICollection<SessionTutor> SessionTutors { get; set; } = new List<SessionTutor>();
    }
}
