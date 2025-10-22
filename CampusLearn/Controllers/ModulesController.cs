using Microsoft.AspNetCore.Mvc;
using CampusLearn.Data;
using CampusLearn.Models;
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
            var modules = await _context.Modules
                .Include(m => m.ModuleCluster)
                .Include(m => m.ModuleHead)
                .ToListAsync();

            return View(modules);
        }

        // GET: /Modules/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var module = await _context.Modules
                .Include(m => m.ModuleCluster)
                .Include(m => m.ModuleHead)
                .Include(m => m.ModuleResources)
                .FirstOrDefaultAsync(m => m.ModuleID == id);

            if (module == null)
                return NotFound();

            return View(module);
        }
    }
}
