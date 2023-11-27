using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Dtos
{
	public class OrderStatusDto
	{
		public int OrderId { get; set; }
		public OrderStatus Status { get; set; }
	}
}
