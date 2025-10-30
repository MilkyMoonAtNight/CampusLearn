using CampusLearn.Data;
using CampusLearn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace CampusLearn.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly CampusLearnContext _db;
        public AnnouncementsController(CampusLearnContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var announcements = await _db.Announcements
                .Include(a => a.Admin)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(announcements);
        }

        public async Task<IActionResult> Details(int id)
        {
            var announcement = await _db.Announcements
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(a => a.AnnouncementID == id);

            if (announcement == null)
                return NotFound();

            var replies = await _db.ReplyAnnouncements
                .Where(r => r.AnnouncementID == id)
                .OrderBy(r => r.PostedAt)
                .ToListAsync();

            ViewBag.Replies = replies;
            return View(announcement);
        }
    }
}
