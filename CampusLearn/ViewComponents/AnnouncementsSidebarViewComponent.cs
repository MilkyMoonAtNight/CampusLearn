using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CampusLearn.Services;

namespace CampusLearn.ViewComponents
{
    public class AnnouncementsSidebarViewComponent : ViewComponent
    {
        private readonly IAnnouncementsStore _store;
        public AnnouncementsSidebarViewComponent(IAnnouncementsStore store) => _store = store;

        // call with e.g. count: 5
        public IViewComponentResult Invoke(int count = 5)
        {
            var items = _store.GetAll().Take(count).ToList();
            return View(items);
        }
    }
}
