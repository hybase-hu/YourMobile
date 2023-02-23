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
	public class ProductTypeRepository : IProductTypeRepository
	{
		private readonly AppDbContext _db;
		public ProductTypeRepository(AppDbContext db) { 
			_db= db;
		}

		public void AddProductType(ProductType productType)
		{
			_db.Add<ProductType>(productType);
			_db.SaveChanges();
		}

		public void DeleteProductType(ProductType productType)
		{
			_db.Remove<ProductType>(productType);
			_db.SaveChanges();
		}

		public List<ProductType> GetTypes()
		{
			return _db.ProductTypes.ToList();
		}

		public void UpdateProductType(ProductType productType)
		{
			throw new NotImplementedException();
		}
	}
}
