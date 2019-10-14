using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using wg_model.Accounts;
using wg_utils;

namespace wg_frame_work
{
    public class AuthenticationSupport
    {
        public string APIToken
        {
            get
            {
                return ServiceEngin.Current.Request.Headers["X-Access-Token"];
            }
        }

        public UserModel CurrentUser
        {
            get
            {
                if (string.IsNullOrEmpty(APIToken))
                    return null;

                return RedisCachedHelper.Get<UserModel>(APIToken);
            }
        }

        public string SignIn(UserModel user)
        {
            var token = Guid.NewGuid().ToString();
            RedisCachedHelper.Set(token, user);
            return token;
        }

        public void SignOut(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                RedisCachedHelper.RemoveKey(token);
            }
        }

        public bool Authoriz()
        {
            var user = CurrentUser;

            //更新缓存时间
            if (user != null)
            {
                RedisCachedHelper.Set(APIToken, user);
            }

            return CurrentUser != null;
        }
    }
}
