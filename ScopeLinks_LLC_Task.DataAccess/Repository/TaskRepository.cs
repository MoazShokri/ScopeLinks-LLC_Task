using ScopeLinks_LLC_Task.DataAccess.Entities;
using ScopeLinks_LLC_Task.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class TaskRepository : Repository<Tasks>, ITaskRepository
	{
		private readonly ApplicationDbContext _db;

		public TaskRepository(ApplicationDbContext db) : base(db)
		{
			this._db = db;
		}

		public async Task<IEnumerable<Tasks>> GetMyTasks(string userId)
		{
			return await _db.Tasks
				.Where(t => t.CreatorId == userId || t.AssignedUsers.Any(au => au.UserId == userId)).OrderBy(x=>x.DueDate)
				.ToListAsync();
		}
		public async Task<IEnumerable<Tasks>> SearchTasksByDueDate(string userId, DateTime dueDate)
		{
			return await _db.Tasks
				.Where(task => (task.CreatorId == userId 
				|| task.AssignedUsers.Any(au => au.UserId == userId))
				&& task.DueDate.Date == dueDate.Date)
				.OrderBy(task => task.DueDate)
				.ToListAsync();
		}

	}
}
