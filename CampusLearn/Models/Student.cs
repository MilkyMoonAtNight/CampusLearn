using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class Student
    {
        public long StudentId { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string? PersonalEmail { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; } = "";
    }
}
