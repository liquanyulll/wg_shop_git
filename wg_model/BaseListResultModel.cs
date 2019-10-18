using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model
{
    public class BaseListResultModel
    {
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public object ContentList { get; set; }
    }
}
