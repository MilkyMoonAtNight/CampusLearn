using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CampusLearn.Models
{
    public class SessionTutor
    {
        public long SessionID { get; set; }
        public Session Session { get; set; }

        public long TutorID { get; set; }
        public Tutors Tutor { get; set; }
    }
}
