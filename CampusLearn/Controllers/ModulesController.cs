using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    public class ModulesController : Controller
    {
        private readonly CampusLearnContext _context;

        public ModulesController(CampusLearnContext context)
        {
            _context = context;
        }

        // GET: /Modules
        public async Task<IActionResult> Index()
        {
            // Include cluster + tutor so they load with module
            var modules = await _context.Modules
                .Include(m => m.ModuleCluster)
                .Include(m => m.ModuleHead)
                .Include(m => m.ModuleResources)
                .AsNoTracking()
                .ToListAsync();

            return View(modules);
        }

        // GET: /Modules/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var module = await _context.Modules
                .Include(m => m.ModuleCluster)
                .Include(m => m.ModuleHead)
                .Include(m => m.ModuleResources)
                .Include(m => m.ModulePlan)
                .Include(m => m.ModuleAssignments)
                .Include(m => m.ModuleProjects)
                .Include(m => m.ModuleTests)
                .Include(m => m.ModuleWeeks)
                .ThenInclude(w => w.Contents)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ModuleID == id);

            if (module == null)
                return NotFound();

            return View(module);
        }
    }
}