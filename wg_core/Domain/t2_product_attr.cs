using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t2_product_attr
    {
        public int ProductId { get; set; }
        public int Collection { get; set; }
        public int Click { get; set; }

        public virtual t2_product Product { get; set; }
    }
}
