using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace YourMobile.Models
{

	public class Product
	{


		[Key]
		public int Id { get; set; }

		[Required]
		[Display(Name = "Product Name")]
		public string ProductName { get; set; }
		[Required]
		[Display(Name = "Product Manifacturer")]
		public string Manifacturer { get; set; }

		[Required]
		[Range(10_000, 100_000_000, ErrorMessage = "Minimum value is 10 000 and max value is 100 000 000")]
		[Display(Name = "Product Price")]
		public int Price { get; set; }


		public int SumCount { get; set; } = 100;


		[Required]
		public int ProductTypeId { get; set; }

		
		[ForeignKey("ProductTypeId")]
		[Display(Name = "Product Type")]
		public ProductType ProductType { get; set; }



		[Display(Name = "Product Description")]
		public string Description { get; set; }


	}
}
