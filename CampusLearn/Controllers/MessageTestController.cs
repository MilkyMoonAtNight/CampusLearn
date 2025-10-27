using Microsoft.AspNetCore.Mvc;

namespace CampusLearn.Controllers
{
    public class MessageTestController : Controller
    {
        public IActionResult Index()
        {
            return View("Test");
        }
    }
}
