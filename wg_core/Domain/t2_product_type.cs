using System;
using System.Collections.Generic;

namespace wg_core.Domain
{
    public partial class t2_product_type
    {
        public t2_product_type()
        {
            t2_product = new HashSet<t2_product>();
        }

        public int Pt_Id { get; set; }
        public string Pt_Name { get; set; }
        public int Sort { get; set; }
        public string Enabled { get; set; }
        public string Deleted { get; set; }

        public virtual ICollection<t2_product> t2_product { get; set; }
    }
}
