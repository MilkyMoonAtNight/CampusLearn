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

        // POST: /LogIn/Index 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))//checking if fields are empty
            {
                ViewBag.Error = "Please enter username and password.";
                return View();
            }
            //Encode Password to match Reg
            var encodedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
            encodedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(encodedPassword));
            //changed from UserStore 
            var user = await _context.Students
                .FirstOrDefaultAsync(u =>  u.PersonalEmail == email);

            if (user == null || user.PasswordHash != encodedPassword)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // ✅ Set session key for messaging
            HttpContext.Session.SetString("LoggedInUser", user.PersonalEmail);

            // Optional: also store other info
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("Email", user.PersonalEmail ?? string.Empty);

            // ✅ Redirect to messaging or dashboard
            return RedirectToAction("Index", "Dashboard"); // or "Dashboard"
        }


        //get: /login/register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //post: /login/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            //checking if username already exists
            if(await _context.Students.AnyAsync(u =>  u.PersonalEmail == student.PersonalEmail))
            {
                ModelState.AddModelError("", "Username or email already exists.");
                return View(student);
            }

            student.PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(student.PasswordHash));
            student.PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(student.PasswordHash));
            // Let the database generate StudentID (assuming it's an auto-incrementing primary key)
            // Remove: student.StudentID = DateTime.Now.Ticks;

            //save to database
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            //redirect to login page after successful registration
            return RedirectToAction("Index");
        }
    }
}
