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
	public class OrderHeaderRepository : IOrderHeaderRepository
	{
		private readonly AppDbContext _context;
		public OrderHeaderRepository(AppDbContext appDbContext) { 
			_context = appDbContext;
		}
		public void AddOrderHeader(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Add(orderHeader);
			_context.SaveChanges();
		}

		public List<OrderHeader> GetAllOrders(string userId)
		{
			return _context.OrderHeaders.Where(u=>u.OwnerId== userId).ToList();
		}

		public List<OrderHeader> GetAllOrders()
		{
			return _context.OrderHeaders.ToList();
		}

		public OrderHeader GetOrderHeader(int orderHeaderId)
		{
			var oderHeader = _context.OrderHeaders.Find(orderHeaderId);
			if (oderHeader == null) { throw new Exception("GetOrderHeaderRepo, order header not found"); }
			return oderHeader;
		}

		public void UpdateOrderHeader(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
			_context.SaveChanges();
		}

		public void UpdateOrderHeaderStatus(int orderHeaderId,string status)
		{
			var orderHeader = _context.OrderHeaders.Find(orderHeaderId);
			if (orderHeader == null) { throw new Exception("OrderHeader update. Order header not found: " + orderHeaderId); }

			orderHeader.Status= status;
			_context.SaveChanges();
		}
	}
}
