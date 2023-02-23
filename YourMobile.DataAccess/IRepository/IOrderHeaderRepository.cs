using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMobile.Models;

namespace YourMobile.DataAccess.IRepository
{
	public interface IOrderHeaderRepository
	{
		public void AddOrderHeader(OrderHeader orderHeader);
		public void UpdateOrderHeader(OrderHeader orderHeader);
		public void UpdateOrderHeaderStatus(int orderHeaderId,string status);

		public OrderHeader GetOrderHeader(int orderHeaderId);
		public List<OrderHeader> GetAllOrders(string userId);
		public List<OrderHeader> GetAllOrders();
	}
}
