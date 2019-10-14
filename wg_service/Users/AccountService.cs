using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using wg_core.Domain;

namespace wg_service.Users
{
    public class AccountService
    {
        private readonly ShopContext _context;

        public AccountService(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExistUserName(string userName)
        {
            return await _context.t1_user.AnyAsync(e=>(e.UserName == userName || e.Mobile == userName));
        }

        public async Task<t1_user> GetBy(string userName, string password)
        {
            return await _context.t1_user.Include(e => e.t1_user_attr).FirstOrDefaultAsync(e => (e.UserName == userName || e.Mobile == userName) && e.PassWord == password);
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
