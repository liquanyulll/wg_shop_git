using System;
using System.Collections.Generic;
using System.Text;

namespace wg_frame_work
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public bool IsShowMsg { get; set; }
        public Object Data { get; set; }
    }
}
