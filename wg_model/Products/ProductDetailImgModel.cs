using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Products
{
	public class ProductDetailImgModel
	{
		public int img_id { get; set; }
		public int ProductId { get; set; }
		public string img_url { get; set; }
		public string img_alt { get; set; }
		public string img_desc { get; set; }
		public string img_sort { get; set; }
		public int Sort { get; set; }
		public string Enabled { get; set; }
	}
}
