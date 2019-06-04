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
using System.Net.Mail;
using System.Net;

namespace GameShop.Controllers
{
    public class BasketOrderController : Controller
    {
        // GET: BasketOrder
        [Authorize(Roles ="User, Admin, Manager")]
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

        [Authorize(Roles = "User, Admin, Manager")]
        [HttpGet]
        public ActionResult PayPalPayment()
            
        {
            ShopDBEntities db = new ShopDBEntities();
            APIContext apiContext = PayPalConfiguration.GetAPIContext();
            try
            {

                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/BasketOrder/PayPalPayment?";

                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {

                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("SuccessView");
        }

        [Authorize(Roles = "User, Admin, Manager")]
        [HttpPost]
        public ActionResult PayWithPayPal(FormCollection formCollection)
        {
            ShopDBEntities dBEntities = new ShopDBEntities();
            var products = ShopCart.GetCartId(this.HttpContext);
            var productsToOrderId = dBEntities.Products_to_order.Where(c => c.Cart_Id == products).ToList();
            
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
                Products_to_order_id = productsToOrderId[0].id_Products_to_order
            };
            dBEntities.Products_order.Add(order);
            dBEntities.SaveChanges();

            sendOrderEmail(User.Identity.Name,false);

            var idOrder = order.id;
            //Paypal
            return RedirectToAction("PayPalPayment", "BasketOrder", new { id=idOrder });

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

        private void sendOrderEmail(string userEmail,bool payed)
        {
            var basicCredential = new NetworkCredential("mygameshopadm@gmail.com", "HelloWorld1");
            string companyEmail= "mygameshopadm@gmail.com";

            MailMessage mail = new MailMessage(companyEmail, userEmail);

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = true;
            //smtpClient.EnableSsl = true;
            smtpClient.Timeout = 100000;
            

            mail.Subject = "Your order have been accepted";
            if (payed)
            {
                mail.Body = "Hello " + User.Identity.Name + ".\nYour order have been added to our system. Please pay with PayPal.";
            }
            else
            {
                mail.Body = "Hello " + User.Identity.Name + ".\nYour order successful payed. We send your products soon";
            }
            smtpClient.Send(mail);


        }
    }
}