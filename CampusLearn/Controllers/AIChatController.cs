using Microsoft.AspNetCore.Mvc;

namespace CampusLearn.Controllers
{
    public class AiChatController : Controller
    {
        [HttpGet]
        public IActionResult Chat()
        {
            return View();
        }
    }
}
