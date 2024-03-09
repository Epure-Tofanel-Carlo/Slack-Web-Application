using SlackDAW1.Data;
using SlackDAW1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SlackDAW1.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoriesController : Controller
	{

		// PASUL 10 - useri si roluri

		private readonly ApplicationDbContext db;

		public CategoriesController(ApplicationDbContext context)
		{
			db = context;
		}
		public ActionResult Index()
		{
			var categories = db.Categories.OrderBy(c => c.CategoryName).ToList();
			var channels = db.Channels.ToList();

			var categoryDeletable = new Dictionary<int, bool>();
			var categoryChannels = new Dictionary<int, List<Channel>>();
			foreach (var category in categories)
			{
				categoryDeletable[category.CategoryID] = !channels.Any(c => c.CategoryID == category.CategoryID);
				categoryChannels[category.CategoryID] = channels.Where(c => c.CategoryID == category.CategoryID).ToList();
			}

			ViewBag.Categories = categories;
			ViewBag.CategoryDeletable = categoryDeletable;
			ViewBag.CategoryChannels = categoryChannels;

			return View();
		}


		public ActionResult Show(int id)
		{
			Category category = db.Categories.Find(id);

			var channels = from channel in db.Channels
						   orderby channel.ChannelName
						   select channel;
			ViewBag.Channels = channels;
			return View(category);
		}

		public ActionResult New()
		{
			return View();
		}

		[HttpPost]
		public ActionResult New(Category cat)
		{
			if (ModelState.IsValid)
			{
				db.Categories.Add(cat);
				db.SaveChanges();
				TempData["message"] = "Categoria a fost adaugata";
				return RedirectToAction("Index");
			}
			else
			{
				return View(cat);
			}
		}

		public ActionResult Edit(int id)
		{
			Category category = db.Categories.Find(id);
			return View(category);
		}

		[HttpPost]
		public ActionResult Edit(int id, Category requestCategory)
		{
			Category category = db.Categories.Find(id);

			if (ModelState.IsValid)
			{

				category.CategoryName = requestCategory.CategoryName;
				db.SaveChanges();
				TempData["message"] = "Categoria a fost modificata!";
				return RedirectToAction("Index");
			}
			else
			{
				return View(requestCategory);
			}
		}

		public ActionResult Delete(int id)
		{
			var category = db.Categories.Find(id);
			if (category != null)
			{
				if (!db.Channels.Any(channel => channel.CategoryID == id))
				{
					db.Categories.Remove(category);
					db.SaveChanges();
					TempData["message"] = "Category deleted successfully.";
				}
				else
				{
					TempData["message"] = "Category cannot be deleted because it has associated channels.";
				}
			}
			else
			{
				TempData["message"] = "Category not found.";
			}

			return RedirectToAction("Index");
		}

		[NonAction]
		public static IEnumerable<SelectListItem> GetAllCategoriesToDisplayForForm(ApplicationDbContext db)
		{
			var selectList = new List<SelectListItem>();

			var categories = from cat in db.Categories
							 orderby cat.CategoryName
							 select cat;

			foreach (var category in categories)
			{
				selectList.Add(new SelectListItem
				{
					Value = category.CategoryID.ToString(),
					Text = category.CategoryName.ToString()
				});
			}
			return selectList;
		}
	}
}