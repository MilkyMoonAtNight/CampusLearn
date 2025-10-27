using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class ChatSession
    {
        [Key]
        public long ChatSessionID { get; set; }

        [Required]
        public long StudentID { get; set; }

        [Required]
        public DateTime StartedAt { get; set; } = DateTime.Now;
        
        public DateTime? EndedAt { get; set; }

        [MaxLength(100)]
        public string Topic { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public ICollection<ChatMessage> Messages { get; set; }
    }
}
