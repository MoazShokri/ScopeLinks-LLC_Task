using Microsoft.EntityFrameworkCore;
using ScopeLinks_LLC_Task.DataAccess.Entities;
using ScopeLinks_LLC_Task.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		private readonly ApplicationDbContext _db;

		public OrderRepository(ApplicationDbContext db) : base(db)
		{
			this._db = db;
		}

		

		public async Task SetOrderStatusAsync(int orderId, OrderStatus status)
		{
			var order = await _db.Orders.FindAsync(orderId);
			if (order != null)
			{
				order.Status = status;
				await _db.SaveChangesAsync();
			}
			else
			{
				throw new KeyNotFoundException("Order not found");
			}
		}

		public async Task MarkOrderAsPaidAsync(string userId)
		{
			var order = await _db.Orders
				.Where(o => o.UserId == userId && o.PaymentStatus == PaymentStatus.Pending)
				.FirstOrDefaultAsync();

			if (order != null)
			{
				order.PaymentStatus = PaymentStatus.Paid;

				await _db.SaveChangesAsync();
			}
			else
			{
				throw new InvalidOperationException("No pending order found for the specified user.");
			}
		}
		public IEnumerable<Order> GetPastOrdersByUserId(string userId)
		{
			try
			{
				var pastOrders = _db.Orders
					.Where(o => o.UserId == userId && o.Status != OrderStatus.Pending)
					.ToList();

				return pastOrders;
			}
			catch (Exception ex)
			{
				// Handle exceptions or log errors
				throw new Exception($"Error retrieving past orders: {ex.Message}");
			}
		}
		public async Task<PagedList<Order>> GetOrdersAsync(string userId, OrderStatus status, int page, int pageSize)
		{
			IQueryable<Order> query = _db.Orders
				.Include(o => o.Products) 
				.OrderByDescending(o => o.OrderDate);

			if (!string.IsNullOrEmpty(userId))
			{
				query = query.Where(o => o.UserId == userId);
			}

			if (!string.IsNullOrEmpty(status.ToString()))
			{
				query = query.Where(o => o.Status == status);
			}

			// Calculate the total count before applying pagination
			var totalCount = await query.CountAsync();

			// Apply pagination
			var orders = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return new PagedList<Order>(orders, totalCount, page, pageSize);
		}




	}
}
