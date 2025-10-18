using Microsoft.AspNetCore.Mvc;

namespace CampusLearn.Controllers
{
    public class LogIn : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {

        }
    }
}
