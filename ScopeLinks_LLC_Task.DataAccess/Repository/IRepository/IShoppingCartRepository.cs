using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository.IRepository
{
	public interface IShoppingCartRepository : IRepository<CartItem>
	{
		List<CartItem> GetCartItems(string userId);
		void AddCartItem(string userId, CartItem item);
		void AddCartItem(string userId, ShoppingCart Cartitem);

		void RemoveCartItem(string userId, int productId);
		Task<decimal> CalculateTotalPrice(string userId);
		Task<ShoppingCart> GetShoppingCartAsync(string userId);
		Task AddToCartAsync(string userId, int productId, int quantity);
		Task RemoveFromCartAsync(string userId, int itemId);
		Task ClearCartAsync(string userId);
		Task<ShoppingCart> GetCartByUserIdAsync(string userId);
	}
}
