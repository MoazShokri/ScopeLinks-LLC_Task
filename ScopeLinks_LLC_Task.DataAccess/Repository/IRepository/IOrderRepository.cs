using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository.IRepository
{
	public interface IOrderRepository : IRepository<Order>
	{
		Task SetOrderStatusAsync(int orderId, OrderStatus status);
		Task MarkOrderAsPaidAsync(string userId);
		IEnumerable<Order> GetPastOrdersByUserId(string userId);
		Task<PagedList<Order>> GetOrdersAsync(string userId, OrderStatus status, int page, int pageSize);
	}
}
