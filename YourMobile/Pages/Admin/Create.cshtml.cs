using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;
using YourMobile.Utility;

namespace YourMobile.Pages.Admin
{
	[Authorize(Roles=WebRules.AdminRole)]
	public class CreateModel : PageModel
	{

		private readonly IProductRepository _productRepo;
		private readonly IProductTypeRepository _productTypeRepo;
		private readonly IPhotoRepository _photoRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;

		[BindProperty]
		public Product Product { get; set; }

		//public List<string> FilesNames { get; set; }

		[BindProperty]
		public IEnumerable<SelectListItem> ProductTypeList { get; set; }
		public IFormFile? FormFile { get; set; }

		public CreateModel(
			IProductRepository repository,
			IProductTypeRepository productType,
			IPhotoRepository photoRepository,
			IWebHostEnvironment webHostEnvironment)
		{
			_productRepo = repository;
			_productTypeRepo = productType;
			_photoRepository = photoRepository;
			_webHostEnvironment = webHostEnvironment;
		}
		public void OnGet(int productId = 0)
		{

			if (productId != 0)
			{
				Product = _productRepo.Get(productId);
			}
			else
			{
				Product = new();
			}
			
			ProductTypeList = _productTypeRepo.GetTypes().Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Id.ToString()
			});
			
			
			
		}


		public async Task<IActionResult> OnPost()
		{

			string webRootPath = _webHostEnvironment.WebRootPath;
			Console.WriteLine("****************"+HttpContext.Request.Form.Files);
			var files = HttpContext.Request.Form.Files;
			Console.WriteLine("files is " + files.Count);

			if (Product.Id == 0)
			{
				_productRepo.AddProduct(Product);
			}
			else
			{
				_productRepo.UpdateProduct(Product);
			}

			

			//photo => photoRepo => db
			if (files.Count > 0 )
			{

				if (Product.Id != 0)
				{
					_photoRepository.DeleteAllPhoto(Product.Id);

					//delete image from server directory
					List<Photo> photos = _photoRepository.GetProductPhoto(Product.Id);
					foreach (Photo photo in photos)
					{
						string tmpPath = Path.Combine(webRootPath, @"img\productsImg\", photo.imageUrl);
						if (System.IO.File.Exists(tmpPath)) { System.IO.File.Delete(tmpPath); }
					}

				}


				foreach (var file in files)
				{
					Console.WriteLine("fileupload " + file.FileName);
					string fileName = Product.ProductName.Replace(' ', '_')
						.Replace('"','_')
						.Replace('\'','_') + Guid.NewGuid().ToString()[1..8];

					var uploadsPath = Path.Combine(webRootPath, @"img\productsImg");
					var extension = file.FileName.Split('.')[1];
					fileName = fileName + "." + extension;
					using (var fileStream = new FileStream(
						Path.Combine(uploadsPath, fileName),
						FileMode.Create)
						)
					{
						file.CopyTo(fileStream);
						fileStream.Close();
						
						_photoRepository.AddPhoto(Product.Id, @"img\productsImg\" + fileName);
					}
				}
				
			}


			return RedirectToPage("/Admin/Index");
		
		}
	}
}
