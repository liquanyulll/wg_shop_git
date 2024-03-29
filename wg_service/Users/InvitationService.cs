﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using wg_core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using wg_utils;

namespace wg_service.Users
{
    public class InvitationService
    {
        private readonly ShopContext _context;

        public InvitationService(ShopContext context)
        {
            _context = context;
        }

        private string GetCreateInvCode()
        {
            return SystemUtil.GenerateStringId().Substring(0, 6);
        }

        public async Task<t3_user_product_invitation> GetInvInfo(int userId, int pid)
        {
            return await _context.t3_user_product_invitations.
                Include(e => e.t3_user_product_invitation_ips)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ProductId == pid);
        }

        public async Task<t3_user_product_invitation> Create(int userId, int pid)
        {
            var product = await _context.t2_products.FirstOrDefaultAsync(e => e.ProductId == pid && e.Enabled == "Y" && e.Deleted == "N");
            if (product == null)
                throw new NotImplementedException("产品不存在");
            if (product.Invs == 0)
                throw new NotImplementedException("该产品不支持邀请方式购买");
            if (_context.t3_user_product_invitations.Any(e => e.UserId == userId && e.ProductId == pid))
                throw new NotImplementedException("已存在邀请记录，无需重复创建");

            var invCode = GetCreateInvCode();
            while (_context.t3_user_product_invitations.Any(e => e.inv_code == invCode))
            {
                invCode = GetCreateInvCode();
            }
            var inv = new t3_user_product_invitation
            {
                inv_code = invCode,
                UserId = userId,
                ProductId = pid,
                need_inv_count = product.Invs.HasValue ? product.Invs.Value : 0,
                inv_count = 0
            };
            _context.t3_user_product_invitations.Add(inv);
            await _context.SaveChangesAsync();
            return inv;
        }

        //记录邀请码不报错
        public async Task<int> LogInv(string invCode, string ip)
        {
            var invInfo = await _context.t3_user_product_invitations.Include(e => e.t3_user_product_invitation_ips).FirstOrDefaultAsync(e => e.inv_code == invCode);
            if (invInfo != null)
            {
                var invIp = invInfo.t3_user_product_invitation_ips.FirstOrDefault(e => e.inv_ip == ip);
                if (invIp == null)
                {
                    invInfo.t3_user_product_invitation_ips.Add(new t3_user_product_invitation_ip
                    {
                        count = 1,
                        inv_ip = ip,
                    });
                    invInfo.inv_count++;
                }
                else
                {
                    invIp.count++;
                }
                await _context.SaveChangesAsync();
                return invInfo.inv_count;
            }
            return 0;
        }
    }
}
