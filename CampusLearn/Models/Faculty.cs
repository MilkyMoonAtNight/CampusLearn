using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CampusLearn.Models
{
    public class Faculty
    {
        [Key]
        public int FacultyID { get; set; }

        [Required, MaxLength(120)]
        public string FacultyName { get; set; }

        public ICollection<Degree> Degrees { get; set; }
    }
}
