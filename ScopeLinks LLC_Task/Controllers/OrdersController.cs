using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScopeLinks_LLC_Task.DataAccess.Entities;
using ScopeLinks_LLC_Task.EmailServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ScopeLinks_LLC_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAuthService _authServices;
		private readonly IEmailService _emailService;


		public OrdersController(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper,
			UserManager<ApplicationUser> userManager , IEmailService emailService)
		{
			this._unitOfWork = unitOfWork;
			this._mapper = mapper;
			this._userManager = userManager;
			this._authServices = authService;
			this._emailService = emailService;
			_response = new();


		}

		[HttpPost("set-status")]
		public async Task<IActionResult> SetOrderStatus([FromForm] OrderStatusDto orderStatusDto)
		{
			try
			{
				await _unitOfWork.Orders.SetOrderStatusAsync(orderStatusDto.OrderId, orderStatusDto.Status);
				return Ok(new { Message = "Order status updated successfully." });
			}
			catch (KeyNotFoundException)
			{
				return NotFound(new { Message = "Order not found." });
			}
			catch (Exception)
			{
				return StatusCode(500, new { Message = "Error updating order status." });
			}
		}
		[HttpGet("past-orders")]
		public IActionResult GetPastOrders()
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{
				var pastOrders = _unitOfWork.Orders.GetPastOrdersByUserId(userId);
				return Ok(pastOrders);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}
		[HttpGet("get-orders")]
		public async Task<IActionResult> GetOrders( [FromForm] OrderStatus status, [FromForm] int page = 1, [FromForm] int pageSize = 10)
		{
			var userId = GetUserIdFromRequestHeader();

			try
			{
				var orders = await _unitOfWork.Orders.GetOrdersAsync(userId, status, page, pageSize);

				var orderDtos = _mapper.Map<List<OrderGetDto>>(orders.Items);

				var response = new
				{
					Orders = orderDtos,
					orders.TotalCount,
					orders.PageNumber,
					orders.PageSize,
					orders.TotalPages
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "Internal Server Error" });
			}
		}
		[HttpPost("checkout")]
		public async Task<IActionResult> Checkout()
		{
			try
			{
				var userId = GetUserIdFromRequestHeader();

				await _emailService.CheckoutAndSendConfirmationEmail(userId);

				return Ok(new { Message = "Order checked out successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"Error during checkout: {ex.Message}" });
			}
		}

		private string GetUserIdFromRequestHeader()
		{
			var userid = _authServices.GetUserIdFromRequestHeader(HttpContext);
			return userid;
		}


	}
}
