using System.ComponentModel.DataAnnotations;

namespace ScopeLinks_LLC_Task.Services.Models
{
	public class RegisterModel
	{
		[Required, StringLength(100)]
		public string? FristName { get; set; }
		[Required, StringLength(100)]
		public string? LastName { get; set; }
		[Required, StringLength(100)]
		public string? UserName { get; set; }

		[Required, StringLength(100)]
		public string? PhoneNumber { get; set; }

		[Required, StringLength(100)]
		public string? Email { get; set; }
		[Required, StringLength(100)]
		public string? Password { get; set; }

		public IFormFile ProfilePicture { get; set; } 


	}
}
