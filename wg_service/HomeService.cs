using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using wg_core.Domain;

namespace wg_service
{
    public class HomeService
    {
        private readonly ShopContext _dbContext;

        public HomeService(ShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetData()
        {
            var result = _dbContext.t0_system.FirstOrDefault();
            return result.desc;
        }
    }
}
