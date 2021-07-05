using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wg_core.Domain;

namespace wg_service.Products
{
    public class ProductTypeService
    {
        private readonly ShopContext _context;

        public ProductTypeService(ShopContext context)
        {
            _context = context;
        }

        public async Task<List<t2_product_type>> GetAll()
        {
            var types = await _context.t2_product_types.Where(e => e.Pt_ParentId == null)
                .Include(e => e.InversePt_Parent).AsNoTracking().OrderByDescending(e => e.Sort).ToListAsync();
            return types;
        }
    }
}
