using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models.ViewModel;

namespace YourMobile.Pages.Home
{
    public class DetailsModel : PageModel
    {
		private readonly IProductRepository _productRepo;
		private readonly IProductTypeRepository _productTypeRepo;
		private readonly IPhotoRepository _photoRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;


		[BindProperty]
		public ProductWithPhotos ProductsWP { get; set; }


		public DetailsModel(
			IProductRepository productRepository,
			IProductTypeRepository productType,
			IPhotoRepository photoRepository,
			IWebHostEnvironment webHostEnvironment)
		{
			_productRepo = productRepository;
			_productTypeRepo = productType;
			_photoRepository = photoRepository;
			_webHostEnvironment = webHostEnvironment;
		}


		public IActionResult OnGet(int productId)
        {
            var product = _productRepo.Get(productId);
			if (product == null)
			{
				return NotFound();
			}

            ProductsWP = new ProductWithPhotos { 
				Product = product,
				Photos = _photoRepository.GetProductPhoto(productId)
			};


			return Page();
        }
    }
}
