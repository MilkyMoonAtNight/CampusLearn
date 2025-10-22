using Microsoft.AspNetCore.Mvc;
using CampusLearn.Models;

namespace CampusLearn.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "LogIn");

            var messages = Message.GetMessagesForUser(username);
            var model = new MessageView { Messages = messages };
            return View(model);

        }

        [HttpPost]
        public IActionResult SendMessage(string messageText)
        {
            var username = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "LogIn");

            Message.SaveMessage(username, messageText, true);
            return RedirectToAction("Chat");

        }

    }
}
