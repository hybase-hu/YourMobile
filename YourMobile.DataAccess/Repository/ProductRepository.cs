using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using YourMobile.DataAccess.Data;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;

namespace YourMobile.DataAccess.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _context;

		public ProductRepository(AppDbContext context)
		{
			_context = context;
		}

		public void AddProduct(Product product)
		{
			if (product == null)
			{
				throw new Exception("Required product if AddProduct use");
			}
			_context.Add(product);
			_context.SaveChanges();
		}

		public void DeleteProduct(Product product)
		{
			if (product != null)
			{
				_context.Products.Remove(product);
				_context.SaveChanges();
			}
		}

		public Product Get(int id)
		{
			//var product = _context.Products.FirstOrDefault(x => x.Id == id);
			var product = _context.Products.Find(id);
			if (product == null)
			{
				throw new Exception("Product not found");
			}
			return product;
		}

		public List<Product> GetAll()
		{
			return _context.Products.ToList();
		}

		public List<Product> GetAll(int productTypeId)
		{
			return _context.Products.Where(p=>p.ProductTypeId == productTypeId).ToList();
		}

		public void UpdateProduct(Product product)
		{

			_context.Products.Update(product);
			_context.SaveChanges();
		}
	}
}
