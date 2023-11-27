namespace ScopeLinks_LLC_Task.EmailServices
{
	public interface IEmailService
	{
		Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
		Task CheckoutAndSendConfirmationEmail(string userId);

	}
}
