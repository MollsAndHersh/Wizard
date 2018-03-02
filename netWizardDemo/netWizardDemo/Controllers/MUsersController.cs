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
    public class MUsersController : Controller
    {
        private testDbContext db = new testDbContext();

        // GET: MUsers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: MUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MUser mUser = db.Users.Find(id);
            if (mUser == null)
            {
                return HttpNotFound();
            }
            return View(mUser);
        }

        // GET: MUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,FirstName,LastName,EMail")] MUser mUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(mUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mUser);
        }

        // GET: MUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MUser mUser = db.Users.Find(id);
            if (mUser == null)
            {
                return HttpNotFound();
            }
            return View(mUser);
        }

        // POST: MUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,FirstName,LastName,EMail")] MUser mUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mUser);
        }

        // GET: MUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MUser mUser = db.Users.Find(id);
            if (mUser == null)
            {
                return HttpNotFound();
            }
            return View(mUser);
        }

        // POST: MUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MUser mUser = db.Users.Find(id);
            db.Users.Remove(mUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UserWizard()
        {
            return PartialView("WizardTemplates/MUser");
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
