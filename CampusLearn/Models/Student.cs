namespace CampusLearn.Models
{
    public class Student
    {
        public long studentid { get; set; }
        public string firstname { get; set; } = "";
        public string? middlename { get; set; }
        public string lastname { get; set; } = "";
        public string? personalemail { get; set; }
        public string? phone { get; set; }
        public string passwordhash { get; set; } = "";
    }
}
