using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models.ViewModel;
using YourMobile.Models;
using YourMobile.DataAccess.Constants;
using Microsoft.AspNetCore.Authorization;
using YourMobile.Utility;

namespace YourMobile.Pages.Admin
{
	[Authorize(Roles = WebRules.AdminRole)]
	public class ManageOrdersModel : PageModel
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


		public ManageOrdersModel(
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
			OrderDataList = new();
			foreach (var orderHeader in _orderHeaderRepository.GetAllOrders())
			{
				OrderDataView orderDataView = new OrderDataView
				{
					Header = _orderHeaderRepository.GetOrderHeader(orderHeader.Id),
					OrderDetailsList = _orderDetailsRepository.GetAllOrderDetail(orderHeader.Id),
					ProductWithPhotoList = new()
				};
				foreach ( var orderDetails in _orderDetailsRepository.GetAllOrderDetail(orderHeader.Id)) {
					orderDataView.ProductWithPhotoList.Add(new ProductWithPhotos
					{
						Photos = _photoRepository.GetProductPhoto(orderDetails.ProductId),
						Product = _productRepo.Get(orderDetails.ProductId)
					});
				}
				OrderDataList.Add(orderDataView);
			}
        }

		public IActionResult OnPostStatusUpdate(string status,int orderHeaderId)
		{
			OrderHeader orderHeader = _orderHeaderRepository.GetOrderHeader(orderHeaderId);
			orderHeader.Status = status;
			_orderHeaderRepository.UpdateOrderHeader(orderHeader);
			Console.WriteLine(status);
			Console.WriteLine(orderHeader.OwnerId);
			return RedirectToPage("/Admin/ManageOrders");
		}
    }
}
