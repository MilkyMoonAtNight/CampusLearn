using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace CampusLearn.Models
{
    public class MessageView
    {
        public List<Message> Messages { get; set; }
    }
    public class Message
    {
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsUser { get; set; } // true if current user, false if other party
    }

}
