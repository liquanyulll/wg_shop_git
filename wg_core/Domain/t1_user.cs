using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t1_user
    {
        public t1_user()
        {
            t1_user_moneykey = new HashSet<t1_user_moneykey>();
            t2_product = new HashSet<t2_product>();
            t3_user_product_invitation = new HashSet<t3_user_product_invitation>();
            t4_order = new HashSet<t4_order>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string EncryptionType { get; set; }
        public string Mobile { get; set; }
        public string Mobile_Valid { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual t1_user_attr t1_user_attr { get; set; }
        public virtual ICollection<t1_user_moneykey> t1_user_moneykey { get; set; }
        public virtual ICollection<t2_product> t2_product { get; set; }
        public virtual ICollection<t3_user_product_invitation> t3_user_product_invitation { get; set; }
        public virtual ICollection<t4_order> t4_order { get; set; }
    }
}
