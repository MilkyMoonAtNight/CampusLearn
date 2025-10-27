using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("chatmessages")]
    public class Message
    {
        [Key]
        [Column("messageid")]
        public int MessageID { get; set; }

        [Required]
        [Column("senderid")]
        public int SenderID { get; set; }

        [Required]
        [Column("recipientid")]
        public int RecipientID { get; set; }

        [Required]
        [Column("messagetext")]
        [StringLength(1000)]
        public string Content { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Optional: navigation properties if you have Student or Tutor entities
        public Student Sender { get; set; }
        public Student Recipient { get; set; }
    }
}
