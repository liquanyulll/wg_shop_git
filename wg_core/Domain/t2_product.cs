using System;
using System.Collections.Generic;

#nullable disable

namespace wg_core.Domain
{
    public partial class t2_product
    {
        public t2_product()
        {
            t2_product_detail_Imgs = new HashSet<t2_product_detail_Img>();
            t2_product_specs = new HashSet<t2_product_spec>();
            t3_user_product_invitations = new HashSet<t3_user_product_invitation>();
            t4_orders = new HashSet<t4_order>();
        }

        public int ProductId { get; set; }
        public string PtIds { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string LogImg { get; set; }
        public decimal? Price { get; set; }
        public int? Invs { get; set; }
        public int? Stock { get; set; }
        public int CreateUserId { get; set; }
        public int? Collection { get; set; }
        public int? Click { get; set; }
        public string Enabled { get; set; }
        public string Deleted { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual t1_user CreateUser { get; set; }
        public virtual ICollection<t2_product_detail_Img> t2_product_detail_Imgs { get; set; }
        public virtual ICollection<t2_product_spec> t2_product_specs { get; set; }
        public virtual ICollection<t3_user_product_invitation> t3_user_product_invitations { get; set; }
        public virtual ICollection<t4_order> t4_orders { get; set; }
    }
}
