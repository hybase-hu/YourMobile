using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;
using YourMobile.Models.ViewModel;

namespace YourMobile.Pages.Home
{
	[Authorize]
    public class MyOrdersModel : PageModel
    {

		private readonly IProductRepository _productRepo;
		private readonly IProductTypeRepository _productTypeRepo;
		private readonly IPhotoRepository _photoRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IOrderHeaderRepository _orderHeaderRepository;
		private readonly IOrderDetailRepository _orderDetailsRepository;


		[BindProperty]
		public List<OrderDataView> OrderDataList { get; set; }

		public List<OrderHeader> OrderHeaders { get; set; }


		public MyOrdersModel(
			IProductRepository productRepository,
			IProductTypeRepository productType,
			IPhotoRepository photoRepository,
			IOrderDetailRepository orderDetailRepository,
			IWebHostEnvironment webHostEnvironment,
			IOrderHeaderRepository orderHeaderRepository)
		{
			_productRepo = productRepository;
			_productTypeRepo = productType;
			_photoRepository = photoRepository;
			_webHostEnvironment = webHostEnvironment;
			_orderDetailsRepository = orderDetailRepository;
			_orderHeaderRepository = orderHeaderRepository;
		}

		public void OnGet()
        {
			var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
			OrderDataList = new();

			foreach (var orderHeader in _orderHeaderRepository.GetAllOrders(userID))
			{
				var tmp = new OrderDataView
				{
					Header = orderHeader,
					OrderDetailsList = _orderDetailsRepository.GetAllOrderDetail(orderHeader.Id),
					ProductWithPhotoList = new()

				};
				foreach (var product in tmp.OrderDetailsList)
				{
					tmp.ProductWithPhotoList.Add(new ProductWithPhotos
					{
						Product = _productRepo.Get(product.ProductId),
						Photos = _photoRepository.GetProductPhoto(product.ProductId),
					});
				}
				OrderDataList.Add(tmp);
			}


			
		}
    }
}
