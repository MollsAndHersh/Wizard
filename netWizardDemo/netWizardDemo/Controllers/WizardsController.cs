using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace netWizardDemo.Controllers
{
    public class WizardsController : Controller
    {
        // GET: Wizards
        public ActionResult Index()
        {
            return View();
        }
    }
}