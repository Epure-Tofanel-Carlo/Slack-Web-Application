using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlackDAW1.Data;
using SlackDAW1.Models;

namespace SlackDAW1.Controllers
{
	[Authorize(Roles = "Admin, Moderator, User")]
	public class UsersController : Controller
	{

		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UsersController(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager
		)
		{
			db = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public IActionResult Invite(int? id)
		{
			var allUsers = db.Users.ToList();
			var usersInChannel = db.UserChannels.Where(uc => uc.ChannelID == id).Select(uc => uc.User).ToList();

			var usersNotInChannel = allUsers.Except(usersInChannel).ToList();

			ViewBag.UsersNotInChannel = usersNotInChannel;
			ViewBag.ChannelID = id;

			return View();
		}

		[HttpPost]
		public IActionResult Invite(int? id, string userId)
		{
			var userChannel = new UserChannel
			{
				UserID = userId,
				ChannelID = (int)id,
				IsModerator = false
			};

			db.UserChannels.Add(userChannel);
			db.SaveChanges();

			var allUsers = db.Users.ToList();
			var usersInChannel = db.UserChannels.Where(uc => uc.ChannelID == id).Select(uc => uc.User).ToList();

			var usersNotInChannel = allUsers.Except(usersInChannel).ToList();

			ViewBag.UsersNotInChannel = usersNotInChannel;
			ViewBag.ChannelID = id;

			return View();
		}

		[Authorize(Roles = "Admin, Moderator, User")]
		[HttpPost]
		public IActionResult RemoveUserFromChannel(string userId, int channelId)
		{
			var me = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

			if (me == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var myRoleInChannel = db.UserChannels
				.FirstOrDefault(uc => uc.UserID == me.Id && uc.ChannelID == channelId && uc.IsModerator);

			if (myRoleInChannel == null)
			{
				return NotFound();
			}

			var userChannel = db.UserChannels
				.SingleOrDefault(uc => uc.UserID == userId && uc.ChannelID == channelId);

			if (userChannel != null)
			{
				db.UserChannels.Remove(userChannel);
				db.SaveChanges();
			}
			else
			{
				return NotFound();
			}

			return RedirectToAction("Show", "Channels", new { id = channelId });
		}

		[Authorize(Roles = "Admin, Moderator, User")]
		[HttpPost]
		public IActionResult PromoteUserOnChannel(string userId, int channelId)
		{
			var me = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

			if (me == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var myRoleInChannel = db.UserChannels
				.FirstOrDefault(uc => uc.UserID == me.Id && uc.ChannelID == channelId && uc.IsModerator);

			if (myRoleInChannel == null)
			{
				return NotFound();
			}

			var userChannel = db.UserChannels
				.SingleOrDefault(uc => uc.UserID == userId && uc.ChannelID == channelId);

			if (userChannel != null)
			{
				userChannel.IsModerator = true;
				db.SaveChanges();
			}
			else
			{
				return NotFound();
			}

			return RedirectToAction("Show", "Channels", new { id = channelId });
		}

		[Authorize(Roles = "Admin, Moderator, User")]
		[HttpPost]
		public IActionResult DemoteUserOnChannel(string userId, int channelId)
		{
			var me = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

			if (me == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var myRoleInChannel = db.UserChannels
				.FirstOrDefault(uc => uc.UserID == me.Id && uc.ChannelID == channelId && uc.IsModerator);

			if (myRoleInChannel == null)
			{
				return NotFound();	
			}

			var userChannel = db.UserChannels
				.SingleOrDefault(uc => uc.UserID == userId && uc.ChannelID == channelId);

			if (userChannel != null)
			{
				userChannel.IsModerator = false;
				db.SaveChanges();
			}
			else
			{
				return NotFound();
			}

			return RedirectToAction("Show", "Channels", new { id = channelId });
		}

		[Authorize(Roles = "Admin, Moderator, User")]
		[HttpPost]
		public IActionResult LeaveChannel(int channelId)
		{
			var me = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

			if (me == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var userChannel = db.UserChannels
				.SingleOrDefault(uc => uc.UserID == me.Id && uc.ChannelID == channelId);

			if (userChannel != null)
			{
				db.UserChannels.Remove(userChannel);
				db.SaveChanges();
			}
			else
			{
				return NotFound();
			}

			return RedirectToAction("Index", "Channels");
		}

		[Authorize(Roles = "Admin, Moderator, User")]
		[HttpPost]
		public IActionResult AddUserToChannel(int id)
		{

			var currentUserId = _userManager.GetUserId(User);

			var isAlreadyInChannel = db.UserChannels.Any(uc => uc.UserID == currentUserId && uc.ChannelID == id);
			if (isAlreadyInChannel)
			{
				return RedirectToAction("Show", "Channels", new { id = id });
			}

						var userChannel = new UserChannel
			{
				UserID = currentUserId,
				ChannelID = id,
				IsModerator = false
			};

			db.UserChannels.Add(userChannel);
			db.SaveChanges();

			return RedirectToAction("Show", "Channels", new { id = id });
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
