using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRuou.Models
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageURL { get; set; }
        // Thêm Navigation Property
        public Product Product { get; set; }
    }
}