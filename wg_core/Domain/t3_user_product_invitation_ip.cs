using System;
using System.Collections.Generic;

#nullable disable

namespace wg_core.Domain
{
    public partial class t3_user_product_invitation_ip
    {
        public int Id { get; set; }
        public string inv_code { get; set; }
        public string inv_ip { get; set; }
        public int count { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual t3_user_product_invitation inv_codeNavigation { get; set; }
    }
}
