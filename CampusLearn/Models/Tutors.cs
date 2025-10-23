using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    public class Tutors
    {
        [Key]
        public long TutorID { get; set; }

        [Required, MaxLength(255)]
        public string TutorName { get; set; }

        [Required, MaxLength(255)]
        public string TutorSurname { get; set; }

        [ForeignKey("Speciality")]
        public int? SpecialityID { get; set; }
        public Speciality? Speciality { get; set; }
        public ICollection<StudentTutor> StudentTutors { get; set; } = new List<StudentTutor>();
        public ICollection<SessionTutor> SessionTutors { get; set; } = new List<SessionTutor>();
        public ICollection<TopicModule> TopicModules { get; set; } = new List<TopicModule>();
    }
}
