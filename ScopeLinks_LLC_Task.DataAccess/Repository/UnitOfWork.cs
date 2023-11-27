using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;
		public ITaskRepository Tasks { get; private set; }
		public IUserTasksRepository UserTasks { get; private set; }
		public IProductRepository Products { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }
		public IProductImageRepository ProductImage { get; private set; }
		public IOrderRepository Orders { get; private set; }
		//public IOrderHeaderRepository OrderHeaders { get; private set; }







		public UnitOfWork(ApplicationDbContext db)
		{
			this._db = db;
			Tasks = new TaskRepository(_db);
			UserTasks= new UserTasksRepository(_db);
			Products = new ProductRepository(_db);
			ShoppingCart= new ShoppingCartRepository(_db);
			ProductImage= new ProductImageRepository(_db);
			Orders = new OrderRepository(_db);
			//OrderHeaders = new OrderHeaderRepository(_db);
		}
		public async void Save()
		{
			await _db.SaveChangesAsync();
		}
	}
}
