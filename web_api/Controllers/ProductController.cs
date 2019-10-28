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
using wg_service.Orders;
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
        private readonly AuthenticationSupport _authenticationSupport;
        private readonly OrderService _orderService;
        private readonly InvitationService _invitationService;

        public ProductController(IMapper mapper,
            AccountService accountService,
            ProductService productService,
            AuthenticationSupport authenticationSupport,
            OrderService orderService,
            InvitationService invitationService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _productService = productService;
            _authenticationSupport = authenticationSupport;
            _orderService = orderService;
            _invitationService = invitationService;
        }

        [SkipUserFilter]
        [HttpPost("Search")]
        public async Task<JsonResult> Search([FromBody]ProductQueryModel model)
        {
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

        [SkipUserFilter]
        [HttpPost("GetById")]
        public async Task<JsonResult> GetById(int pid)
        {
            var user = _authenticationSupport.CurrentUser;
            var product = await _productService.Get(pid);
            var model = _mapper.Map<ProductModel>(product);
            var detailImgs = _mapper.Map<List<ProductDetailImgModel>>(product.t2_product_detail_Img.ToList());
            model.DetailImgs = detailImgs;

            if (user != null)
            {
                var invInfo = await _invitationService.GetInvInfo(user.UserId, pid);
                if (invInfo != null)
                {
                    var invModel = _mapper.Map<InvitationModel>(invInfo);
                    model.InvInfo = invModel;
                }

                var order = await _orderService.GetOrderInfo(user.UserId, pid);
                if (order != null)
                {
                    model.IsBuy = true;
                    model.BuyWay = order.pay_way;
                    //如果是已购买 就取资源地址
                    model.ProductSources = product.t2_product_spec.Where(e => e.Key.Contains("source")).OrderBy(e => e.config_id).Select(e => e.Value).ToList();
                }

            }

            return SucessResult(model);
        }

        [HttpPost("buyProduct")]
        public async Task<JsonResult> BuyProduct(string way, int pid)
        {
            var user = _authenticationSupport.CurrentUser;

            if (way != "inv" && way != "amt")
                throw new NotImplementedException("错误的支付方式！");

            if (way == "amt")
            {
                if (!user.Amount.HasValue || user.Amount.Value <= 0)
                    throw new NotImplementedException("用户余额不足，请于个人中心充值");
            }

            await _orderService.CreateOrder(user.UserId, way, pid);

            return Sucess("购买成功");
        }
    }
}
