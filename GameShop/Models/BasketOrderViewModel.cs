using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class BasketOrderViewModel
    {
        public List<Products_to_order> Products { get; set; }

        public List<Products_order> Order { get; set; }
    }
}