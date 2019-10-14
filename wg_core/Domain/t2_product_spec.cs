using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t2_product_spec
    {
        public int config_id { get; set; }
        public int ProductId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }

        public virtual t2_product Product { get; set; }
    }
}
