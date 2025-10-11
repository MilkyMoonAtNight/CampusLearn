using System.ComponentModel.DataAnnotations;

namespace CampusLearn.Models
{
    public class Login
    {
        
        public string sEmail { get; set; } = "";
        public string sPassword { get; set; } = "";
        bool bRememberMe { get; set; }= false;

    }
}
