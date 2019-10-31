using System;
using System.Collections.Generic;
using System.Text;
using wg_model.Accounts;

namespace wg_model.Products
{
    public class ProductModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string LogImg { get; set; }
        public decimal Price { get; set; }
        public string Inspection { get; set; }
        public int Invs { get; set; }
        public int Stock { get; set; }
        public bool IsBuy { get; set; }
        public string BuyWay { get; set; }
        public List<string> ProductSources { get; set; }
        public InvitationModel InvInfo { get; set; }
        public List<ProductDetailImgModel> DetailImgs { get; set; }
    }
}
