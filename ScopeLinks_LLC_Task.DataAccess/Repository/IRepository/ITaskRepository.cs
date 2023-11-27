using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository.IRepository
{
	public interface ITaskRepository : IRepository<Tasks>
	{
		Task<IEnumerable<Tasks>> GetMyTasks(string userId);
		Task<IEnumerable<Tasks>> SearchTasksByDueDate(string userId, DateTime dueDate);
	}
}
