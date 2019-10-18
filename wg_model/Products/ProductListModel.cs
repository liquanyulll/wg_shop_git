using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Products
{
    public class ProductListModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string LogImg { get; set; }
        public decimal Price { get; set; }
        public int Invs { get; set; }
        public int Stock { get; set; }
    }
}
