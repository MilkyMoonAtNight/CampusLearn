using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static CampusLearn.Controllers.LogInController;

namespace CampusLearn.Models
{
    public static class UserStore
    {
        public static List<User> Users = new List<User>
        {
        new User
        {
            Username = "admin",
            Password = "admin123",
            FirstName = "John",
            MiddleInitial = "J.",
            LastName = "Doe",
            Email = "admin@student.belgiumcampus.ac.za",
            Phone = "012-3456789",
            Groups = new List<string> { "SEN-381", "PRG-281" },
            LastAccess = new DateTime(2025, 10, 7)
        },
        new User
        {
            Username = "user1",
            Password = "password1",
            FirstName = "Alice",
            MiddleInitial = "M.",
            LastName = "Smith",
            Email = "alice@student.belgiumcampus.ac.za",
            Phone = "011-2233445",
            Groups = new List<string> { "PRG-281" },
            LastAccess = new DateTime(2025, 10, 6)
        },
        new User
        {
            Username = "user2",
            Password = "password2",
            FirstName = "Bob",
            MiddleInitial = "K.",
            LastName = "Ngwenya",
            Email = "bob@student.belgiumcampus.ac.za",
            Phone = "010-9988776",
            Groups = new List<string> { "SEN-381" },
            LastAccess = new DateTime(2025, 10, 5)
        }

        };
    }

    public class User
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle initial is required.")]
        [StringLength(1, ErrorMessage = "Middle initial must be a single character.")]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        public List<string> Groups { get; set; } = new();

        [DataType(DataType.DateTime)]
        public DateTime LastAccess { get; set; } = DateTime.Now;

        public string FullName => $"{FirstName} {MiddleInitial} {LastName}";

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

}
