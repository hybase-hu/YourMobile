using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;
using YourMobile.Models.ViewModel;

namespace YourMobile.Pages.Home
{
    public class IndexModel : PageModel
    {

		private readonly IProductRepository _productRepo;
		private readonly IProductTypeRepository _productTypeRepo;
		private readonly IPhotoRepository _photoRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;


		[BindProperty]
		public List<ProductWithPhotos> ProductsWithPhotosList { get; set; }
		

		public IndexModel(
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
		public void OnGet(int productTypeId = 0 )
        {
			List<Product> products = new List<Product>();
			if (productTypeId != 0)
			{
				products = _productRepo.GetAll(productTypeId);
			}
			else
			{
				products = _productRepo.GetAll();
			}
			
			List<ProductWithPhotos> tmp = new List<ProductWithPhotos>();
			List<Photo> noPhoto= new List<Photo>();
			

			
			foreach (var product in products)
			{
				List<Photo> photos = _photoRepository.GetProductPhoto(product.Id);
				if (photos.Count == 0)
				{
					noPhoto.Add(new Photo { imageUrl = "img/page/no-image.png", ProductId = product.Id });

					tmp.Add(new ProductWithPhotos { Photos = noPhoto, Product = product });
				}
				else
				{
					tmp.Add(new ProductWithPhotos { Photos = photos, Product = product });
				}
				
			}
			ProductsWithPhotosList = tmp;
        }
    }
}
