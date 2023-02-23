using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourMobile.Models
{
	public class OrderHeader
	{
		[Key]
		public int Id { get; set; }
		
		
		[Required]
		public string OwnerId { get; set; }

		public DateTime CreatedDate { get; set; }
		public string Status { get; set; }

		public string? Comment { get; set; }

		public string? TransactionId { get; set; }
		public string? SessionId { get; set; }


		[Display(Name = "Phone Number (+36/70 538 85 07)")]
		public string PhoneNumber { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string Country { get; set; }

		[Required]
		public string Street { get; set; }

		[Required]
		public int PostCode { get; set; }

		[Required]
		public string HouseNumber { get; set; }

		public int TotalPrice { get; set; }

	}
}
