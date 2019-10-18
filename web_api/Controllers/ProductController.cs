using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using wg_frame_work;
using wg_model;
using wg_model.Accounts;
using wg_model.Products;
using wg_service.Products;
using wg_service.Users;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_api.Controllers
{
    [Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly AccountService _accountService;
        private readonly ProductService _productService;

        public ProductController(IMapper mapper, AccountService accountService, ProductService productService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _productService = productService;
        }

        [SkipUserFilter]
        [HttpPost("Search")]
        public async Task<JsonResult> Search([FromBody]ProductQueryModel model)
        {
            try
            {
                throw new Exception("123123123");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                var cc = 123;
                var ccdd = 123;
            }

            var products = _productService.Search(model.TypeId, model.PName, model.PageIndex.Value, model.PageSize.Value);
            var productModels = _mapper.Map<List<ProductListModel>>(products.ToList());
            var result = new BaseListResultModel()
            {
                TotalPages = products.TotalPages,
                TotalItems = products.TotalCount,
                CurrentPage = products.PageIndex,
                ItemsPerPage = products.PageSize,
                ContentList = productModels
            };
            return SucessResult(result);
        }
    }
}
