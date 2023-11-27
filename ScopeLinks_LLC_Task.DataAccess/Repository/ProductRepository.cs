using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ScopeLinks_LLC_Task.DataAccess.Dtos;
using ScopeLinks_LLC_Task.DataAccess.Entities;
using ScopeLinks_LLC_Task.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly ApplicationDbContext _db;

		public ProductRepository(ApplicationDbContext db) : base(db)
		{
			this._db = db;

		}


		public async Task<int> AddProductAsync(ProductAddDto productDto)
		{
			var product = new Product
			{
				Name = productDto.Name,
				Price = productDto.Price,
				Description = productDto.Description
			};

			var imagePaths = await UploadImages(productDto.Images);
			product.Images.AddRange(imagePaths);

			await _db.Products.AddAsync(product);
			await _db.SaveChangesAsync();

			return product.Id;
		}
		public async Task<int> UpdateProduct(Product existingProduct, ProductUpdateDto productDto)
		{
			existingProduct.Name = productDto.Name;
			existingProduct.Price = productDto.Price;
			existingProduct.Description = productDto.Description;

			// Update or add new images as needed
			var imagePaths = await UploadImages(productDto.Images);
			existingProduct.Images.Clear(); // Clear existing images
			existingProduct.Images.AddRange(imagePaths); // Add new images

			await _db.SaveChangesAsync();

			return existingProduct.Id;
		}

		private async Task<List<ProductImage>> UploadImages(List<IFormFile> images)
		{
			var uploadedImages = new List<ProductImage>();

			foreach (var image in images)
			{
				var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await image.CopyToAsync(fileStream);
				}

				var productImage = new ProductImage
				{
					FileName = fileName
				};

				uploadedImages.Add(productImage);
			}

			return uploadedImages;
		}
		public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
		{
			// Assuming _dbContext is your database context
			var products = await _db.Products
				.Where(p => productIds.Contains(p.Id))
				.ToListAsync();

			return products;
		}
	}
}
