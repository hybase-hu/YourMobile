using Microsoft.AspNetCore.Mvc;
using YourMobile.DataAccess.Data;

namespace YourMobile.Pages.ViewComponents
{

	[ViewComponent(Name ="ProductType")]
	public class ProductTypeViewComponents:ViewComponent
	{
		private readonly AppDbContext _context;
		public ProductTypeViewComponents(AppDbContext context)
		{
			_context = context;
		}



		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View("Index",_context.ProductTypes.ToList());
		}

	}
}
