using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using YourMobile.DataAccess.Constants;
using System.Security.Claims;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;
using Stripe.Checkout;

namespace YourMobile.Pages.Order
{

	[Authorize]
    public class IndexModel : PageModel
    {

		private readonly IOrderDetailRepository _orderDetailRepository;
		private readonly IOrderHeaderRepository _orderHeaderRepository;
		private readonly ICartRepository _cartRepository;
		private readonly IProductRepository _productRepository;

		[BindProperty]
		public List<YourMobile.Models.Cart> Carts { get; set; }

		[BindProperty]
		public OrderHeader OrderHeader { get; set; }
		[BindProperty]
		public int TotalPrice { get; set; }
		[BindProperty]
		public int TotalPriceWithVat { get; set; }

		[BindProperty]
		public int TotalPriceWithVatAndShipping { get; set; }

		[TempData]
		public string StatusMessage { get; set; }


		public IndexModel(
			IOrderHeaderRepository orderHeaderRepository,
			ICartRepository cartRepository,
			IOrderDetailRepository orderDetailRepository,
			IProductRepository productRepository
			)
		{
			_orderDetailRepository = orderDetailRepository;
			_orderHeaderRepository = orderHeaderRepository;
			_cartRepository = cartRepository;
			_productRepository = productRepository;
		}
		public IActionResult OnGet()
        {
			var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Carts = _cartRepository.GetUsersCarts(userID);
			if (Carts == null )
			{
				return NotFound();
			}

			foreach(var cart in Carts )
			{
				var product = _productRepository.Get(cart.ProductId);
				TotalPrice += product.Price;
				if (product.SumCount < 1)
				{
					StatusMessage = "Sorry, this item is not available more - " + product.ProductName;
					return RedirectToPage("/Cart/Index",StatusMessage=StatusMessage);
				}
			}
			TotalPriceWithVat = (int)(TotalPrice * YourMobile.DataAccess.Constants.Price.VAT);
			TotalPriceWithVatAndShipping = TotalPriceWithVat + YourMobile.DataAccess.Constants.Price.ShippingFee;
			OrderHeader = new OrderHeader {
				TotalPrice = TotalPriceWithVatAndShipping,
				OwnerId = userID,
			};



			return Page();

        }

		public IActionResult OnPostSave()
		{


			var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Carts = _cartRepository.GetUsersCarts(userID);
			OrderHeader.OwnerId = userID;
			OrderHeader.Status = OrderStatus.OrderPending;
			//ár számítás
			foreach (var cart in Carts)
			{
				var product = _productRepository.Get(cart.ProductId);

				if(product.SumCount < 1)
				{
					StatusMessage = "Sorry, this item is not available more - " + product.ProductName;
					return Page();
				}

				TotalPrice += product.Price;
			}
			TotalPriceWithVat = (int)(TotalPrice * YourMobile.DataAccess.Constants.Price.VAT);
			TotalPriceWithVatAndShipping = TotalPriceWithVat + YourMobile.DataAccess.Constants.Price.ShippingFee;
			OrderHeader.TotalPrice = TotalPriceWithVatAndShipping;

			OrderHeader.CreatedDate = DateTime.Now;

			_orderHeaderRepository.AddOrderHeader(OrderHeader);
			foreach (var cart in Carts)
			{
				_orderDetailRepository.AddOrderDetail(new OrderDetail
				{
					OrderId = OrderHeader.Id,
					ProductId = cart.ProductId,


				});
			}


			//card payment

			var domain = "https://localhost:7072";
			var options = new SessionCreateOptions
			{
				LineItems = new List<SessionLineItemOptions>(),
				PaymentMethodTypes = new List<string> { "card", },
				Mode = "payment",
				SuccessUrl = domain + $"/Order/Success?id={OrderHeader.Id}",
				CancelUrl = domain + "/Order/Index",
				ShippingOptions = new List<SessionShippingOptionOptions>
				{
					new SessionShippingOptionOptions
					{
						ShippingRateData = new SessionShippingOptionShippingRateDataOptions
						{
							Type = "fixed_amount",
							FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
							{
								Amount = Price.ShippingFee * 100,
								Currency = "HUF",
							},
							DisplayName = "Free shipping",
							DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
							{
								Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
								{
									Unit = "business_day",
									Value = 2,
								},
								Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
								{
									Unit = "business_day",
									Value = 7,
								},
							},
						},
					}
				}

			};

			foreach (var cart in Carts)
			{
				var product = _productRepository.Get(cart.ProductId);
				var sessionListItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{

						UnitAmount = (long)(product.Price * Price.VAT) * 100,
						Currency = "HUF",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = product.ProductName,
							Description = product.Description
						}
					},
					Quantity = 1
				};
				options.LineItems.Add(sessionListItem);
				
			}

			var service = new SessionService();
			Session session = service.Create(options);
			Response.Headers.Add("Location", session.Url);
			OrderHeader.SessionId = session.Id;
			//OrderHeader.TransactionId = session.PaymentIntentId;
			//ezt itt még nem kapjuk meg, majd a Success oldalon
			//OrderHeader.Status = OrderStatus.OrderPaymentSuccess;
			

			_orderHeaderRepository.UpdateOrderHeader(OrderHeader);
			_cartRepository.DeleteCarts(Carts);

			return new StatusCodeResult(303);



			
		}//onPostSave

    }
}
