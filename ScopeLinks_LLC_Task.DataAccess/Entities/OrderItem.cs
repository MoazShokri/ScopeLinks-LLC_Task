using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class OrderItem
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public string PaymentIntentId { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }

	}
}
