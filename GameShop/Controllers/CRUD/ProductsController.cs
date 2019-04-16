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
    public class ProductsController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Products
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Avalibility).Include(p => p.Category).Include(p => p.Language).Include(p => p.Producent);
            return View(product.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Avalibility_id = new SelectList(db.Avalibility, "id", "Avalibility1");
            ViewBag.Category_id = new SelectList(db.Category, "id", "Category1");
            ViewBag.Language_id = new SelectList(db.Language, "id", "Language_name");
            ViewBag.Producent_id = new SelectList(db.Producent, "id", "Producent_name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Product_name,Description,Category_id,Price,Language_id,Avalibility_id,Producent_id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Avalibility_id = new SelectList(db.Avalibility, "id", "Avalibility1", product.Avalibility_id);
            ViewBag.Category_id = new SelectList(db.Category, "id", "Category1", product.Category_id);
            ViewBag.Language_id = new SelectList(db.Language, "id", "Language_name", product.Language_id);
            ViewBag.Producent_id = new SelectList(db.Producent, "id", "Producent_name", product.Producent_id);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Avalibility_id = new SelectList(db.Avalibility, "id", "Avalibility1", product.Avalibility_id);
            ViewBag.Category_id = new SelectList(db.Category, "id", "Category1", product.Category_id);
            ViewBag.Language_id = new SelectList(db.Language, "id", "Language_name", product.Language_id);
            ViewBag.Producent_id = new SelectList(db.Producent, "id", "Producent_name", product.Producent_id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Product_name,Description,Category_id,Price,Language_id,Avalibility_id,Producent_id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Avalibility_id = new SelectList(db.Avalibility, "id", "Avalibility1", product.Avalibility_id);
            ViewBag.Category_id = new SelectList(db.Category, "id", "Category1", product.Category_id);
            ViewBag.Language_id = new SelectList(db.Language, "id", "Language_name", product.Language_id);
            ViewBag.Producent_id = new SelectList(db.Producent, "id", "Producent_name", product.Producent_id);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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
