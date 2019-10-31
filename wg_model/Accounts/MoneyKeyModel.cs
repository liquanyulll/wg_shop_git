using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Accounts
{
    public class MoneyKeyModel
    {
        public int mk_id { get; set; }
        public string mony_key { get; set; }
        public decimal price { get; set; }
        public string used_ip { get; set; }
        public DateTime used_time { get; set; }
        public string plat { get; set; }
    }
}
