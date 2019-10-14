using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t2_product_detail_Img
    {
        public int img_id { get; set; }
        public int ProductId { get; set; }
        public string img_url { get; set; }
        public string img_alt { get; set; }
        public int? img_desc { get; set; }
        public int Sort { get; set; }
        public string Enabled { get; set; }

        public virtual t2_product Product { get; set; }
    }
}
