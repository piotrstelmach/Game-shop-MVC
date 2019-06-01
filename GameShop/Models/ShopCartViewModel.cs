using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class ShopCartViewModel
    {
        public List<Products_to_order> Products{ get; set; }
        public string cartId { get; set; }
    }
}