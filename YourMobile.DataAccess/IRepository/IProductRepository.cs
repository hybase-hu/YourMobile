
using YourMobile.Models;

namespace YourMobile.DataAccess.IRepository
{
	public interface IProductRepository
	{
		public List<Product> GetAll();
		public List<Product> GetAll(int productTypeId);
		public void AddProduct(Product product);
		public void UpdateProduct(Product product);
		public void DeleteProduct(Product product);
		public Product Get(int id);
	}
}
