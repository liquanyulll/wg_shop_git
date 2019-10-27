using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using wg_core.Domain;
using wg_frame_work;
using wg_model;
using wg_model.Accounts;
using wg_model.Orders;
using wg_service.Orders;
using wg_service.Users;
using wg_utils;

namespace web_api.Controllers
{
	[Produces("application/json")]
	[Route("api/Order")]
	public class OrderController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly AccountService _accountService;
		private readonly AuthenticationSupport _authenticationSupport;
		private readonly OrderService _orderService;
		private readonly InvitationService _invitationService;

		public OrderController(IMapper mapper,
			AccountService accountService,
			AuthenticationSupport authenticationSupport,
			OrderService orderService,
			InvitationService invitationService)
		{
			_mapper = mapper;
			_accountService = accountService;
			_authenticationSupport = authenticationSupport;
			_orderService = orderService;
			_invitationService = invitationService;
		}

		[HttpPost("Search")]
		public async Task<JsonResult> Search([FromBody]OrderQueryModel model)
		{
			var user = _authenticationSupport.CurrentUser;
			var orders = _orderService.Search(model.ProductName, user.UserId, model.PageIndex.Value, model.PageSize.Value);
			var orderModels = _mapper.Map<List<OrderModel>>(orders.ToList());
			var result = new BaseListResultModel()
			{
				TotalPages = orders.TotalPages,
				TotalItems = orders.TotalCount,
				CurrentPage = orders.PageIndex,
				ItemsPerPage = orders.PageSize,
				ContentList = orderModels
			};
			return SucessResult(result);
		}
	}
}