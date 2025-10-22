using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace CampusLearn.Models
{
    public class Session
    {
        public long SessionID { get; set; }
        public string SessionTopic { get; set; }

        public ICollection<SessionTutor> SessionTutors { get; set; } = new List<SessionTutor>();
        public ICollection<SessionStudent> SessionStudents { get; set; } = new List<SessionStudent>();
        public ICollection<SessionRating> SessionRatings { get; set; } = new List<SessionRating>();
    
    }
}
