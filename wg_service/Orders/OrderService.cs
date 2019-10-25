using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using wg_core.Domain;
using wg_frame_work;
using wg_service.Users;
using wg_utils;

namespace wg_service.Orders
{
    public class OrderService
    {
        private readonly ShopContext _context;

        private readonly InvitationService _invitationService;
        private readonly AuthenticationSupport _authenticationSupport;

        public OrderService(ShopContext context,
            InvitationService invitationService,
            AuthenticationSupport authenticationSupport)
        {
            _context = context;
            _invitationService = invitationService;
            _authenticationSupport = authenticationSupport;
        }

        public async Task<t4_order> GetOrderInfo(int userId, int pid)
        {
            return await _context.t4_order.FirstOrDefaultAsync(e => e.UserId == userId && e.ProductId == pid);
        }

        public async Task CreateOrder(int userId, string way, int pid)
        {
            var product = await _context.t2_product.FirstOrDefaultAsync(e => e.ProductId == pid);
            if (product == null)
                throw new NotImplementedException("产品不存在！");

            var user = await _context.t1_user.Include(e => e.t1_user_attr).FirstOrDefaultAsync(e => e.UserId == userId);
            if (user == null)
                throw new NotImplementedException("用户不存在！");

            switch (way)
            {
                case "inv":
                    var invInfo = await _invitationService.GetInvInfo(user.UserId, pid);
                    if (invInfo == null)
                        throw new NotImplementedException("无邀请记录，不可购买");
                    if (invInfo.inv_count < invInfo.need_inv_count)
                        throw new NotImplementedException($"邀请次数不够，还需{invInfo.need_inv_count - invInfo.inv_count}次");
                    break;
                case "amt":
                    if (product.Price == 0)
                        throw new NotImplementedException($"该产品不支持现金购买");
                    if (user.t1_user_attr.Amount < product.Price)
                        throw new NotImplementedException("用户余额不足，请于个人中心充值");

                    //扣钱
                    user.t1_user_attr.Amount = user.t1_user_attr.Amount - product.Price;
                    break;
            }

            _context.t4_order.Add(new t4_order
            {
                order_guid = SystemUtil.GenerateStringId(),
                ProductId = pid,
                UserId = userId,
                Amt = way == "amt" ? product.Price : 0,
                pay_way = way,
                created_time = DateTime.Now
            });
            await _context.SaveChangesAsync();
            //跟新金额缓存
            _authenticationSupport.ReloadUserAmountCache(user.t1_user_attr.Amount);
        }
    }
}
