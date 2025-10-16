namespace CampusLearn.Models
{
    public class Student
    {
        public long StudentID { get; set; }
        public string FirstName { get; set; } = "";
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = "";
        public string? PersonalEmail { get; set; }
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = "";
    }
}
