using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMobile.DataAccess.Data;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;

namespace YourMobile.DataAccess.Repository
{
	public class OrderDetailRepository : IOrderDetailRepository
	{
		private readonly AppDbContext _context;

		public OrderDetailRepository(AppDbContext context)
		{
			_context = context;
		}

		public void AddOrderDetail(OrderDetail orderDetail)
		{
			_context.OrderDetails.Add(orderDetail);
			_context.SaveChanges();
		}

		public List<OrderDetail> GetAllOrderDetail(int orderId)
		{
			return _context.OrderDetails.Where(u=>u.OrderId == orderId).ToList();
		}

		public List<OrderDetail> GetAllOrderDetail()
		{
			return _context.OrderDetails.ToList();
		}
	}
}
