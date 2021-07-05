using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wg_core;
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

        public IPagedList<t4_order> Search(string productName, int? userId = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _context.t4_orders.Include(e => e.Product).AsQueryable();
            if (userId.HasValue)
                query = query.Where(e => e.UserId == userId.Value);
            if (!string.IsNullOrEmpty(productName))
                query = query.Where(e => e.Product.ProductName.Contains(productName));

            query = query.OrderByDescending(c => c.order_id);
            var result = new PagedList<t4_order>(query, pageIndex, pageSize);
            return result;
        }

        public async Task<t4_order> GetOrderInfo(int userId, int pid)
        {
            return await _context.t4_orders.FirstOrDefaultAsync(e => e.UserId == userId && e.ProductId == pid);
        }

        public async Task CreateOrder(int userId, string way, int pid)
        {
            var product = await _context.t2_products.FirstOrDefaultAsync(e => e.ProductId == pid);
            if (product == null)
                throw new NotImplementedException("产品不存在！");

            var user = await _context.t1_users.FirstOrDefaultAsync(e => e.UserId == userId);
            if (user == null)
                throw new NotImplementedException("用户不存在！");

            if (await _context.t4_orders.AnyAsync(e => e.UserId == userId && e.ProductId == pid))
                throw new NotImplementedException("该用户已购买/获取资源，无须重复购买/获取");

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
                    if (user.Money < product.Price)
                        throw new NotImplementedException("用户余额不足，请于个人中心充值");

                    //扣钱
                    user.Money = user.Money - product.Price;
                    break;
            }

            _context.t4_orders.Add(new t4_order
            {
                order_guid = SystemUtil.GenerateStringId(),
                ProductId = pid,
                UserId = userId,
                Amt = way == "amt" ? product.Price.Value : product.Invs.Value,
                pay_way = way,
                created_time = DateTime.Now
            });
            await _context.SaveChangesAsync();
            //跟新金额缓存
            _authenticationSupport.ReloadUserAmountCache(user.Money.Value);
        }
    }
}
