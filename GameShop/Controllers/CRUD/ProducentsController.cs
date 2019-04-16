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
    public class ProducentsController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Producents
        public ActionResult Index()
        {
            return View(db.Producent.ToList());
        }

        // GET: Producents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producent producent = db.Producent.Find(id);
            if (producent == null)
            {
                return HttpNotFound();
            }
            return View(producent);
        }

        // GET: Producents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Producent_name")] Producent producent)
        {
            if (ModelState.IsValid)
            {
                db.Producent.Add(producent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producent);
        }

        // GET: Producents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producent producent = db.Producent.Find(id);
            if (producent == null)
            {
                return HttpNotFound();
            }
            return View(producent);
        }

        // POST: Producents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Producent_name")] Producent producent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producent);
        }

        // GET: Producents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producent producent = db.Producent.Find(id);
            if (producent == null)
            {
                return HttpNotFound();
            }
            return View(producent);
        }

        // POST: Producents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producent producent = db.Producent.Find(id);
            db.Producent.Remove(producent);
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
