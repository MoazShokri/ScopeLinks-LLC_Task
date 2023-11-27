using Microsoft.AspNetCore.Http;

namespace ScopeLinks_LLC_Task.DataAccess.Dtos
{
	public class ProductUpdateDto
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
		public List<IFormFile> Images { get; set; }
	}
}
