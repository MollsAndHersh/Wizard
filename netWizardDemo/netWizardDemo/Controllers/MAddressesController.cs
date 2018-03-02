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
    public class MAddressesController : Controller
    {
        private testDbContext db = new testDbContext();

        // GET: MAddresses
        public ActionResult Index()
        {
            return View(db.Address.ToList());
        }

        // GET: MAddresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAddress mAddress = db.Address.Find(id);
            if (mAddress == null)
            {
                return HttpNotFound();
            }
            return View(mAddress);
        }

        // GET: MAddresses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StreetAddress,City,State,Zip")] MAddress mAddress)
        {
            if (ModelState.IsValid)
            {
                db.Address.Add(mAddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mAddress);
        }

        // GET: MAddresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAddress mAddress = db.Address.Find(id);
            if (mAddress == null)
            {
                return HttpNotFound();
            }
            return View(mAddress);
        }

        // POST: MAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StreetAddress,City,State,Zip")] MAddress mAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mAddress);
        }

        // GET: MAddresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAddress mAddress = db.Address.Find(id);
            if (mAddress == null)
            {
                return HttpNotFound();
            }
            return View(mAddress);
        }

        // POST: MAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MAddress mAddress = db.Address.Find(id);
            db.Address.Remove(mAddress);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddressWizard()
        {
            return PartialView("WizardTemplates/MAddress");
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
