using System.Collections.Generic;

namespace CampusLearn.Models
{
    public class MessageUserView
    {
        public long Id { get; set; } // rename from ID → Id

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Groups { get; set; } = new();
        public string Role { get; set; }
    }


    public class MessageView
    {
        public long CurrentUserID { get; set; }
        public long SelectedRecipientID { get; set; }
        public List<MessageUser> AllUsers { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();
        public string? SearchQuery { get; set; }
        public string? RoleFilter { get; set; }
        public IReadOnlyList<string> Groups { get; set; } = Array.Empty<string>();
        public MessageUser? TargetUser => AllUsers?.FirstOrDefault(u => u.Id == SelectedRecipientID);
    }
}