using System.Linq;
using System.Threading.Tasks;
using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    public class LogInController : Controller
    {
        private readonly CampusLearnContext _context;

        public LogInController(CampusLearnContext context)
        {
            _context = context;
        }

        // GET: /LogIn/Index
        [HttpGet]
        public IActionResult Index()
        {
            // Render the login view (removed temporary Content response)
            return View();
        }

        // POST: /LogIn/Index — use UserStore (in-memory) for authentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter username and password.";
                return View();
            }

            // Allow login by username or email
            var user = UserStore.Users
                .FirstOrDefault(u => string.Equals(u.Username, username, System.StringComparison.OrdinalIgnoreCase)
                                  || string.Equals(u.Email, username, System.StringComparison.OrdinalIgnoreCase));

            if (user is null || user.Password != password)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // Store minimal session info
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Email", user.Email ?? string.Empty);

            return RedirectToAction("Index", "LogIn");
        }

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
            student.StudentID = DateTime.Now.Ticks;

            student.PasswordHash = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(student.PasswordHash));
            student.StudentID = System.DateTime.Now.Ticks;


            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
