using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models.ViewModels
{
    public enum ForumProgress
    {
        Fresh,
        Pending,
        Solved,
        Important,
        General
    }
    public class ForumTopicCreateVm
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ForumProgress? Progress { get; set; }
    }

    public class ReplyCreateVm
    {
        [Required]
        public int ForumTopicId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;
    }

}
