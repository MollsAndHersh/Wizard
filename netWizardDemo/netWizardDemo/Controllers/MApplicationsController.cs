using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using netWizardDemo.Models;
using netWizardDemo.Models.Contexts;

namespace netWizardDemo.Controllers
{
    public class MApplicationsController : Controller
    {
        private testDbContext db = new testDbContext();

        // GET: MApplications
        public ActionResult Index()
        {
            return View(db.Applications.ToList());
        }

        // GET: MApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MApplication mApplication = db.Applications.Find(id);
            if (mApplication == null)
            {
                return HttpNotFound();
            }
            return View(mApplication);
        }

        // GET: MApplications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] MApplication mApplication)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(mApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mApplication);
        }

        // GET: MApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MApplication mApplication = db.Applications.Find(id);
            if (mApplication == null)
            {
                return HttpNotFound();
            }
            return View(mApplication);
        }

        // POST: MApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] MApplication mApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mApplication);
        }

        // GET: MApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MApplication mApplication = db.Applications.Find(id);
            if (mApplication == null)
            {
                return HttpNotFound();
            }
            return View(mApplication);
        }

        // POST: MApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MApplication mApplication = db.Applications.Find(id);
            db.Applications.Remove(mApplication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ApplicationWizard()
        {
            ViewBag.ApplicationId = new SelectList(db.Applications, "Id", "Name");
            return PartialView("WizardTemplates/MApplication");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
