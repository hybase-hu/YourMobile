using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourMobile.DataAccess.Constants
{
	public static class OrderStatus
	{
		public static string OrderPending = "OrderPending";
		public static string OrderPaymentSuccess = "OrderPaymentSuccess";
		public static string OrderAccept = "OrderAccept";
		public static string OrderDeliveryPrepared = "OrderDeliveryPrepared";
		public static string OrderSuccess = "OrderSuccess";
		public static string OrderCanceled = "OrderCancelled";
		public static string OrderRejects = "OrderRejects";
	}
}
