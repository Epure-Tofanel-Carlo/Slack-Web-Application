using Microsoft.AspNetCore.Mvc;
using SlackDAW1.Models;
using SlackDAW1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

//chiar nu inteleg de ce nu accepta messages
//nvm acum merge

namespace SlackDAW1.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MessagesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var messages = db.Messages.OrderByDescending(m => m.Timestamp).Include("Channel");
            return View(messages);
        }

       
        public IActionResult New()
        {
            Message message = new Message();

            ViewBag.Channels = ChannelsController.GetAllChannelsToDisplayForForm(db);

            return View(message);
        }

      
        [HttpPost]
        public IActionResult New(Message message)
        {

            message.Timestamp = DateTime.Now;
            message.SenderID = _userManager.GetUserId(User);

			if (ModelState.IsValid)
            {   
                db.Messages.Add(message);
                db.SaveChanges();
                TempData["message"] = "Message was added";
                return RedirectToRoute(new { controller = "Channels", action = "Show", id = message.ChannelID });
            } else
            {
				ViewBag.Channels = ChannelsController.GetAllChannelsToDisplayForForm(db);
                return View(message);
			}
        }

    
        public IActionResult Edit(int id)
        {
            var message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }


        [HttpPost]
        public IActionResult Edit(int id, Message requestMessage)
        {
            var message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                message.Body = requestMessage.Body;
                message.Timestamp = DateTime.Now;
               
                db.SaveChanges();
                TempData["message"] = "Message was edited";
                return RedirectToRoute(new { controller = "Channels", action = "Show", id = message.ChannelID });
            }
            return View(message);
        }

       
       [HttpPost]
        public IActionResult Delete(int MessageID)
        {
            var message = db.Messages.Find(MessageID);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            db.SaveChanges();
            TempData["message"] = "Message was deleted";

            return RedirectToRoute(new { controller = "Channels", action = "Show", id = message.ChannelID });
        }

       
        public IActionResult Show(int id)
        {
            var message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }
    }
}