using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wg_frame_work
{
    public class UserFilterAttribute : ActionFilterAttribute
    {
        private readonly AuthenticationSupport _authenticationSerivce;

        public UserFilterAttribute(AuthenticationSupport authenticationSerivce)
        {
            _authenticationSerivce = authenticationSerivce;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //判断是否需要忽略用户验证
            if (context.ActionDescriptor.FilterDescriptors.Any(e => e.Filter.GetType().Name == nameof(SkipUserFilterAttribute)))
            {
                return;
            }

            if (!_authenticationSerivce.Authoriz())
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ObjectResult(new { code = 401, sub_msg = "未授权" });
            }
        }
    }
}
