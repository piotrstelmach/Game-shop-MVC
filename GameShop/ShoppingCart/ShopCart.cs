using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameShop.Models;

namespace GameShop.ShoppingCart
{
    public class ShopCart
    {


        private ShopDBEntities db = new ShopDBEntities();
        public string CartId { get; set; }
        public const string CartSessionKey = "ProductOrderId";

        public static ShopCart GetCart(HttpContextBase context)
        {
            var cart = new ShopCart();
            cart.CartId =GetCartId(context);
            return cart;

        }

        public static string GetCartId(HttpContextBase context)
        {

            if (context.Session[CartSessionKey] == null)
            {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();

            }
             return context.Session[CartSessionKey].ToString();
        }

        public void AddProductToCard(Product product)
        {
            var productCartItem = db.Products_to_order.SingleOrDefault(
                c => c.Cart_Id==CartId && c.Product_id == product.id);

            
            if (productCartItem == null)
            {
                
                productCartItem = new Products_to_order
                {
                    Product_id=product.id,
                    Amount = 1,
                    Product_total_price=product.Price,
                    Cart_Id=CartId 
                };
                db.Products_to_order.Add(productCartItem);
            }
            else
            {
                productCartItem.Amount++;
                productCartItem.Product_total_price += product.Price;
            }

            db.SaveChanges();
            
        }

        public void RemoveProduct(int id)
        {
            var productCartItem = db.Products_to_order.SingleOrDefault(
                 c => c.Cart_Id == CartId && c.Product_id==id);

            if (productCartItem != null)
            {
                if (productCartItem.Amount > 1)
                {
                    productCartItem.Amount--;
                    productCartItem.Product_total_price -= productCartItem.Product.Price;
                }
                else
                {
                    db.Products_to_order.Remove(productCartItem);
                }

                
            }
            db.SaveChanges();

        }
        public List<Products_to_order> getCartProducts()
        {
            return db.Products_to_order.Where(c => c.Cart_Id == CartId).ToList();
        }

        public decimal? CountTotalPrice()
        {
            decimal? count = db.Products_to_order.Where(c => c.Cart_Id == CartId).Sum(c => c.Product.Price);
            return count ?? decimal.Zero;
        }

    }
}