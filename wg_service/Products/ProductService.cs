using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wg_core;
using wg_core.Domain;

namespace wg_service.Products
{
    public class ProductService
    {
        private readonly ShopContext _context;

        public ProductService(ShopContext context)
        {
            _context = context;
        }

        public IPagedList<t2_product> Search(string typeAll = null, int? typeId = null, string pName = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _context.t2_products.AsQueryable();
            if (typeId.HasValue)
            {
                //if (typeAll == "Y")
                //{
                //    var typeIds = _context.t2_product_types.Where(e => e.Pt_ParentId == typeId.Value).Select(e => e.Pt_Id).ToList();
                //    typeIds.Add(typeId.Value);
                //    query = query.Where(e => typeIds.Contains(e.Pt_Id));
                //}
                //else
                //{
                //    query = query.Where(e => e.Pt_Id == typeId.Value);
                //}
                //假如类型有,1.水果,2.西瓜  3.种植,4.种子,5.西瓜子
                //假如只是水果:tp1
                //假如是西瓜，且搜水果搜得到,那么就是,tp1,tp2
                //假如是西瓜子...总之要哪个标签能搜到，就加哪个类型

                var tpId = "tp" + typeId;
                query = query.Where(e => e.PtIds.Contains(tpId));
            }
            if (!string.IsNullOrEmpty(pName))
                query = query.Where(e => e.ProductName.Contains(pName));

            query = query.OrderByDescending(c => c.CreatedTime);
            var result = new PagedList<t2_product>(query, pageIndex, pageSize);
            return result;
        }

        public async Task<t2_product> Get(int id)
        {
            //
            return await _context.t2_products.Include(e => e.t2_product_specs).Include(e => e.t2_product_detail_Imgs).FirstOrDefaultAsync(e => e.ProductId == id);
        }
    }
}
