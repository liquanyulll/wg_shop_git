using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Accounts
{
    public class InvitationModel
    {
        public string inv_code { get; set; }
        public int ProductId { get; set; }
        public int need_inv_count { get; set; }
        public int inv_count { get; set; }
    }
}
