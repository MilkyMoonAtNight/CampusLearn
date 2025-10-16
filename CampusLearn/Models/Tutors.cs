namespace CampusLearn.Models
{
    public class Tutors
    {
        public long TutorID { get; set; }
        public string TutorName { get; set; } = "";
        public string? TutorMiddleInit { get; set; }
        public string TutorSurname { get; set; } = "";
        public int? SpecialityID { get; set; }
    }
}
