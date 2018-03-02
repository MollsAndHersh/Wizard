using netWizardDemo.Models;
using netWizardDemo.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Forms;

namespace netWizardDemo.Controllers
{
    public class HomeController : Controller
    {
        private testDbContext db = new testDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult AddNewUser(VMNewUser user)
        {
            MUser u = new MUser()
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EMail = user.EMail
            };
            foreach (MAddress a in user.Addresses)
            {
                db.Address.Add(a);
                u.Addresses.Add(a);
            }
            foreach(VMApplicationWizard app in user.Applications)
            {
                var id = app.ApplicationId;
                var foundModel =  db.Applications.Find(id);
                u.Applications.Add(foundModel);
            }
            db.Users.Add(u);
            db.SaveChanges();
            return Json("success");
        }
    }
}