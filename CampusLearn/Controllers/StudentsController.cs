using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly CampusLearnContext _context;

        public StudentsController(CampusLearnContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return await _context.student.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            var student = await _context.student.FindAsync(id);
            if (student == null)
                return NotFound();
            return student;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Student>> RegisterStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Simple password hashing (replace with secure hashing in production)
            student.PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(student.PasswordHash));
            student.StudentId = DateTime.Now.Ticks;

            _context.student.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }
        [HttpGet("/Students/Register")]
        public IActionResult Register()
        {
            return View(); // This will look for Views/Students/Register.cshtml
        }

        [HttpPost("/Students/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            student.PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(student.PasswordHash));
            student.StudentId = DateTime.Now.Ticks;

            _context.student.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "LogIn"); // Redirect to login after success
        }

    }
}
