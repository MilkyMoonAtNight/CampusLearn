using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class MessageView
    {
        public List<Message> Messages { get; set; }
    }
    public class Message
    {
        [Required(ErrorMessage = "Sender name is required.")]
        [StringLength(50, ErrorMessage = "Sender name cannot exceed 50 characters.")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Message content cannot be empty.")]
        [StringLength(1000, ErrorMessage = "Message content is too long.")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public bool IsUser { get; set; }
    }
        private static readonly List<Message> _messages = new();

        public static List<Message> GetMessagesForUser(string username)
        {
            return _messages
                .Where(m => m.SenderName == username || m.IsUser)
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        public static void SaveMessage(string senderName, string content, bool isUser)
        {
            _messages.Add(new Message
            {
                SenderName = senderName,
                Content = content,
                Timestamp = DateTime.Now,
                IsUser = isUser
            });
        }
    }

}
