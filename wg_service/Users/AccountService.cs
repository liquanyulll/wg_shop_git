using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wg_core;
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
            user.Money = 0;
            return user;
        }

        public async Task<bool> IsExistUserName(string userName)
        {
            return await _context.t1_users.AnyAsync(e => (e.UserName == userName || e.Mobile == userName));
        }

        public async Task<t1_user> GetBy(string userName, string password)
        {
            return await _context.t1_users.FirstOrDefaultAsync(e => (e.UserName == userName || e.Mobile == userName) && e.PassWord == password);
        }

        /// <summary>
        /// 重设密码，如果没有就注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task ResetPassword(string userName, string password)
        {
            var user = await _context.t1_users.FirstOrDefaultAsync(e => e.UserName == userName);
            if (user == null)
            {
                user = CreateEntity(userName, password);
                _context.t1_users.Add(user);
            }
            else
            {
                user.PassWord = EncyryptionUtil.AESEncrypt(password);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Insert(t1_user entity)
        {
            await _context.t1_users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public IPagedList<t1_user_login_history> SearchLoginHistory(int? userId = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _context.t1_user_login_histories.AsQueryable();
            if (userId.HasValue)
                query = query.Where(e => e.user_id == userId.Value);

            query = query.OrderByDescending(c => c.login_time);
            var result = new PagedList<t1_user_login_history>(query, pageIndex, pageSize);
            return result;
        }

        public async Task AddLoginIp(int userId, string ip)
        {
            _context.t1_user_login_histories.Add(new t1_user_login_history
            {
                login_time = DateTime.Now,
                ipaddress = ip,
                user_id = userId
            });
            await _context.SaveChangesAsync();
        }
    }
}
