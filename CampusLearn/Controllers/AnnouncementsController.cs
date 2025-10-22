using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CampusLearn.Services;

namespace CampusLearn.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsStore _store;
        public AnnouncementsController(IAnnouncementsStore store) => _store = store;

        public IActionResult Index(string sort = "newest")
        {
            var items = _store.GetAll();
            if (sort == "oldest") items = items.OrderBy(a => a.CreatedAt).ToList();
            ViewBag.Sort = sort;
            return View(items.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string moduleTag, string body)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                TempData["Error"] = "Title and Body are required.";
                return RedirectToAction("Index");
            }
            _store.Add(title, moduleTag, body);
            TempData["Message"] = "Announcement posted.";
            return RedirectToAction("Index");
        }
    }
}
