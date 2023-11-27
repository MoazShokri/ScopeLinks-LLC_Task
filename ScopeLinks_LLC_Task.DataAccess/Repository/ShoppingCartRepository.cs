using Microsoft.EntityFrameworkCore;
using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class ShoppingCartRepository : Repository<CartItem>, IShoppingCartRepository
	{
		private readonly ApplicationDbContext _db;

		public ShoppingCartRepository(ApplicationDbContext db) : base(db)
		{
			this._db = db;
		}

	
      
	        public List<CartItem> GetCartItems(string userId)
	        {
	        	return _db.CartItems
	        		.Where(item => item.UserId == userId)
	        		.ToList();
	        }
   	        
	        public void AddCartItem(string userId, CartItem item)
	        {
	        	item.UserId = userId; // Set the user ID before adding to the database
	        	_db.CartItems.Add(item);
	        	_db.SaveChanges();
	        }
   	        
	        public void RemoveCartItem(string userId, int productId)
	        {
	        	var itemToRemove = _db.CartItems
	        		.FirstOrDefault(item => item.UserId == userId && item.ProductId == productId);
   	        
	        	if (itemToRemove != null)
	        	{
	        		_db.CartItems.Remove(itemToRemove);
	        		_db.SaveChanges();
	        	}
 	        }

		   public async Task<decimal> CalculateTotalPrice(string userId)
		   {
		   	return await _db.CartItems
		   		.Where(cartItem => cartItem.UserId == userId)
		   		.SumAsync(cartItem => cartItem.Price * cartItem.Quantity);
		   }
		   public async Task<ShoppingCart> GetShoppingCartAsync(string userId)
		   {
		   	var cartItems = await _db.CartItems
		   		.Where(ci => ci.UserId == userId)
		   		.Include(ci => ci.ProductId)
		   		.ToListAsync();
		   
		   	return new ShoppingCart
		   	{
		   		UserId = userId,
		   		CartItems = cartItems
		   	};
		   }
		

		public async Task AddToCartAsync(string userId, int productId, int quantity)
		   {
		   	var existingCartItem = await _db.CartItems
		   		.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
		   
		   	if (existingCartItem != null)
		   	{
		   		existingCartItem.Quantity += quantity;
		   	}
		   	else
		   	{
		   		var newCartItem = new CartItem
		   		{
		   			UserId = userId,
		   			ProductId = productId,
		   			Quantity = quantity
		   		};
		   		_db.CartItems.Add(newCartItem);
		   	}
		   
		   	await _db.SaveChangesAsync();
		   }
		   
		   public async Task RemoveFromCartAsync(string userId, int itemId)
		   {
		   	var cartItemToRemove = await _db.CartItems
		   		.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.Id == itemId);
		   
		   	if (cartItemToRemove != null)
		   	{
		   		_db.CartItems.Remove(cartItemToRemove);
		   		await _db.SaveChangesAsync();
		   	}
		   }
		
		public async Task ClearCartAsync(string userId)
		   {
		   	var cartItems = await _db.CartItems
		   		.Where(ci => ci.UserId == userId)
		   		.ToListAsync();
		   
		   	_db.CartItems.RemoveRange(cartItems);
		   	await _db.SaveChangesAsync();
		 }
		public async Task<ShoppingCart> GetCartByUserIdAsync(string userId)
		{
			var existingAbandonedCart = await _db.shoppingCarts
				.Include(sc => sc.CartItems) 
				.FirstOrDefaultAsync(sc => sc.UserId == userId && sc.Status == CartStatus.Abandoned);

			if (existingAbandonedCart != null)
			{
				return existingAbandonedCart;
			}
			else
			{
				// If no abandoned cart found, create a new one
				var newCart = new ShoppingCart
				{
					UserId = userId,
					Status = CartStatus.Active, 
				};

				await _db.shoppingCarts.AddAsync(newCart);
				await _db.SaveChangesAsync();

				return newCart;
			}
		}
		public void AddCartItem(string userId, ShoppingCart Cartitem)
		{
			Cartitem.UserId = userId; // Set the user ID before adding to the database
			_db.shoppingCarts.Add(Cartitem);
			_db.SaveChanges();
		}





	}
}
