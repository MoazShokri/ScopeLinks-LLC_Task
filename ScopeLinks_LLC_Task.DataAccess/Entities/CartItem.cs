﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class CartItem
	{
		public int Id { get; set; }  
		public int ProductId { get; set; }
		public string? ProductName { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
