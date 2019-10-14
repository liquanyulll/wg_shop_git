using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace wg_frame_work
{
    public class BaseController : Controller
    {
        public JsonResult SucessResult(object data, bool isShowMsg = false)
        {
            return Json(new ResponseModel { IsSuccess = true, Msg = null, IsShowMsg = isShowMsg, Data = data });
        }

        public JsonResult Error(string msg)
        {
            return Json(new ResponseModel { IsSuccess = false, Msg = msg, IsShowMsg = true, Data = null });
        }

        public JsonResult Sucess(string msg)
        {
            return Json(new ResponseModel { IsSuccess = true, Msg = msg, IsShowMsg = true, Data = null });
        }
    }
}
