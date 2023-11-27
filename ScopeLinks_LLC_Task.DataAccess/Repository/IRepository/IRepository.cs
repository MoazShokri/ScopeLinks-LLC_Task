using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync();
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

		Task<T> GetFristOrDefaultAsync(Expression<Func<T, bool>> filter);
		T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);

		Task<T> GetAsync(Expression<Func<T, bool>> filter, bool Tracking = true);
		Task<T> AddAsync(T entity);
		Task<T> DeleteAsync(T entity);
		void Update(T entity);
		Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null);
		Task<T> GetAsyncWithInclude(Expression<Func<T, bool>> filter, bool Tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
	}
}
