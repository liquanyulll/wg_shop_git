using System;
using System.Collections.Generic;

#nullable disable

namespace wg_core.Domain
{
    public partial class t1_user
    {
        public t1_user()
        {
            t1_user_login_histories = new HashSet<t1_user_login_history>();
            t1_user_moneykeys = new HashSet<t1_user_moneykey>();
            t2_products = new HashSet<t2_product>();
            t3_user_product_invitations = new HashSet<t3_user_product_invitation>();
            t4_orders = new HashSet<t4_order>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string EncryptionType { get; set; }
        public string Mobile { get; set; }
        public string Mobile_Valid { get; set; }
        public decimal? Money { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<t1_user_login_history> t1_user_login_histories { get; set; }
        public virtual ICollection<t1_user_moneykey> t1_user_moneykeys { get; set; }
        public virtual ICollection<t2_product> t2_products { get; set; }
        public virtual ICollection<t3_user_product_invitation> t3_user_product_invitations { get; set; }
        public virtual ICollection<t4_order> t4_orders { get; set; }
    }
}
