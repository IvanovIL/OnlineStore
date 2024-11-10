

namespace OnlineStore.AppServices.Products.Models
{
	public sealed class GetProductsRequest
	{ 
		public string ProductName { get; set; }

		public bool IncludeCategory { get; set; }

		public bool IncludeImages { get; set; }
	}
}
