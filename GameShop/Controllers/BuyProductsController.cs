﻿using GameShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameShop.ShoppingCart;


namespace GameShop.Controllers
{
    public class BuyProductsController : Controller
    {
        public ShopDBEntities db = new ShopDBEntities();

        [Authorize(Roles = "User, Admin, Manager")]
        public ActionResult Index()
        {
            var cart = ShopCart.GetCart(this.HttpContext);

            var shopCartViewModel = new ShopCartViewModel
            {
                Products = cart.getCartProducts(),
                cartId = cart.CartId
        };
            return View(shopCartViewModel);
        }

        [Authorize(Roles = "User, Admin, Manager")]
        public ActionResult AddProductToCart(int id)
        {
            var product = db.Product.Single(p=>p.id==id);
            var cart=ShopCart.GetCart(this.HttpContext);

            cart.AddProductToCard(product);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User, Admin, Manager")]
        public ActionResult RemoveProductFromCart(int id)
        {
            var cart = ShopCart.GetCart(this.HttpContext);

            cart.RemoveProduct(id);

            return RedirectToAction("Index");
        }


    }
}