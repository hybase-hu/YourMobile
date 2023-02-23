using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourMobile.Models
{
	public class Photo
	{
		[Key]
		public int Id { get; set; }
		

		[Required]
		public int ProductId { get; set; }

		[Required]
		public string imageUrl { get; set; }
	}
}
