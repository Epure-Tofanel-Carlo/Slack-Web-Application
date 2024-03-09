using SlackDAW1.Models;
using SlackDAW1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace SlackDAW1.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext db;

        public SearchController(ApplicationDbContext context)
        {
            db = context;
        }

        // index method in which we will search
        // usign the search string from the query string
        // we searching for channels that contain the search string in their name
        public IActionResult Index() {
            var searchString = Request.Query["search"].ToString();
            searchString = searchString.Trim();
            ViewBag.SearchString = searchString;

            var channels = db.Channels.Include("Category").Where(c => c.ChannelName.Contains(searchString)).ToList();
            ViewBag.Channels = channels;

            return View();
        }
    }
}