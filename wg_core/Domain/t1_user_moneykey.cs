using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t1_user_moneykey
    {
        public int mk_id { get; set; }
        public string mony_key { get; set; }
        public decimal price { get; set; }
        public string used { get; set; }
        public int? used_user_id { get; set; }
        public string used_ip { get; set; }
        public DateTime? used_time { get; set; }
        public DateTime created_time { get; set; }
        public string plat { get; set; }

        public virtual t1_user used_user_ { get; set; }
    }
}
