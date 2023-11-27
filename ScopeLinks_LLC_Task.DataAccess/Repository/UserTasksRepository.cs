using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class UserTasksRepository : Repository<UserTasks>, IUserTasksRepository
	{
		public UserTasksRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
