using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Stripe.Checkout;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;

namespace YourMobile.Pages.Order
{
    public class SuccessModel : PageModel
    {

        private readonly IOrderHeaderRepository _orderHeaderRepository;
		private readonly IOrderDetailRepository _orderDetailRepository;
		private readonly ICartRepository _cartRepository;
		private readonly IProductRepository _productRepository;
		[BindProperty]
        public OrderHeader OrderHeader { get; set; }


        public SuccessModel(
            IOrderHeaderRepository orderHeaderRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductRepository productRepository
            )
        {
            //ezek azért kellenek most, mert a rendelés "details"-eit végigjárva
            //a termékek mennyiésgét csökkenteni akarjuk annyival, amennyit rendeltek
            _orderHeaderRepository = orderHeaderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
        }

        public void OnGet(int id)
        {
            var oderHeader = _orderHeaderRepository.GetOrderHeader(id);
            OrderHeader = oderHeader;
            SessionService service = new SessionService();
            Session session = service.Get(OrderHeader.SessionId);
            if(session.PaymentStatus.ToLower() == "paid")
            {
                OrderHeader.Status = YourMobile.DataAccess.Constants.OrderStatus.OrderPaymentSuccess;
                OrderHeader.TransactionId = session.PaymentIntentId;
                _orderHeaderRepository.UpdateOrderHeader(OrderHeader);
            }

            var orderDetails = _orderDetailRepository.GetAllOrderDetail(OrderHeader.Id);
            foreach (var obj in orderDetails)
            {
                var product = _productRepository.Get(obj.ProductId);
                product.SumCount -= 1; //mivel csak egy termek rendelheto egyszerre,
                                      //így nem foglalkozunk a rendelés mennyiséggel most.
                _productRepository.UpdateProduct(product);
            }


        }
    }
}
