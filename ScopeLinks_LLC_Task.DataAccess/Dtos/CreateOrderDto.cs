using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Dtos
{
	public class CreateOrderDto
	{
		public List<Product> Products { get; set; } 
		public decimal TotalPrice { get; set; }

	}
}
