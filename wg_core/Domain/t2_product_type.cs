﻿using System;
using System.Collections.Generic;

#nullable disable

namespace wg_core.Domain
{
    public partial class t2_product_type
    {
        public t2_product_type()
        {
            InversePt_Parent = new HashSet<t2_product_type>();
        }

        public int Pt_Id { get; set; }
        public int? Pt_ParentId { get; set; }
        public string Pt_Name { get; set; }
        public int? Sort { get; set; }
        public string Enabled { get; set; }

        public virtual t2_product_type Pt_Parent { get; set; }
        public virtual ICollection<t2_product_type> InversePt_Parent { get; set; }
    }
}
