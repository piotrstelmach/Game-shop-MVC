using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameShop.Models;

namespace GameShop.Controllers.CRUD
{
    public class AvalibilitiesController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Avalibilities
        public ActionResult Index()
        {
            return View(db.Avalibility.ToList());
        }

        // GET: Avalibilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avalibility avalibility = db.Avalibility.Find(id);
            if (avalibility == null)
            {
                return HttpNotFound();
            }
            return View(avalibility);
        }

        // GET: Avalibilities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Avalibilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Avalibility1")] Avalibility avalibility)
        {
            if (ModelState.IsValid)
            {
                db.Avalibility.Add(avalibility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(avalibility);
        }

        // GET: Avalibilities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avalibility avalibility = db.Avalibility.Find(id);
            if (avalibility == null)
            {
                return HttpNotFound();
            }
            return View(avalibility);
        }

        // POST: Avalibilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Avalibility1")] Avalibility avalibility)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avalibility).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(avalibility);
        }

        // GET: Avalibilities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avalibility avalibility = db.Avalibility.Find(id);
            if (avalibility == null)
            {
                return HttpNotFound();
            }
            return View(avalibility);
        }

        // POST: Avalibilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Avalibility avalibility = db.Avalibility.Find(id);
            db.Avalibility.Remove(avalibility);
            db.SaveChanges();
            return RedirectToAction("Index");
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
