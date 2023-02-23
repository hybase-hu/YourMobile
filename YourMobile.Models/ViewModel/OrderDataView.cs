using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourMobile.Models.ViewModel
{
	public class OrderDataView
	{

		public List<ProductWithPhotos> ProductWithPhotoList { get; set; }
		public OrderHeader Header { get; set; }
		public List<OrderDetail> OrderDetailsList { get; set; }

	}
}
