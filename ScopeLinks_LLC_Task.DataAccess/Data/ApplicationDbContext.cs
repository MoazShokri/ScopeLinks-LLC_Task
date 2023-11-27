using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScopeLinks_LLC_Task.DataAccess.Entities;
using System.Reflection.Emit;

namespace ScopeLinks_LLC_Task.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<UserTasks>().HasKey(x => new { x.UserId, x.TaskId });
			builder.Entity<Order>()
	         .HasOne(o => o.User)
	         .WithMany(u => u.Orders)
	         .HasForeignKey(o => o.UserId)
	         .OnDelete(DeleteBehavior.Cascade);

			// Seed Admin Account
			var hasher = new PasswordHasher<ApplicationUser>();

			builder.Entity<ApplicationUser>().HasData(
				new ApplicationUser
				{
					UserName = "adminSeed@example.com",
					FristName = "adminSeed",
					LastName = "adSeed",
					NormalizedUserName = "ADMINSEED@EXAMPLE.COM",
					Email = "adminSeed@example.com",
					NormalizedEmail = "ADMINSEED@EXAMPLE.COM",
					EmailConfirmed = true,
					PasswordHash = hasher.HashPassword(null, "123456@@789"),
					SecurityStamp = string.Empty
				}
			);


			base.OnModelCreating(builder);
		}
		public DbSet<Tasks> Tasks { get; set; }
		public DbSet<UserTasks> UserTasks { get; set; }
		public DbSet<Product> Products { get; set; }
	    public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Order> Orders{ get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		public DbSet<ShoppingCart> shoppingCarts { get; set; }





	}
}
