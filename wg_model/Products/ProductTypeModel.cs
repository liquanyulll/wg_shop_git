using System.Collections.Generic;

namespace wg_model.Products
{
    public class ProductTypeModel
    {
        public int Pt_Id { get; set; }
        public string Pt_Name { get; set; }
        public int Sort { get; set; }
        public string Enabled { get; set; }
        public string Deleted { get; set; }

        public List<ProductTypeModel> Childrens { get; set; }
    }
}
