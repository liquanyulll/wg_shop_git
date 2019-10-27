using System;
using System.Collections.Generic;
using System.Text;

namespace wg_model.Orders
{
	public class OrderModel
	{
		public int order_id { get; set; }
		public string order_guid { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int UserId { get; set; }
		public decimal Amt { get; set; }
		public string pay_way { get; set; }
		public DateTime created_time { get; set; }
	}
}
