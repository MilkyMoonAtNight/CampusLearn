using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class ChatMessage
    {
        public long ChatMessageID { get; set; }

        [Required]
        public long ChatSessionID { get; set; }

        [Required]
        public string MessageText { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        // Optional FKs (match DB nullability)
        public long? SenderStudentID { get; set; }
        public long? SenderTutorID { get; set; }
        public long? ReceiverStudentID { get; set; }
        public long? ReceiverTutorID { get; set; }

        // Navigations
        public ChatSession ChatSession { get; set; } = null!;
        public Student? SenderStudent { get; set; }
        public Tutor? SenderTutor { get; set; }
        public Student? ReceiverStudent { get; set; }
        public Tutor? ReceiverTutor { get; set; }
    }

}
