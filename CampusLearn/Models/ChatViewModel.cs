namespace CampusLearn.Models
{
    public class ChatViewModel
    {
        public MessageUser TargetUser { get; set; }
        public List<ChatMessage> Messages { get; set; }
        public long CurrentUserID { get; set; }
        public long ChatSessionID { get; set; }
        public bool IsCurrentUserStudent { get; set; } 
    }

}
