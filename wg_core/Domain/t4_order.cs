using System;
using System.Collections.Generic;

#nullable disable

namespace wg_core.Domain
{
    public partial class t4_order
    {
        public int order_id { get; set; }
        public string order_guid { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public decimal Amt { get; set; }
        public string pay_way { get; set; }
        public DateTime created_time { get; set; }

        public virtual t2_product Product { get; set; }
        public virtual t1_user User { get; set; }
    }
}
