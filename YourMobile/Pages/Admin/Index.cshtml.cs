using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;
using YourMobile.Utility;

namespace YourMobile.Pages.Admin
{
	[Authorize(Roles = WebRules.AdminRole)]
	public class IndexModel : PageModel
    {


		private readonly IProductRepository _productRepo;

		private readonly IPhotoRepository _photoRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;

		[BindProperty]
		public Product Product { get; set; }



		[BindProperty]
		public IEnumerable<Product> ProductList { get; set; }


		public IndexModel(
			IProductRepository repository,
			IPhotoRepository photoRepository,
			IWebHostEnvironment webHostEnvironment)
		{
			_productRepo = repository;
			_photoRepository = photoRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		public void OnGet()
        {

			ProductList = _productRepo.GetAll();

        }

		public IActionResult OnPostDelete(int productId)
		{
			var product = _productRepo.Get(productId);
			if (product == null)
			{
				return NotFound();
			}
			_productRepo.DeleteProduct(product);
			var photos = _photoRepository.GetProductPhoto(productId);
			foreach (var photo in photos)
			{
				var imgPath = Path.Combine(_webHostEnvironment.WebRootPath, photo.imageUrl);
				if (System.IO.File.Exists(imgPath))
				{
					Console.WriteLine("DEleete photo");
					System.IO.File.Delete(imgPath);
				}
				else
				{
					Console.WriteLine("Not found" + imgPath);
				}
				
			}
			_photoRepository.DeleteAllPhoto(productId);
			

			return RedirectToPage("/Admin/Index");
		}
    }
}
