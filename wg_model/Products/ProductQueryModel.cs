using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Products
{
    public class ProductQueryModel : BaseListQueryModel
    {
        public int? TypeId { get; set; }
        public string TypeAll { get; set; }
        public string PName { get; set; }
    }
}
