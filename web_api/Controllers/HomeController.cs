using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wg_frame_work;
using wg_utils;

namespace web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    {
        public HomeController() {

        }

        [SkipUserFilter]
        [HttpGet("GetVerifCode")]
        public JsonResult GetVerifCode(int codeLength = 4)
        {
            var codeDesResult = EncyryptionUtil.DESEncrypt(SystemUtil.GetRandomVGCharac(codeLength));
            var result = new { codeDes = codeDesResult, imgUrl = WebConfig.SystemWebDomain+"/api/home/GetVerfyPic?code=" + codeDesResult };
            return SucessResult(result);
        }

        [SkipUserFilter]
        [HttpGet("GetVerfyPic")]
        public ActionResult GetVerfyPic(string code)
        {
            var accCode = EncyryptionUtil.DESDecrypt(code);
            var bytes = SystemUtil.CreateValidateGraphic(accCode);
            return File(bytes, @"image/jpeg");
        }

        [SkipUserFilter]
        [HttpGet("CheckVCode")]
        public ActionResult CheckVCode(string code, string codeDes, string mobile = null, string smsType = null)
        {
            var result = false;
            if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(smsType))
            {
                try
                {
                    result = string.Equals(code, EncyryptionUtil.DESDecrypt(codeDes), StringComparison.InvariantCultureIgnoreCase);
                }
                catch { }
            }
            else
            {
                result = SMSCodeBaseFunc.CheckSmsCode(mobile, code, smsType, false);
            }

            return SucessResult(result ? "Y" : "N");
        }

        /// <summary>
        /// 输入随机码长度，获取短信验证随机码，并发送
        /// 返回的内容是短信验证码的DES
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        [SkipUserFilter]
        [HttpGet("SendSmsVCode")]
        public ActionResult SendSmsVCode(string VGcode, string VGcodeDes, string targetMobile, string smsType = "all")
        {
            try
            {
                var VGCheck = string.Equals(VGcode, EncyryptionUtil.DESDecrypt(VGcodeDes), StringComparison.InvariantCultureIgnoreCase);
                if (!VGCheck)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new NotImplementedException("图片验证码不正确");
            }

            var code = SMSCodeBaseFunc.SendSmsCode(targetMobile, smsType);
            var codeDesResult = EncyryptionUtil.DESEncrypt(code.ToString());

            return Json(new { codeDes = codeDesResult });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
