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
    public class Order_statusController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Order_status
        public ActionResult Index()
        {
            return View(db.Order_status.ToList());
        }

        // GET: Order_status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_status order_status = db.Order_status.Find(id);
            if (order_status == null)
            {
                return HttpNotFound();
            }
            return View(order_status);
        }

        // GET: Order_status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order_status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Order_status1")] Order_status order_status)
        {
            if (ModelState.IsValid)
            {
                db.Order_status.Add(order_status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order_status);
        }

        // GET: Order_status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_status order_status = db.Order_status.Find(id);
            if (order_status == null)
            {
                return HttpNotFound();
            }
            return View(order_status);
        }

        // POST: Order_status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Order_status1")] Order_status order_status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order_status);
        }

        // GET: Order_status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_status order_status = db.Order_status.Find(id);
            if (order_status == null)
            {
                return HttpNotFound();
            }
            return View(order_status);
        }

        // POST: Order_status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order_status order_status = db.Order_status.Find(id);
            db.Order_status.Remove(order_status);
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
