using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class ChatSession
    {
        public long ChatSessionID { get; set; }

        [Required]
        public long StudentID { get; set; }

        public string? Topic { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public Student Student { get; set; } = null!;
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
