using System;
using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class Admin
    {
        public int AdminID { get; set; }

        [Required, StringLength(255)]
        public string AdminName { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string AdminSurname { get; set; } = string.Empty;

        [Phone, StringLength(50)]
        public string? AdminPhone { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public string AdminEmail { get; set; } = string.Empty;

        [Required]
        public string AdminPasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsSuperAdmin { get; set; } = false;
    }
}

