using Practical1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practical1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        Practical1Entities db;
        AssignEvent assignEvent;
        public AdminController(Practical1Entities db)
        {
            assignEvent = new AssignEvent();
            this.db = db;
        }
        public ActionResult Index()
        {
            ViewBag.Role = "Admin";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            var r = db.Users.Where(x => x.Email.Equals(user.Email) && x.Password.Equals(user.Password) && x.Role.Equals(user.Role)).SingleOrDefault();

                if (r == null)
                {
                    TempData["error"] = "Invalide Username Or Password";
                    return View();
                }
                Session["email"] = user.Email;
                Session["name"] = r.FirstName;
                return RedirectToAction("Home");
            
        }
        public ActionResult CreateEvent()
        {
            return View(db.Events.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(Event events)
        {
            try
            {
                db.Events.Add(events);
                db.SaveChanges();
                TempData["error"] = "Event Created Sucessfully";
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(db.Events.ToList());
        }
        public ActionResult EditEvent(int? id)
        {
            return View(db.Events.Find(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(Event events)
        {
            db.Entry(events).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CreateEvent");
        }
        public ActionResult AssignEvent()
        {
            assignEvent= Assign(assignEvent, db);
            return View(assignEvent);
        }
        [HttpPost]
        public ActionResult AssignEvent(Event_User event_User)
        {
            try
            {
                db.Event_User.Add(event_User);
                db.SaveChanges();
                TempData["error"] = "Assign Event Sucessfully";
                
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            assignEvent = Assign(assignEvent, db);
            return View(assignEvent);
        }
        public ActionResult AssignDelete(int id)
        {
            db.Event_User.Remove(db.Event_User.Find(id));
            db.SaveChanges();
            TempData["error"] = "Deleted Sucessfully";
            return RedirectToAction("AssignEvent");
        }
        public ActionResult DeleteEvent(int id)
        {
            db.Events.Remove(db.Events.Find(id));
            var e = db.Event_User.Where(x => x.Event_Id == id);
            foreach (var item in e)
            {
                db.Event_User.Remove(item);

            }
            db.SaveChanges();
            TempData["error"] = "Deleted Sucessfully";
            return RedirectToAction("CreateEvent");
        }
        public ActionResult UserList()
        {

            return View(db.Users.ToList());
        }
        public ActionResult EditUser(int ?id)
        {
            return View(db.Users.Find(id));
        }
        [HttpPost]
        public ActionResult EditUser(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserList");
        }
        public ActionResult DeleteUser(int id)
        {
            db.Users.Remove(db.Users.Find(id));
            db.Event_User.RemoveRange(db.Event_User.Where(x => x.U_id == id).ToList());
            db.SaveChanges();
            return RedirectToAction("UserList");
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("email");
            Session.Remove("name");
            return RedirectToAction("Index","Home");
        }
        public static AssignEvent Assign(AssignEvent assignEvent,Practical1Entities db)
        {
            assignEvent.users = db.Users.Where(x => x.Role.Equals("User")).ToList();
            assignEvent.events = db.Events.ToList();
            assignEvent.event_user = db.Event_User.ToList(); ;
            return assignEvent;

        }
    }
}