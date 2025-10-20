using CampusLearn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static CampusLearn.Controllers.LogInController;

namespace CampusLearn.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("LoggedInUser");

            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "LogIn");

            var user = UserStore.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return RedirectToAction("Index", "LogIn");

            return View(user); // Pass user to the view
        }

    }
}
