using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class ProductImage
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public int ProductId { get; set; } //3
		public Product Product { get; set; }
	}
}
