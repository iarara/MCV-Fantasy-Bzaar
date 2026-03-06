using MCV_Fantasy_Bzaar.Services;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Fantasy_Bzaar.Controllers
{
    public class EncyclopediaController : Controller
    {
        private readonly EncyclopediaService _encyclopedia;

        public EncyclopediaController(EncyclopediaService encyclopedia)
        {
            _encyclopedia = encyclopedia;
        }

        public ActionResult Index(bool isStaff = false)
        {
            ViewBag.IsStaff = isStaff;
            return View(_encyclopedia.AllComics);
        }

        [HttpPost]
        public ActionResult Search(string query, string author, string year, string genre, string lang, bool isStaff = false)
        {
            ViewBag.IsStaff = isStaff;
            var results = _encyclopedia.SearchAndTrack(query, author, year, genre, lang);
            return View("Index", results);
        }

        [HttpPost]
        public ActionResult Flag(string title)
        {
            _encyclopedia.FlagRecord(title);
            return RedirectToAction("Index", new { isStaff = true });
        }

        public ActionResult Analytics()
        {
            ViewBag.TopQueries = _encyclopedia.SearchQueriesCounter.OrderByDescending(x => x.Value).Take(10);
            ViewBag.TopResults = _encyclopedia.ComicAppearanceCounter.OrderByDescending(x => x.Value).Take(10);
            ViewBag.PopularComics = _encyclopedia.ComicAppearanceCounter.Where(x => x.Value > 100);

            return View();
        }
    }
}

