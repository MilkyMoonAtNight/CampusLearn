using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusLearn.Controllers
{
    /// Simple Grades page for black-box testing (in-memory sample).
    public class GradesController : Controller
    {
        public IActionResult Index()
        {
            var vm = new GradesViewModel
            {
                Overall = 69,
                Modules = new List<ModuleGrades>
                {
                    new ModuleGrades("Mathematics", 71, new []{ ("Summative Test", 69), ("Assignment 1", 74), ("Quiz", 70) }),
                    new ModuleGrades("Statistics", 65, new []{ ("Assignment", 67), ("Test", 63) }),
                    new ModuleGrades("Programming", 72, new []{ ("Practical", 75), ("Project", 70) }),
                    new ModuleGrades("Database Development", 68, new []{ ("ERD", 70), ("SQL Test", 66) }),
                    new ModuleGrades("Networking", 64, new []{ ("Lab", 66), ("Test", 62) }),
                }
            };
            return View(vm);
        }

        // ----- View Models -----
        public class GradesViewModel
        {
            public int Overall { get; set; }
            public List<ModuleGrades> Modules { get; set; } = new();
        }

        public class ModuleGrades
        {
            public string Module { get; set; }
            public int Total { get; set; }
            public List<Entry> Breakdown { get; set; } = new();
            public ModuleGrades(string module, int total, IEnumerable<(string name, int mark)> parts)
            {
                Module = module; Total = total;
                Breakdown = parts.Select(p => new Entry { Name = p.name, Mark = p.mark }).ToList();
            }
            public class Entry { public string Name { get; set; } = ""; public int Mark { get; set; } }
        }
    }
}
