using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScopeLinks_LLC_Task.DataAccess.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ScopeLinks_LLC_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = Roles.Admin)]

	public class ProductsController : ControllerBase
	{
		protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;

		public ProductsController(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper,
			UserManager<ApplicationUser> userManager)
		{
			this._unitOfWork = unitOfWork;
			this._mapper = mapper;
			this._userManager = userManager;
			_response = new();
		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetProduct()
		{
			try
			{
				var products = await _unitOfWork.Products.GetAllAsync();
				var productDtos = _mapper.Map<List<productGetDto>>(products);

				// Include image information
				foreach (var productDto in productDtos)
				{
					var productImages = await _unitOfWork.ProductImage.GetImagesByProductIdAsync(productDto.Id);
					productDto.Images = _mapper.Map<List<ProductImageDto>>(productImages);
				}

				_response.Result = productDtos;
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(ex.Message);
			}

		}
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetProductById(int id)
		{
			Product product = await _unitOfWork.Products.GetAsyncWithInclude(x => x.Id == id, include: q => q.Include(p => p.Images));

			if (product != null)
			{
				var productDto = _mapper.Map<ProductOneDto>(product);
				return Ok(productDto);
			}
			try
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages = new() { " This Product Not Found " };
				_response.IsSuccess = false;
				return NotFound(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(ex.Message);
			}
		}
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> CreateProduct([FromForm] ProductAddDto product)
		{
			if (!ModelState.IsValid)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new() { "Model State is Not Valid" };
				_response.IsSuccess = false;
				return BadRequest(_response);
			}
			if (product.Images == null || product.Images.Count == 0)
			{
				ModelState.AddModelError("Images", "At least one image is required.");
				return BadRequest(ModelState);
			}
			try
			{

				await _unitOfWork.Products.AddProductAsync(product);
				_unitOfWork.Save();
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(ex.Message);
			}
		}
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> EditProduct([FromForm] int id, [FromForm] ProductUpdateDto product)
		{

			var existingProduct = await _unitOfWork.Products.GetFristOrDefaultAsync(x => x.Id == id);

			if (existingProduct == null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages = new List<string> { "This Product does not exist" };
				_response.IsSuccess = false;
				return NotFound(_response);
			}

			try
			{
				await _unitOfWork.Products.UpdateProduct(existingProduct, product);

				_response.Result = _mapper.Map<ProductOneDto>(existingProduct);
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(_response);
			}
		}
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _unitOfWork.Products.GetFristOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages = new() { "This Product not exist" };
				return NotFound(_response);
			}
			try
			{
				await _unitOfWork.Products.DeleteAsync(product);
				_unitOfWork.Save();
				return Ok(product);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(ex.Message);
			}


		}
	}
}
	
