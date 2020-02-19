using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practical1.Controllers
{
    public class HomeController : Controller
    {
        Practical1Entities db;
        public HomeController(Practical1Entities db)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}