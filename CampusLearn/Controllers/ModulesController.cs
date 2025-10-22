using Microsoft.AspNetCore.Mvc;
using CampusLearn.Models;

namespace CampusLearn.Controllers
{
    public class ModulesController : Controller
    {
        private static List<Module> _modules = new()
        {
            new Module
            {
                Id = 1,
                Title = "Mathematics Cluster",
                Instructor = "Thabo Mbeki",
                WeeksRemaining = 7,
                Completion = 25,
                ImagePath = "/images/images.jpg",
                WeeklyContent = new List<WeekContent>
                {
                    new WeekContent { WeekNumber = 1, Topic = "Algebra Basics", Description = "Introduction to variables and expressions." },
                    new WeekContent { WeekNumber = 2, Topic = "Linear Equations", Description = "Solving equations and graphing lines." }
                },
                Announcements = new List<Announcement>
                {
                    new Announcement { Title = "Welcome!", Message = "Class starts Monday at 9am." }
                },
                Assignments = new List<Assignment>
                {
                    new Assignment { Title = "Algebra Worksheet", Instructions = "Complete all exercises on page 12.", DueDate = DateTime.Today.AddDays(5) }
                }
            },
            new Module
            {
                Id = 2,
                Title = "English Cluster",
                Instructor = "Nelson Mandela",
                WeeksRemaining = 6,
                Completion = 10,
                ImagePath = "/images/7952.jpg", 
                WeeklyContent = new List<WeekContent>
                {
                    new WeekContent { WeekNumber = 1, Topic = "Introduction to Literature", Description = "Exploring genres and literary devices." },
                    new WeekContent { WeekNumber = 2, Topic = "Poetry Analysis", Description = "Understanding rhythm, tone, and metaphor." }
                },
                Announcements = new List<Announcement>
                {
                    new Announcement { Title = "Reading List Posted", Message = "Check the resources tab for your first reading assignment." },
                    new Announcement { Title = "Group Work Reminder", Message = "Form your literature discussion groups by Friday." }
                },
                Assignments = new List<Assignment>
                {
                    new Assignment { Title = "Poetry Reflection", Instructions = "Write a 300-word reflection on your favorite poem.", DueDate = DateTime.Today.AddDays(7) },
                    new Assignment { Title = "Literary Essay", Instructions = "Compare two genres using examples from the reading list.", DueDate = DateTime.Today.AddDays(14) }
                }
            }

        };
        public IActionResult Index()
        {

            return View(_modules);
        }
        public IActionResult Details(int id)
        {
            var module = _modules.FirstOrDefault(m => m.Id == id);
            if (module == null) return NotFound();
            return View(module);
        }


    }
}
