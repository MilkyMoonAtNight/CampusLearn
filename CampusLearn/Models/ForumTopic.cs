using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class ForumTopic
    {
        public List<Reply> Replies { get; set; } = new List<Reply>();
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Subject { get; set; } 

        [Required]
        public string Description { get; set; }

        public int Contributions { get; set; } = 0;

        public string Progress { get; set; } = "Fresh";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    public class Reply
    {
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.Now;
    }

}
