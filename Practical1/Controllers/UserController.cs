using Practical1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practical1.Controllers
{
    public class UserController : Controller
    {
        Practical1Entities db;
        DbServices dbService;
        public UserController(Practical1Entities db)
        {
           dbService= new DbServices(db);
            this.db = db;
        }
        // GET: User
        public ActionResult Index()
        {
            ViewBag.Role = "User";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            var r = dbService.UserAuthenticate(user);
            if(r==null)
            {
                TempData["error"] = "Invalide Username Or Password";
                return View();
            }
            Session["email"] = user.Email;
            Session["id"] = r.U_id;
            Session["name"] = r.FirstName;
            return RedirectToAction("Home");
        }
        public ActionResult SignUp()
        {
            ViewBag.Role = "User";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(db.Users.Where(x=>x.Email.Equals(user.Email)).FirstOrDefault()!=null)
                    {
                        TempData["error"] = "Already registered Please Login..";
                        
                    }
                    else if(dbService.SaveUser(user))
                    {
                        TempData["error"] = "Sucessfully registered";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["error"] = "Something Went Wrong";
                    return View();
                }
                return RedirectToAction("Index");

            }
            else
            {
                return View();
            }
        }
        public ActionResult AssignEvent()
        {
            try
            {
                int uid = Convert.ToInt32(Session["id"].ToString());
                var r = db.Event_User.Where(x => x.U_id == uid).ToList();
                return View(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Index");
            }
        }

        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("email");
            Session.Remove("name");
            return RedirectToAction("Index", "Home");
        }
    }
}