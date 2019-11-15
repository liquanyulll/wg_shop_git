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
            var query = _context.t2_product.AsQueryable();
            if (typeId.HasValue)
            {
                if (typeAll == "Y")
                {
                    var typeIds = _context.t2_product_type.Where(e => e.Pt_ParentId == typeId.Value).Select(e => e.Pt_Id).ToList();
                    typeIds.Add(typeId.Value);
                    query = query.Where(e => typeIds.Contains(e.Pt_Id));
                }
                else
                {
                    query = query.Where(e => e.Pt_Id == typeId.Value);
                }
            }
            if (!string.IsNullOrEmpty(pName))
                query = query.Where(e => e.ProductName.Contains(pName));

            query = query.OrderByDescending(c => c.Pt_Id);
            var result = new PagedList<t2_product>(query, pageIndex, pageSize);
            return result;
        }

        public async Task<t2_product> Get(int id)
        {
            return await _context.t2_product.Include(e => e.t2_product_spec).Include(e => e.t2_product_detail_Img).FirstOrDefaultAsync(e => e.ProductId == id);
        }
    }
}
