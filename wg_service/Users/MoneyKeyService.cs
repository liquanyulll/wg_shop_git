using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using wg_core.Domain;
using wg_frame_work;
using wg_utils;

namespace wg_service.Users
{
    public class MoneyKeyService
    {
        private readonly ShopContext _context;

        public MoneyKeyService(ShopContext context)
        {
            _context = context;
        }

        public async Task<decimal> UserdMoneyKey(int userId, string mk)
        {
            var user = await _context.t1_user.Include(e=>e.t1_user_attr).FirstOrDefaultAsync(e => e.UserId == userId);
            if (user == null)
            {
                throw new NotImplementedException("用户不存在");
            }

            var moneyKey = await _context.t1_user_moneykey.FirstOrDefaultAsync(e => e.mony_key == mk && e.used == "N");
            if (moneyKey == null)
            {
                throw new NotImplementedException("卡密不存在");
            }
            moneyKey.used = "Y";
            moneyKey.used_time = DateTime.Now;
            moneyKey.used_ip = ServiceEngin.IP;
            moneyKey.used_user_id = user.UserId;
            user.t1_user_attr.Amount += moneyKey.price;
            await _context.SaveChangesAsync();
            return user.t1_user_attr.Amount;
        }
    }
}
