using System.ComponentModel.DataAnnotations;

namespace ScopeLinks_LLC_Task.Services.Models
{
	public class TokenRequestModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
