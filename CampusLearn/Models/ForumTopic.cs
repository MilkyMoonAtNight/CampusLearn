using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CampusLearn.Models
{
    public class ForumTopic
    {
        //public List<Reply> Replies { get; set; } = new List<Reply>();
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty; //added string.Empty

        [Required]
        public string Subject { get; set; } = string.Empty; //added string.Empty

        [Required]
        public string Description { get; set; } = string.Empty; //added string.Empty

        public string Author { get; set; } = string.Empty; //added string.Empty

        public int Contributions { get; set; } = 0;

        public string Progress { get; set; } = "Fresh";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Reply> Replies { get; set; } = new List<Reply>(); //Added new List<Reply>()
    }
    public class Reply
    {
        public int ReplyID { get; set; }
        public int ForumTopicId { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ForumTopic? ForumTopic { get; set; }
    }

}
