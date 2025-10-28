using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    public class ChatMessage
    {
        [Key]
        public long ChatMessageID { get; set; }

        [Required]
        public long ChatSessionID { get; set; }

        public ChatSession ChatSession { get; set; }

        [Required]
        public bool IsFromStudent { get; set; }

        [Required]
        public string MessageText { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        
    }
}
