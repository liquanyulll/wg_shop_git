using System;
using System.Collections.Generic;

#nullable disable

namespace wg_core.Domain
{
    public partial class t3_user_product_invitation
    {
        public t3_user_product_invitation()
        {
            t3_user_product_invitation_ips = new HashSet<t3_user_product_invitation_ip>();
        }

        public string inv_code { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int need_inv_count { get; set; }
        public int inv_count { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual t2_product Product { get; set; }
        public virtual t1_user User { get; set; }
        public virtual ICollection<t3_user_product_invitation_ip> t3_user_product_invitation_ips { get; set; }
    }
}
