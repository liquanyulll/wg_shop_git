using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using wg_frame_work;
using wg_model.Accounts;
using wg_model.Products;
using wg_service.Products;
using wg_service.Users;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_api.Controllers
{
    [Produces("application/json")]
    [Route("api/ProductType")]
    public class ProductTypeController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly AccountService _accountService;
        private readonly ProductTypeService _productTypeService;

        public ProductTypeController(IMapper mapper, AccountService accountService, ProductTypeService productTypeService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _productTypeService = productTypeService;
        }

        [SkipUserFilter]
        [HttpGet("ProductTypes")]
        public async Task<JsonResult> GetProductTypes()
        {
            var types = await _productTypeService.GetAll();
            var typeModels = _mapper.Map<List<ProductTypeModel>>(types);
            return SucessResult(typeModels);
        }
    }
}
