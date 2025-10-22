using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class Speciality
    {
        [Key]
        public int SpecialityID { get; set; }

        [Required, MaxLength(100)]
        public string SpecialityName { get; set; }

        public ICollection<Tutors> Tutors { get; set; }
    }
}
