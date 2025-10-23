using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class MessageView
    {
        public string CurrentUser { get; set; }
        public string SelectedRecipient { get; set; }
        public List<User> AllUsers { get; set; }
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        [Required(ErrorMessage = "Sender name is required.")]
        [StringLength(50, ErrorMessage = "Sender name cannot exceed 50 characters.")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Recipient name is required.")]
        [StringLength(50, ErrorMessage = "Recipient name cannot exceed 50 characters.")]
        public string RecipientName { get; set; }

        [Required(ErrorMessage = "Message content cannot be empty.")]
        [StringLength(1000, ErrorMessage = "Message content is too long.")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public bool IsUser { get; set; }

        private static readonly List<Message> _messages = new();


        // Get conversation between two users
        public static List<Message> GetConversation(string userA, string userB)
        {
            return _messages
                .Where(m => (m.SenderName == userA && m.RecipientName == userB) ||
                            (m.SenderName == userB && m.RecipientName == userA))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }
        public static void SaveMessage(string senderName, string recipientName, string content, bool isUser)
        {
            _messages.Add(new Message
            {
                SenderName = senderName,
                RecipientName = recipientName,
                Content = content,
                Timestamp = DateTime.Now,
                IsUser = isUser
            });
        }

    }

}
