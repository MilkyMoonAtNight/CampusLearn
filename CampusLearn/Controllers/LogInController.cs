using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusLearn.Controllers
{
    public class LogInController : Controller
    {
        private readonly CampusLearnContext _context;

        public LogInController(CampusLearnContext context)
        {
            _context = context;
        }

        // Existing login logic...

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            student.PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(student.PasswordHash));
            student.StudentId = DateTime.Now.Ticks;

            _context.student.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
