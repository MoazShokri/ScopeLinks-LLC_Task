using System.Net.Mail;

namespace ScopeLinks_LLC_Task.EmailServices
{
	public class EmailService : IEmailService
	{
		private readonly IUnitOfWork _unitOfWork;

		public EmailService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
		{
			// Example using SmtpClient:
			var message = new MailMessage
			{
				From = new MailAddress(""),
				Subject = subject,
				Body = body,
				IsBodyHtml = isHtml,
			};

			message.To.Add(to);

			using (var client = new SmtpClient())
			{
				// Configure the client if needed
				await client.SendMailAsync(message);
			}
		}
		public async Task CheckoutAndSendConfirmationEmail(string userId)
		{
			try
			{
				// Retrieve the user's shopping cart
				var shoppingCart = await _unitOfWork.ShoppingCart.GetShoppingCartAsync(userId);

				// Create an order based on the user's shopping cart
				var order = CreateOrderFromCart(userId, shoppingCart);

				// Save the order to the database
				await _unitOfWork.Orders.AddAsync(order);
				 _unitOfWork.Save();

				// Send order confirmation email to the user
				await SendOrderConfirmationEmail(order);

				// Clear the user's shopping cart
				await _unitOfWork.ShoppingCart.ClearCartAsync(userId);
			}
			catch (Exception ex)
			{
				// Handle exceptions appropriately (log, notify, etc.)
				Console.WriteLine($"Error during checkout: {ex.Message}");
				throw;
			}
		}

		private Order CreateOrderFromCart(string userId, ShoppingCart shoppingCart)
		{
			
			return new Order
			{
				UserId = userId,
				Status = OrderStatus.Pending ,
				PaymentStatus= PaymentStatus.Pending ,
				OrderDate = DateTime.Now,
			};
		}

		private async Task SendOrderConfirmationEmail(Order order)
		{
			var emailContent = GenerateOrderConfirmationEmailContent(order);
			await SendEmailAsync(order.UserId, "Order Confirmation", emailContent);
		}

		private string GenerateOrderConfirmationEmailContent(Order order)
		{
			
			return $"<p>Dear {order.UserId},<br />Your order has been confirmed. Thank you for shopping with us!</p>";
		}
	}
}
