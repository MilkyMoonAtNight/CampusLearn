using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class SessionStudent
    {
        public long SessionID { get; set; }
        public Session Session { get; set; }

        public long StudentID { get; set; }
        public Student Student { get; set; }
    }
}
