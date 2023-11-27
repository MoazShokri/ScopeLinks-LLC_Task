using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Dtos
{
	public class ProductAddDto
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
		public List<IFormFile> Images { get; set; }
	}
}
