using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMobile.Models;

namespace YourMobile.DataAccess.IRepository
{
	public interface IOrderDetailRepository
	{
		public void AddOrderDetail(OrderDetail orderDetail);
		public List<OrderDetail> GetAllOrderDetail(int orderId);
		public List<OrderDetail> GetAllOrderDetail();
		
	}
}
