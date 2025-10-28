using Microsoft.AspNetCore.Mvc;
using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CampusLearnContext _context;

        public ProfileController(CampusLearnContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("LoggedInUserID");

            if (!userId.HasValue)
                return RedirectToAction("Index", "LogIn");

            // If you have both students and tutors, you can check both
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentID == userId.Value);

            if (student != null)
                return View("Index", student);

            var tutor = await _context.Tutors
                .FirstOrDefaultAsync(t => t.TutorID == userId.Value);

            if (tutor != null)
                return View("TutorProfile", tutor);

            return RedirectToAction("Index", "LogIn");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Index", "LogIn"); 
        }
    }
}
