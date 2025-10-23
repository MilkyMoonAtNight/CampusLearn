using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace CampusLearn.Models
{
    public class Student
    {
        public long StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalEmail { get; set; }
        public string Phone { get; set; }

        public string PasswordHash { get; set; }

        // <-- Add this
        

        public ICollection<StudentTutor> StudentTutors { get; set; } = new List<StudentTutor>();
        public ICollection<SessionStudent> SessionStudents { get; set; } = new List<SessionStudent>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
    }
}
