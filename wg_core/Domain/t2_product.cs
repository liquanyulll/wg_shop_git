using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t2_product
    {
        public t2_product()
        {
            t2_product_detail_Img = new HashSet<t2_product_detail_Img>();
            t2_product_spec = new HashSet<t2_product_spec>();
            t3_user_product_invitation = new HashSet<t3_user_product_invitation>();
            t4_order = new HashSet<t4_order>();
        }

        public int ProductId { get; set; }
        public int Pt_Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string LogImg { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int Invs { get; set; }
        public int CreateUserId { get; set; }
        public string Enabled { get; set; }
        public string Deleted { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual t1_user CreateUser { get; set; }
        public virtual t2_product_type Pt_ { get; set; }
        public virtual t2_product_attr t2_product_attr { get; set; }
        public virtual ICollection<t2_product_detail_Img> t2_product_detail_Img { get; set; }
        public virtual ICollection<t2_product_spec> t2_product_spec { get; set; }
        public virtual ICollection<t3_user_product_invitation> t3_user_product_invitation { get; set; }
        public virtual ICollection<t4_order> t4_order { get; set; }
    }
}
