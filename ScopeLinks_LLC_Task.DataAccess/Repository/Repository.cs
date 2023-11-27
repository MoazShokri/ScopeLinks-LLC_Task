

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		public DbSet<T> _dbset;

		public Repository(ApplicationDbContext db)
		{
			this._db = db;
			this._dbset = db.Set<T>();
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await _dbset.ToListAsync();

		}
		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
		{
			return await _dbset.Where(filter).ToListAsync();
		}

		public async Task<T> AddAsync(T entity)
		{
			await _dbset.AddAsync(entity);
			await _db.SaveChangesAsync();
			return entity;
		}

		public async Task<T> DeleteAsync(T entity)
		{
			_dbset.Remove(entity);
			await _db.SaveChangesAsync();

			return await Task.FromResult(entity);
		}


		public async Task<T> GetFristOrDefaultAsync(Expression<Func<T, bool>> filter)
		{
			return await _dbset.FirstOrDefaultAsync(filter);
		}

		public async virtual void Update(T entity)
		{
			_dbset.Update(entity);
			

		}
		public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool Tracking = true)
		{
			IQueryable<T> query = _dbset;
			if (!Tracking)
			{
				query = query.AsNoTracking();
			}
			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.FirstOrDefaultAsync(filter);

		}

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null)
		{
			if (filter != null)
			{
				return await _dbset.AnyAsync(filter);
			}
			else
			{
				return await _dbset.AnyAsync();
			}

		}
		public async Task<T> GetAsyncWithInclude(Expression<Func<T, bool>> filter,
			bool Tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
		{
			IQueryable<T> query = _dbset;

			if (!Tracking)
			{
				query = query.AsNoTracking();
			}

			if (include != null)
			{
				query = include(query);
			}

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.FirstOrDefaultAsync(filter);
		}
		public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
		{
			if (tracked)
			{
				IQueryable<T> query = _dbset;

				query = query.Where(filter);
				if (includeProperties != null)
				{
					foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						query = query.Include(includeProp);
					}
				}
				return query.FirstOrDefault();
			}
			else
			{
				IQueryable<T> query = _dbset.AsNoTracking();

				query = query.Where(filter);
				if (includeProperties != null)
				{
					foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						query = query.Include(includeProp);
					}
				}
				return query.FirstOrDefault();
			}

		}
		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = _dbset;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}
			return query.ToList();
		}
	}
}
