using GameShop.Models;
using GameShop.PayPalSDK;
using GameShop.ShoppingCart;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace GameShop.Controllers
{
    public class BasketOrderController : Controller
    {
        // GET: BasketOrder
        public ActionResult Index()
        {
            ShopDBEntities db = new ShopDBEntities();
            List<Products_order> _order = new List<Products_order>() { new Products_order() };
            var cart = ShopCart.GetCart(this.HttpContext);

            return View(new BasketOrderViewModel()
            {
                Products = cart.getCartProducts(),
                Order = _order
            });
        }

        public ActionResult PayPalPayment()
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/BasketOrder/PayPalPayment?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return View("FailureView");
            }
            return View("SuccessView");
        }

        [HttpPost]
        public ActionResult PayWithPayPal(FormCollection formCollection)
        {
            ShopDBEntities dBEntities = new ShopDBEntities();
            var products = ShopCart.GetCartId(this.HttpContext);
            var productsToOrderId = dBEntities.Products_to_order.SingleOrDefault(c => c.Cart_Id == products) as Products_to_order;
            
            Products_order order = new Products_order
            {
                Order_status_id = 1,
                Payed = false,
                Sended = false,
                Order_date = DateTime.Now,
                Address = new Models.Address
                {
                    Country = formCollection["order.Address.Country"],
                    Flat_number = Convert.ToInt32(formCollection["order.Address.Flat_number"]),
                    House_number = Convert.ToInt32(formCollection["order.Address.House_number"]),
                    Street = formCollection["order.Address.Street"],
                    Zip_code = formCollection["order.Address.Zip_code"]
                },
                First_Name = formCollection["order.First_Name"],
                Last_Name =formCollection["order.Last_Name"],
                User_id = User.Identity.GetUserId(),
                Total_price = Convert.ToDecimal(formCollection["order.Total_price"]),
                Products_to_order_id = productsToOrderId.id_Products_to_order
            };
            dBEntities.Products_order.Add(order);
            dBEntities.SaveChanges();


            //Paypal
            return RedirectToAction("PayPalPayment");
            //on successful payment, show success page to user.
            //var tmpOrder = dBEntities.Products_order.Select(c => c.id == productsToOrderId.id_Products_to_order) as Products_order;
            //order.Payed = true;
            //order.Order_status_id = 3;
            //dBEntities.SaveChanges();
            //return View("SuccessView");

        }

        private Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var products = ShopCart.GetCart(this.HttpContext).getCartProducts();
            decimal totalPrice=0.00m;
            var tax = 0;
            var shipping = 0;
            var subtotal = 0;

            foreach(var product in products)
            {
                totalPrice += product.Product.Price;
            }

            var ItemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var product in products)
            {
                ItemList.items.Add(new Item
                {
                    
                    name = product.Product.Product_name,
                    currency = "PLN",
                    quantity = product.Amount.ToString(),
                    price = (tax + shipping + subtotal+product.Product.Price).ToString("N", CultureInfo.CreateSpecificCulture("en-US").NumberFormat),
                    
                });
            }

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "Cancel=true",
                return_url = redirectUrl
            };

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            //var orderDetails = new Details()
            //{
            //    tax = tax.ToString(),
            //    shipping = shipping.ToString(),
            //    subtotal =subtotal.ToString()
            //};

            var amount = new Amount()
            {
                total=(totalPrice+tax+shipping+subtotal).ToString("N", CultureInfo.CreateSpecificCulture("en-US").NumberFormat),
                currency = "PLN",
                //details = orderDetails
            };

            var transaction = new List<Transaction>();
            transaction.Add(new Transaction()
            {
                description = "Your GameShopProducts",
                amount = amount,
                item_list=ItemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transaction,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }
    }
}