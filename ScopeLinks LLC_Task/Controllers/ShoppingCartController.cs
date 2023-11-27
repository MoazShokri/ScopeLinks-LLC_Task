using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ScopeLinks_LLC_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]

	public class ShoppingCartController : ControllerBase
	{
		protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAuthService _authServices;


		public ShoppingCartController(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper,
			UserManager<ApplicationUser> userManager)
		{
			this._unitOfWork = unitOfWork;
			this._mapper = mapper;
			this._userManager = userManager;
			this._authServices= authService;
			_response = new();
		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetCartItems()
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{

				var carts = _unitOfWork.ShoppingCart.GetCartItems(userId);

				if (carts == null || !carts.Any())
				{
					// No cart items found
					_response.IsSuccess = false;
					_response.ErrorMessages = new List<string> { "No cart items found." };
					return NotFound(_response);
				}
				// Calculate total price for each cart item
				var cartItems = _mapper.Map<List<CartItemGet>>(carts);
				foreach (var cartItem in cartItems)
				{
					cartItem.Price = await _unitOfWork.ShoppingCart.CalculateTotalPrice(userId);
				}

				_response.Result = cartItems;
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);

				//_response.Result = _mapper.Map<List<CartItemGet>>(carts);
				//_response.StatusCode = HttpStatusCode.OK;
				//return Ok(_response);
			}
			catch (Exception ex)
			{
				// Handle other exceptions
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> AddCartItem([FromForm] CartItemAddDto cartDto)
		{
			var userId =  GetUserIdFromRequestHeader();

			if (string.IsNullOrEmpty(userId))
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new() { "Invalid or missing user ID." };
				_response.IsSuccess = false;
				return BadRequest(_response);
			}

			if (!ModelState.IsValid)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new() { "Model State is Not Valid" };
				_response.IsSuccess = false;
				return BadRequest(_response);
			}

			try
			{
				

				CartItem cart = _mapper.Map<CartItem>(cartDto);
				_response.Result = cart;
				_unitOfWork.ShoppingCart.AddCartItem(userId, cart);
				_unitOfWork.Save();
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("{userId}")]
		public async Task<IActionResult> GetShoppingCart()
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{
				var shoppingCart = await _unitOfWork.ShoppingCart.GetShoppingCartAsync(userId);
				return Ok(shoppingCart);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}

		[HttpPost("add-to-cart")]
		public async Task<IActionResult> AddToCart( [FromForm] AddToCartDto addToCartDto)
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{
				await _unitOfWork.ShoppingCart.AddToCartAsync(userId, addToCartDto.ProductId, addToCartDto.Quantity);
				return Ok(new { Message = "Item added to the cart successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}

		[HttpDelete("remove-from-cart/{itemId}")]
		public async Task<IActionResult> RemoveFromCart( int itemId)
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{
				await _unitOfWork.ShoppingCart.RemoveFromCartAsync(userId, itemId);
				return Ok(new { Message = "Item removed from the cart successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}

		[HttpDelete("clear-cart")]
		public async Task<IActionResult> ClearCart()
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{
				await _unitOfWork.ShoppingCart.ClearCartAsync(userId);
				return Ok(new { Message = "Cart cleared successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}
		public async Task<IActionResult> ClearCartWhenPaid()
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{
				// Mark the user's order as paid (assuming you have a method to update order status)
				await _unitOfWork.Orders.MarkOrderAsPaidAsync(userId);

				// Clear the user's cart
				await _unitOfWork.ShoppingCart.ClearCartAsync(userId);

				return Ok(new { Message = "Cart cleared successfully after payment." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}
		public async Task<ShoppingCart> GetOrCreateCartAsync()
		{
			var userId = GetUserIdFromRequestHeader();

			// Check if the user already has an abandoned cart
			var existingCart = await _unitOfWork.ShoppingCart.GetCartByUserIdAsync(userId);

			if (existingCart != null)
			{
				// User has an abandoned cart, return it
				return existingCart;
			}

			// User doesn't have an abandoned cart, create a new one
			var newCart = new ShoppingCart { UserId = userId, CartItems = new List<CartItem>() };
			 _unitOfWork.ShoppingCart.AddCartItem(userId,newCart);
			 _unitOfWork.Save();

			return newCart;
		}

		private string GetUserIdFromRequestHeader()
		{
			var userid = _authServices.GetUserIdFromRequestHeader(HttpContext);
			return userid;
		}

	}
}
