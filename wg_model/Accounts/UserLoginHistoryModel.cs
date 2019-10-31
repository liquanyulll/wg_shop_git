using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Accounts
{
    public class UserLoginHistoryModel
    {
        public DateTime login_time { get; set; }
        public string ipaddress { get; set; }
    }
}
