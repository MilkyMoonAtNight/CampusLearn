using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class SessionRating
    {
        public long SessionID { get; set; }
        public Session Session { get; set; }

        public long RatingID { get; set; }
        public Rating Rating { get; set; }
    }
}
