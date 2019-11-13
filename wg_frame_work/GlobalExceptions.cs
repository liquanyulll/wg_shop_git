using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using wg_utils;

namespace wg_frame_work
{
    public class GlobalExceptions : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Result = new InternalServerErrorObjectResult(new ResponseModel { IsSuccess = false, Msg = context.Exception.Message, IsShowMsg = true, Data = null });
            }
            else
            {
                var guid = Guid.NewGuid().ToString();
                LogUtil.LogExceptionInfo("SystemError", context.Exception, $"系统错误:Guid{guid}");
                context.Result = new InternalServerErrorObjectResult(new ResponseModel { IsSuccess = false, Msg = "未知错误，请联系管理员，查询错误号：" + guid, IsShowMsg = true, Data = null });
            }
        }

        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status200OK;
            }
        }
    }
}
