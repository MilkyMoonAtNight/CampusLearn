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
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<string> Groups { get; set; }
        public DateTime LastAccess { get; set; }
        public string FullName => $"{FirstName} {MiddleInitial} {LastName}";
        public string Password { get; set; }
    }

}
