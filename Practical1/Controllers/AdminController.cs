using Practical1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Practical1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        DbServices dbService;
        public AdminController(DbServices dbService)
        {
            this.dbService = dbService;
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
               var r = dbService.UserAuthenticate(user);
               if (r==null)
               {
               TempData["error"] = "Invalide Username Or Password";
               return View();
               }
               else
               {
               Session["email"] = user.Email;
               Session["name"] = r.FirstName;
               return RedirectToAction("Home");
               }
        }
        public ActionResult CreateEvent(int ?page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(dbService.GetAllEvents().ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(Event events,int ?page)
        {
            if(dbService.SaveEvent(events))
            {
                TempData["error"] = "Event Created Sucessfully";
            }
            else
            {
                TempData["error"] = "Something Wrong";
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(dbService.GetAllEvents().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult EditEvent(int? id)
        {
            var r = dbService.GetEventbyId(id);
            return View(r);
        }
        [HttpPost]
        public ActionResult EditEvent(Event events)
        {
          
            if(!dbService.EditEvent(events))
            {
                TempData["error"] = "Something Wrong";

            }
            return RedirectToAction("CreateEvent");
        }
        public ActionResult AssignEvent()
        {
            AssignEvent assignEvent = dbService.AssignEvent();
            List<SelectListItem> ls = new List<SelectListItem>();
            foreach (var item in assignEvent.users)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = item.FirstName,
                    Value = item.U_id.ToString()
                };
                ls.Add(selectListItem);
            }
            ViewBag.ls = ls;
            return View(assignEvent);
        }
        [HttpPost]
        public ActionResult AssignEvent(IEnumerable<String> U_id,int ?Event_Id, int ?page)
        {   
            if (Event_Id == null)
            {
                TempData["error"] = "No Event Available";
            }
            else if (U_id == null)
            {
                TempData["error"] = "No User is Selected";
            }
            else
            {
                try
                {
                    var a = new List<Event_User>();
                    foreach (var item in U_id)
                    {
                        var r = new Event_User();
                        r.Event_Id = Convert.ToInt32(Event_Id);
                        r.U_id = Convert.ToInt32(item);
                        a.Add(r);

                    }
                    if (dbService.AddEvent_User(a))
                    {
                        TempData["error"] = "Assign Event Sucessfully";
                    }
                    else
                    {
                        TempData["error"] = "Something Wrong";
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
            }
            //assignEvent = Assign(assignEvent, db);
            return RedirectToAction("AssignEvent");
        }
        public ActionResult AssignDelete(int id)
        {
            if (!dbService.DeleteEvent_UserById(id))
            {
                TempData["error"] = "Something Wrong";
            }
            else
            {
                TempData["error"] = "Deleted Sucessfully";
            }
            return RedirectToAction("AssignEvent");
        }
        public ActionResult DeleteEvent(int id,int ?P)
        {
            if (!dbService.RemoveEventById(id))
            {
                TempData["error"] = "Something Wrong";
            }
            else
            {
                TempData["error"] = "Deleted Sucessfully";
            }
            return RedirectToAction("CreateEvent",new {Page=P });
        }
        public ActionResult UserList()
        {

            return View(dbService.GetAllUsers());
        }
        public ActionResult EditUser(int ?id)
        {
            return View(dbService.GetUserbyId(id));
        }
        [HttpPost]
        public ActionResult EditUser(User user)
        {   if(!dbService.EditUser(user))
            {
                TempData["error"] = "Something Wrong";
            }
            return RedirectToAction("UserList");
        }
        public ActionResult DeleteUser(int id)
        {
            if(!dbService.RemoveUserById(id))
            {
                TempData["error"] = "Something Wrong";
            }
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
        
    }
}