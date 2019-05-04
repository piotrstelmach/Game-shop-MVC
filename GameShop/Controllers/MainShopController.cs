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
        // GET: MainShop
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
    }
}