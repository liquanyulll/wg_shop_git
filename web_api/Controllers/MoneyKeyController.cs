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
    [Route("api/MoneyKey")]
    public class MoneyKeyController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly AccountService _accountService;
        private readonly AuthenticationSupport _authenticationSupport;
        private readonly MoneyKeyService _moneyKeyService;

        public MoneyKeyController(IMapper mapper, AccountService accountService, AuthenticationSupport authenticationSupport, MoneyKeyService moneyKeyService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _authenticationSupport = authenticationSupport;
            _moneyKeyService = moneyKeyService;
        }

        [HttpPost("UsedMk")]
        public async Task<JsonResult> UsedMk(string mk)
        {
            var user = _authenticationSupport.CurrentUser;
            var amount = await _moneyKeyService.UserdMoneyKey(user.UserId, mk);
            //更新缓存余额
            _authenticationSupport.ReloadUserAmountCache(amount);
            return Sucess("充值成功");
        }
    }
}
