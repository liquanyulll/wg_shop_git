using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Accounts
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Mobile { get; set; }
        public string SMSCode { get; set; }
        public string CodeDes { get; set; }
        public string VerfyCode { get; set; }

        #region
        public string LoginTime { get; set; }
        public string LoginIP { get; set; }
        public decimal? Amount { get; set; }
        #endregion
    }
}
