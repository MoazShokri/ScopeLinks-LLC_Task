using ScopeLinks_LLC_Task.DataAccess.Dtos;
using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository.IRepository
{
	public interface IProductRepository : IRepository<Product>
	{
		Task<int> AddProductAsync(ProductAddDto productDto);
		Task<int> UpdateProduct(Product existingProduct, ProductUpdateDto productDto);
		Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);

	}
}
