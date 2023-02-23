using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourMobile.Models
{
	public class Cart
	{
		public string Id { get; set; }

		[Required]
		public int Count { get; set; } = 1;

		[Required]
		public int ProductId { get; set; }


		[Required]
		public string ApplicationUserId { get; set; }


	}
}
