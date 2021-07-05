using System;
using System.Collections.Generic;

#nullable disable

namespace wg_core.Domain
{
    public partial class t1_user_login_history
    {
        public int his_id { get; set; }
        public int user_id { get; set; }
        public DateTime login_time { get; set; }
        public string ipaddress { get; set; }

        public virtual t1_user user { get; set; }
    }
}
