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
    public class Products_orderController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Products_order
        public ActionResult Index()
        {
            var products_order = db.Products_order.Include(p => p.Address).Include(p => p.AspNetUsers).Include(p => p.Order_status).Include(p => p.Products_to_order);
            return View(products_order.ToList());
        }

        // GET: Products_order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_order products_order = db.Products_order.Find(id);
            if (products_order == null)
            {
                return HttpNotFound();
            }
            return View(products_order);
        }

        // GET: Products_order/Create
        public ActionResult Create()
        {
            ViewBag.Address_id = new SelectList(db.Address, "id", "Street");
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Order_status_id = new SelectList(db.Order_status, "id", "Order_status1");
            ViewBag.Products_to_order_id = new SelectList(db.Products_to_order, "id_Products_to_order", "id_Products_to_order");
            return View();
        }

        // POST: Products_order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Order_status_id,Payed,Sended,Order_date,Sended_date,Address_id,First_Name,Last_Name,User_id,Products_to_order_id,Total_price")] Products_order products_order)
        {
            if (ModelState.IsValid)
            {
                db.Products_order.Add(products_order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Address_id = new SelectList(db.Address, "id", "Street", products_order.Address_id);
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", products_order.User_id);
            ViewBag.Order_status_id = new SelectList(db.Order_status, "id", "Order_status1", products_order.Order_status_id);
            ViewBag.Products_to_order_id = new SelectList(db.Products_to_order, "id_Products_to_order", "id_Products_to_order", products_order.Products_to_order_id);
            return View(products_order);
        }

        // GET: Products_order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_order products_order = db.Products_order.Find(id);
            if (products_order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Address_id = new SelectList(db.Address, "id", "Street", products_order.Address_id);
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", products_order.User_id);
            ViewBag.Order_status_id = new SelectList(db.Order_status, "id", "Order_status1", products_order.Order_status_id);
            ViewBag.Products_to_order_id = new SelectList(db.Products_to_order, "id_Products_to_order", "id_Products_to_order", products_order.Products_to_order_id);
            return View(products_order);
        }

        // POST: Products_order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Order_status_id,Payed,Sended,Order_date,Sended_date,Address_id,First_Name,Last_Name,User_id,Products_to_order_id,Total_price")] Products_order products_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Address_id = new SelectList(db.Address, "id", "Street", products_order.Address_id);
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", products_order.User_id);
            ViewBag.Order_status_id = new SelectList(db.Order_status, "id", "Order_status1", products_order.Order_status_id);
            ViewBag.Products_to_order_id = new SelectList(db.Products_to_order, "id_Products_to_order", "id_Products_to_order", products_order.Products_to_order_id);
            return View(products_order);
        }

        // GET: Products_order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_order products_order = db.Products_order.Find(id);
            if (products_order == null)
            {
                return HttpNotFound();
            }
            return View(products_order);
        }

        // POST: Products_order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products_order products_order = db.Products_order.Find(id);
            db.Products_order.Remove(products_order);
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
