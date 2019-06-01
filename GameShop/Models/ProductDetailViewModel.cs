using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class ProductDetailViewModel
    {
        public List<Product> Product { get; set; }
        public List<Category> Categories { get; set; }

    }
}