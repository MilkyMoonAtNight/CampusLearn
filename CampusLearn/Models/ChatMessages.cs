using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    public class ChatMessages
    {
        [Key]
        public long ChatMessageID { get; set; }

        [Required]
        public long ChatSessionID { get; set; }

        [ForeignKey(nameof(ChatSessionID))]
        public ChatSession ChatSession { get; set; }

        [Required]
        public bool IsFromStudent { get; set; }

        [Required]
        public string MessageText { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
