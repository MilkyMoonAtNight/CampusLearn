using Microsoft.AspNetCore.Mvc;
using CampusLearn.Models;

namespace CampusLearn.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index(string recipient = null)
        {
            var currentUser = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(currentUser))
                return RedirectToAction("Index", "LogIn");

            var allUsers = UserStore.Users.Where(u => u.Username != currentUser).ToList();

            var selectedRecipient = recipient ?? allUsers.FirstOrDefault()?.Username;
            var messages = Message.GetConversation(currentUser, selectedRecipient);

            var model = new MessageView
            {
                CurrentUser = currentUser,
                SelectedRecipient = selectedRecipient,
                AllUsers = allUsers,
                Messages = messages
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SendMessage(string messageText, string recipient)
        {
            var sender = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(sender))
                return RedirectToAction("Index", "LogIn");

            Message.SaveMessage(sender, recipient, messageText, true);
            return RedirectToAction("Index", new { recipient });
        }
    }
}
