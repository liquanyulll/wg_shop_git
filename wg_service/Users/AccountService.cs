using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using wg_core.Domain;
using wg_utils;

namespace wg_service.Users
{
    public class AccountService
    {
        private readonly ShopContext _context;

        public AccountService(ShopContext context)
        {
            _context = context;
        }

        public t1_user CreateEntity(string userName, string password)
        {
            var user = new t1_user();
            user.UserName = userName;
            user.Mobile = userName;
            user.Mobile_Valid = "Y";
            user.EncryptionType = "AES";
            user.PassWord = EncyryptionUtil.AESEncrypt(password);
            user.CreatedTime = DateTime.Now;
            //附属
            user.t1_user_attr = new t1_user_attr
            {
                Amount = 0,//账户上余额
            };
            return user;
        }

        public async Task<bool> IsExistUserName(string userName)
        {
            return await _context.t1_user.AnyAsync(e => (e.UserName == userName || e.Mobile == userName));
        }

        public async Task<t1_user> GetBy(string userName, string password)
        {
            return await _context.t1_user.Include(e => e.t1_user_attr).FirstOrDefaultAsync(e => (e.UserName == userName || e.Mobile == userName) && e.PassWord == password);
        }

        /// <summary>
        /// 重设密码，如果没有就注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task ResetPassword(string userName, string password)
        {
            var user = await _context.t1_user.FirstOrDefaultAsync(e => e.UserName == userName);
            if (user == null)
            {
                user = CreateEntity(userName, password);
                _context.t1_user.Add(user);
            }
            else
            {
                user.PassWord = EncyryptionUtil.AESEncrypt(password);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Insert(t1_user entity)
        {
            entity.t1_user_attr = new t1_user_attr
            {
                Amount = 0,//账户上余额
            };
            await _context.t1_user.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
