
namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class ApplicationUser : IdentityUser
	{
		[Required, MaxLength(50)]
		public string? FristName { get; set; }
		[Required, MaxLength(50)]
		public string? LastName { get; set; }

		public byte[]? ProfilePicture { get; set; }
		public List<UserTasks> AssignedTasks { get; set; }
		public List<CartItem> Cart { get; set; }

		public List<Order> Orders { get; set; }



	}
}
