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
    public class Products_to_orderController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Products_to_order
        public ActionResult Index()
        {
            var products_to_order = db.Products_to_order.Include(p => p.Product);
            return View(products_to_order.ToList());
        }

        // GET: Products_to_order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_to_order products_to_order = db.Products_to_order.Find(id);
            if (products_to_order == null)
            {
                return HttpNotFound();
            }
            return View(products_to_order);
        }

        // GET: Products_to_order/Create
        public ActionResult Create()
        {
            ViewBag.Product_id = new SelectList(db.Product, "id", "Product_name");
            return View();
        }

        // POST: Products_to_order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Products_to_order,Amount,Product_total_price,Product_id")] Products_to_order products_to_order)
        {
            if (ModelState.IsValid)
            {
                db.Products_to_order.Add(products_to_order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Product_id = new SelectList(db.Product, "id", "Product_name", products_to_order.Product_id);
            return View(products_to_order);
        }

        // GET: Products_to_order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_to_order products_to_order = db.Products_to_order.Find(id);
            if (products_to_order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_id = new SelectList(db.Product, "id", "Product_name", products_to_order.Product_id);
            return View(products_to_order);
        }

        // POST: Products_to_order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Products_to_order,Amount,Product_total_price,Product_id")] Products_to_order products_to_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products_to_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product_id = new SelectList(db.Product, "id", "Product_name", products_to_order.Product_id);
            return View(products_to_order);
        }

        // GET: Products_to_order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_to_order products_to_order = db.Products_to_order.Find(id);
            if (products_to_order == null)
            {
                return HttpNotFound();
            }
            return View(products_to_order);
        }

        // POST: Products_to_order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products_to_order products_to_order = db.Products_to_order.Find(id);
            db.Products_to_order.Remove(products_to_order);
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
