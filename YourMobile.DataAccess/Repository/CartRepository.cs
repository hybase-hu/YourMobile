using Microsoft.EntityFrameworkCore;
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
	public class CartRepository : ICartRepository
	{
		private readonly AppDbContext _context;
		public CartRepository(AppDbContext appDbContext) {
			_context= appDbContext;
		}


		public void AddCart(int productId,string userId)
		{
			
			var product = _context.Products.Where(u=>u.Id== productId).FirstOrDefault();
			if (product == null)
			{
				throw new Exception("Add Production not valid, product id not found: " + productId);
			}
			else
			{

				_context.Carts.Add(
					new Cart { 
						ApplicationUserId= userId ,
						ProductId=product.Id,
						Id=Guid.NewGuid().ToString()});
				_context.SaveChanges();
			}
		}



		public void DeleteCarts(List<Cart> cartList)
		{
			foreach(var cart in cartList)
			{
				_context.Carts.Remove(cart);

			}
			_context.SaveChanges();
		}

		public void DeleteProductFromCarts(string userId, int productId)
		{

			//lehetséges, hogy több "ugyanaz" a termék van felvéve a kosárba
			//csak az elsőt törli. De mivel minden mennyiség 1, így kb mindegy
			var cartsFromDb = _context.Carts.Where(u=>u.ApplicationUserId == userId && u.ProductId ==productId).FirstOrDefault();
			if (cartsFromDb == null)
			{
				throw new Exception("DeleteProductFromCarts not found product id: " + productId + " uid:" + userId);
			}
			_context.Carts.Remove(cartsFromDb);
			_context.SaveChanges();
		}

		public List<Cart> GetUsersCarts(string userId)
		{
			return _context.Carts.Where(u => u.ApplicationUserId == userId).ToList();
		}
	}
}
