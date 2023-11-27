using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public List<Product> Products { get; set; } = new List<Product>();
		public OrderStatus Status { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public DateTime OrderDate { get; set; }
		public PaymentStatus PaymentStatus { get; set; }



	}
}
