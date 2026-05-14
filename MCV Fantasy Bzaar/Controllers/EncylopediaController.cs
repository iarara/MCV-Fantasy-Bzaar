using MCV_Fantasy_Bzaar.Services;
using Microsoft.AspNetCore.Mvc;
using MCV_Fantasy_Bzaar.Models;
using System.Collections.Generic;
using System.Linq;

namespace MCV_Fantasy_Bzaar.Controllers
{
    public class EncyclopediaController : Controller
    {
        private readonly EncyclopediaService _encyclopedia;
        private readonly IConfiguration _configuration;

        public EncyclopediaController(EncyclopediaService encyclopedia, IConfiguration configuration)
        {
            _encyclopedia = encyclopedia;
            _configuration = configuration;
        }

        public ActionResult Index(bool isStaff = false)
        { 
            // Here, I'm keeping track of the staff status so the view knows whether to show Admin buttons or not
            ViewBag.IsStaff = isStaff;

            // Below I pass the Google Maps API key from the configuration to the view so it can be used in JavaScript for the map functionality.
            // This way, I only have to manage the API key in one place (appsettings.json) and it can be easily accessed in any view that needs it without hardcoding it in multiple places.
            ViewBag.ApiKey = _configuration["GoogleMaps:ApiKey"];
            
            var model = _encyclopedia.AllComics;
            return View(model ?? new List<BookDetails>());

        }

        [HttpPost]
        public ActionResult Search(string query, string author, string year, string genre, bool isStaff = false)
        {
            // Here I pass all search box inputs through to the filtering service
            ViewBag.IsStaff = isStaff;

            // And again, I pass the Google Maps API key to the view for use in JavaScript. This allows the search results page to
            // also have map functionality if needed, without having to duplicate the API key in multiple places.
            ViewBag.ApiKey = _configuration["GoogleMaps:ApiKey"];

            var results = _encyclopedia.SearchAndTrack(query, author, year, genre, null);
            return View("Index", results);
        }

        [HttpPost]
        public ActionResult Flag(string title)
        {
            // Here is the function so staff can flag records which saves them into a text file and marks them as flagged in the UI, even after a restart
            _encyclopedia.FlagRecord(title);
            TempData["Message"] = $"Success: '{title}' has been flagged.";
            return RedirectToAction("Index", new { isStaff = true });
        }

        public ActionResult Analytics()
        {
            // Here I take the search tracking dictionary and order it to show the top 10 most searched terms in the management analytics page
            var topSearches = _encyclopedia.SearchCounts
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToList();
            return View(topSearches);
        }

        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            // Here is a very basic login function that checks for credentials and redirects to the staff view if they match,
            // otherwise it shows an error message
            if (username == "admin" && password == "password123")
            {
                return RedirectToAction("Index", new { isStaff = true });
            }
            ViewBag.Error = "Invalid Login details.";
            return View();
        }

        public ActionResult Logout()
        {
            // Here it redirects back to the public view
            return RedirectToAction("Index", new { isStaff = false });
        }
    }
}