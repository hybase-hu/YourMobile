using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMobile.Models;

namespace YourMobile.DataAccess.IRepository
{
	public interface ICartRepository
	{
		public void AddCart(int ProductId,string owner);


		public void DeleteCarts(List<Cart> cartList);

		public void DeleteProductFromCarts(string userId, int productId);
		public List<Cart> GetUsersCarts(string userId);
	}
}
