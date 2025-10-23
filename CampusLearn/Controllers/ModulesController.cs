using Microsoft.AspNetCore.Mvc;
using CampusLearn.Models; // Adjust namespace as needed

public class ModulesController : Controller
{
    // Simulated in-memory module list
    private static readonly List<TopicModule> _modules = new()
    {
        new TopicModule
        {
            ModuleID = 101,
            ModuleName = "English for Academic Purposes",
            ClusterID = 1,
            ModuleCluster = new ModuleCluster { ClusterID = 1, ClusterName = "English" },
            ModuleHeadID = 1001,
            ModuleHead = new Tutors { TutorID = 1001, TutorName = "Dr. Sarah Mokoena" }
        },
        new TopicModule
        {
            ModuleID = 102,
            ModuleName = "Software Engineering Fundamentals",
            ClusterID = 2,
            ModuleCluster = new ModuleCluster { ClusterID = 2, ClusterName = "Technology" },
            ModuleHeadID = 1002,
            ModuleHead = new Tutors { TutorID = 1002, TutorName = "Prof. Johan van der Merwe" }
        },
        new TopicModule
        {
            ModuleID = 103,
            ModuleName = "Entrepreneurship 181",
            ClusterID = 3,
            ModuleCluster = new ModuleCluster { ClusterID = 3, ClusterName = "Business" },
            ModuleHeadID = 1003,
            ModuleHead = new Tutors { TutorID = 1003, TutorName = "Ms. Lerato Dlamini" }
        }
    };

    // GET: /Modules
    public IActionResult Index()
    {
        return View(_modules);
    }

    // GET: /Modules/Details/{id}
    public IActionResult Details(int id)
    {
        var module = _modules.FirstOrDefault(m => m.ModuleID == id);
        if (module == null)
            return NotFound();

        return View(module);
    }
}