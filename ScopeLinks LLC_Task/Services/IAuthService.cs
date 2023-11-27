namespace ScopeLinks_LLC_Task.Services
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(RegisterModel model);
		Task<AuthModel> GetTokenAsync(TokenRequestModel model);
		Task<string> AddRoleAsync(AddRoleModel model);
		string GetUserIdFromRequestHeader(HttpContext httpContext);
		string GetUserIdFromToken(HttpContext httpContext, out string CustomerId);


	}
}
