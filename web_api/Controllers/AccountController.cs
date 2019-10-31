using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using wg_core.Domain;
using wg_frame_work;
using wg_model.Accounts;
using wg_service.Users;
using wg_utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_api.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly AccountService _accountService;
        private readonly AuthenticationSupport _authenticationSupport;
        private readonly InvitationService _invitationService;


        public AccountController(IMapper mapper,
            AccountService accountService,
            AuthenticationSupport authenticationSupport,
            InvitationService invitationService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _authenticationSupport = authenticationSupport;
            _invitationService = invitationService;
        }

        #region 用户 
        [HttpGet("CurrentUser")]
        [SkipUserFilter]
        public async Task<JsonResult> GetCurrentUser()
        {
            var user = _authenticationSupport.CurrentUser;
            return SucessResult(user);
        }

        [SkipUserFilter]
        [HttpPost("Register")]
        public async Task<JsonResult> Register([FromBody]UserModel regModel)
        {
            //短信验证登陆
            if (!SMSCodeBaseFunc.CheckSmsCode(regModel.UserName, regModel.SMSCode, "register"))
            {
                return Error("手机短信验证码错误！");
            }

            //去注册
            var user = _accountService.CreateEntity(regModel.UserName, regModel.PassWord);
            await _accountService.Insert(user);
            return Sucess("注册成功");
        }

        [SkipUserFilter]
        [HttpPost("Login")]
        public async Task<JsonResult> Login([FromBody]UserModel logModel)
        {
            if (string.IsNullOrEmpty(logModel.UserName) || string.IsNullOrEmpty(logModel.PassWord))
            {
                return Error("用户名或密码不能为空!");
            }

            if (string.IsNullOrEmpty(logModel.VerfyCode))
            {
                return Error("图形验证码不能为空!");
            }

            if (!string.Equals(logModel.VerfyCode, EncyryptionUtil.DESDecrypt(logModel.CodeDes), StringComparison.InvariantCultureIgnoreCase))
            {
                return Error("图形验证码错误!");
            }
            var pwd = EncyryptionUtil.AESEncrypt(logModel.PassWord);
            var user = await _accountService.GetBy(logModel.UserName, pwd);
            if (user != null)
            {
                var userModel = _mapper.Map<UserModel>(user);
                userModel.LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                userModel.LoginIP = HttpContext.Connection.RemoteIpAddress.ToString();
                userModel.Amount = user.t1_user_attr.Amount;
                var token = _authenticationSupport.SignIn(userModel);
                return SucessResult(token);
            }

            return Error("用户名或密码错误!");
        }

        [SkipUserFilter]
        [HttpPost("ResetPassword")]
        public async Task<JsonResult> ResetPassword([FromBody]UserModel regModel)
        {
            if (!SMSCodeBaseFunc.CheckSmsCode(regModel.UserName, regModel.SMSCode, "reset"))
            {
                return Error("手机短信验证码错误！");
            }

            await _accountService.ResetPassword(regModel.UserName, regModel.PassWord);

            return Sucess("成功，正在返回登陆");
        }

        [SkipUserFilter]
        [HttpGet("CheckUserName")]
        public async Task<JsonResult> CheckUserName(string userName)
        {
            var isExit = await _accountService.IsExistUserName(userName);
            return SucessResult(isExit ? "Y" : "N");
        }
        #endregion

        #region 分享邀请产品
        [HttpPost("CreateInvProduct")]
        public async Task<JsonResult> CreateInvProduct(int pid)
        {
            var user = _authenticationSupport.CurrentUser;
            var invInfo = await _invitationService.Create(user.UserId, pid);
            var model = _mapper.Map<InvitationModel>(invInfo);

            return SucessResult(model);
        }

        [SkipUserFilter]
        [HttpPost("GetInvInfo")]
        public async Task<JsonResult> GetInvInfo(int pid)
        {
            var user = _authenticationSupport.CurrentUser;
            if (user == null)
                return SucessResult(null);

            var invInfo = await _invitationService.GetInvInfo(user.UserId, pid);
            if (invInfo == null)
                return SucessResult(null);

            var model = _mapper.Map<InvitationModel>(invInfo);
            return SucessResult(model);
        }

        [SkipUserFilter]
        [HttpPost("LogInv")]
        public async Task<JsonResult> LogInv(string invCode)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            int count = await _invitationService.LogInv(invCode, ip);
            return SucessResult(count);
        }
        #endregion
    }
}
