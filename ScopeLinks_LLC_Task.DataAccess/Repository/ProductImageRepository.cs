using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
	{
		private readonly ApplicationDbContext _db;


		public ProductImageRepository(ApplicationDbContext db) : base(db)
		{
          this._db = db;
		}
		public async Task<List<ProductImage>> GetImagesByProductIdAsync(int productId)
		{
			return await _db.ProductImages
				.Where(image => image.ProductId == productId)
				.ToListAsync();
		}
	}
}
