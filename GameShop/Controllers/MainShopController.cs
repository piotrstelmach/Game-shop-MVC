using GameShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GameShop.Controllers
{
    
    public class MainShopController : Controller
    {
        private ShopDBEntities db;
        MainShopViewModel mainShopViewModel;
        ProductDetailViewModel productDetailViewModel;
        // GET: MainShop

        [AllowAnonymous]
        public ActionResult Index()
        {
            db = new ShopDBEntities();
             mainShopViewModel = new MainShopViewModel
            {
                Categories = db.Category.ToList(),
                Products = db.Product.ToList()
            };
            return View(mainShopViewModel);
        }

        [AllowAnonymous]
        public ActionResult GetProductByCategory(string categoryName)
        {
            db = new ShopDBEntities();
            mainShopViewModel = new MainShopViewModel();
            mainShopViewModel.Categories = db.Category.ToList();
            List<Product> productList = db.Product.Where(x => x.Category.Category1 == categoryName).ToList();
            mainShopViewModel.Products = new List<Product>();
            mainShopViewModel.Products = productList;
            return PartialView("Product_partial", mainShopViewModel);
        }

        [AllowAnonymous]
        public ActionResult ProductDetail(int id)
        {
            db = new ShopDBEntities();
            var product = db.Product.Where(c => c.id == id);
            var categories = db.Category.ToList();
            productDetailViewModel = new ProductDetailViewModel
            {
                Product =product.ToList(),
                Categories = categories
            };
            return View(productDetailViewModel);
        }
    }
}