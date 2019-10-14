using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t1_user_attr
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }

        public virtual t1_user User { get; set; }
    }
}
