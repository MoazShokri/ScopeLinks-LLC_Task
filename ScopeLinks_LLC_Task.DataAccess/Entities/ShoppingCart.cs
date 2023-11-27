using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class ShoppingCart
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public List<CartItem> CartItems { get; set; }
		public CartStatus Status { get; set; } 
	}
}
