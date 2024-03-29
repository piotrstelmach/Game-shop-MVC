//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Products_to_order = new HashSet<Products_to_order>();
        }
    
        public int id { get; set; }
        public string Product_name { get; set; }
        public string Description { get; set; }
        public Nullable<int> Category_id { get; set; }
        public decimal Price { get; set; }
        public Nullable<int> Language_id { get; set; }
        public Nullable<int> Avalibility_id { get; set; }
        public Nullable<int> Producent_id { get; set; }
    
        public virtual Avalibility Avalibility { get; set; }
        public virtual Category Category { get; set; }
        public virtual Language Language { get; set; }
        public virtual Producent Producent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products_to_order> Products_to_order { get; set; }
    }
}
