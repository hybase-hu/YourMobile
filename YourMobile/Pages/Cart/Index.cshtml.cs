using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models.ViewModel;

namespace YourMobile.Pages.Cart
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ICartRepository _cartRepository;

        public List<ProductWithPhotos> ProductWP { get; set; }

        [BindProperty]
        public List<YourMobile.Models.Cart> CartItems { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IndexModel(
            IPhotoRepository photoRepository,
            IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            ICartRepository cartRepository
            )
        {
            _photoRepository = photoRepository;
            _productTypeRepository = productTypeRepository;
            _productRepository= productRepository;
            _cartRepository= cartRepository;
        }

        public void OnGet(string statusMessage=null)
        {
            if ( statusMessage != null)
            {
                StatusMessage = statusMessage;
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if (_cartRepository.GetUsersCarts(claim.Value).Count == 0)
                {
                    CartItems = null;
                }
                else
                {
                    CartItems = _cartRepository.GetUsersCarts(claim.Value);
                    List<ProductWithPhotos> pwp = new List<ProductWithPhotos>();
                    foreach (var item in CartItems)
                    {
                        pwp.Add(new ProductWithPhotos {
                            Photos = _photoRepository.GetProductPhoto(item.ProductId),
                            Product = _productRepository.Get(item.ProductId)
                        });
                    }
                    ProductWP = pwp;
                }

            }
        }


        public IActionResult OnPostDeleteProductFromCart(int productId)
        {
			var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
			_cartRepository.DeleteProductFromCarts(userID, productId);
            return RedirectToPage("Index");
		}


        public IActionResult OnPost(int productId) { 
            var product = _productRepository.Get(productId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var cart = _cartRepository.GetUsersCarts(claim.Value);
                if (cart != null)
                {
                    _cartRepository.AddCart(productId, claim.Value);
                    
                }
				return RedirectToPage("Index");
			}
        
        }
    }
}
