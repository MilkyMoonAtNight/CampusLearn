using System.Collections.Generic;

namespace CampusLearn.Models
{
    public class MessageUser
    {
        public long ID { get; set; } // StudentID, TeacherID, or TutorID
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; } 
    }

    public class MessageView
    {
        public int CurrentUserID { get; set; }
        public int SelectedRecipientID { get; set; }

        public List<MessageUser> AllUsers { get; set; } // Or List<Tutor> if tutors are messaging
        public List<Message> Messages { get; set; }
        public string SearchQuery { get; set; }
        public string RoleFilter { get; set; }
    }
}