using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMobile.Models;

namespace YourMobile.DataAccess.IRepository
{
	public interface IProductTypeRepository
	{
		List<ProductType> GetTypes();
		void AddProductType(ProductType productType);
		void UpdateProductType(ProductType productType);
		void DeleteProductType(ProductType productType);
	}
}
