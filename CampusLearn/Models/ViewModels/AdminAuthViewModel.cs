using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models.ViewModels
{
    public class AdminLoginVm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class AdminCreateVm
    {
        [Required, StringLength(255)]
        public string AdminName { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string AdminSurname { get; set; } = string.Empty;

        [Phone, StringLength(50)]
        public string? AdminPhone { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public string AdminEmail { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public bool IsSuperAdmin { get; set; }
    }

    public class AdminEditVm
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

        public bool IsSuperAdmin { get; set; }

        [DataType(DataType.Password), MinLength(8)]
        public string? NewPassword { get; set; } // optional change
    }
}
