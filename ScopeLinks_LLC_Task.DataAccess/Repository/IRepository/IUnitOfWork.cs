using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		public ITaskRepository Tasks { get;}
		public IUserTasksRepository UserTasks { get;}
		public IProductRepository Products { get;}
		public IShoppingCartRepository ShoppingCart { get; }
		public IProductImageRepository ProductImage { get; }

		public IOrderRepository Orders { get; }

		//public IOrderHeaderRepository OrderHeaders { get; }

		void Save();

	}
}
