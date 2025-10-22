using Microsoft.AspNetCore.Mvc;
using CampusLearn.Models;

namespace CampusLearn.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            // var messages = MessageStore.GetMessagesForUser(User.Identity.Name);
            //return View(new ChatViewModel { Messages = messages });
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(string messageText)
        {
            //MessageStore.SaveMessage(User.Identity.Name, messageText);
            return RedirectToAction("Chat");
        }

    }
}
