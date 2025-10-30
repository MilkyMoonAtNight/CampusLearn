using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("messageuser")]
    public class MessageUser
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("userid")]
        public long? UserId { get; set; }

        [Column("firstname")]
        [StringLength(255)]
        public string FirstName { get; set; } = string.Empty;

        [Column("lastname")]
        [StringLength(255)]
        public string LastName { get; set; } = string.Empty;

        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Column("phonenumber")]
        [StringLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        [NotMapped]
        public List<string> Groups { get; set; } = new List<string>();

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}