
namespace ScopeLinks_LLC_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		protected APIResponse _response;
		private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
		private long _maxAllowedProfilePicture = 1048576;
		public AuthController(IAuthService authService)
		{
			this._authService = authService;
			_response = new();
		}

		[HttpPost("register")]
		public async Task<ActionResult<APIResponse>> RegisterAsync([FromForm] RegisterModel model)
		{
			try
			{
				if (model.ProfilePicture == null)
				{
					return BadRequest("Profile picture is required!");
				}

				if (!_allowedExtenstions.Contains(Path.GetExtension(model.ProfilePicture.FileName).ToLower()))
				{
					return BadRequest("Only .png and .jpg images are allowed!");
				}

				if (model.ProfilePicture.Length > _maxAllowedProfilePicture)
				{
					return BadRequest("Max allowed size for profile picture is 1MB!");
				}

				if (!ModelState.IsValid)
				{
					var errorMessages = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages = errorMessages;
					return BadRequest(_response);
				}

				var result = await _authService.RegisterAsync(model);

				if (!result.IsAuthenticated)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add(result.Message); // Use the error message from the authModel
					return BadRequest(_response);
				}

				// Store the profile picture data
				using var dataStream = new MemoryStream();
				await model.ProfilePicture.CopyToAsync(dataStream);
				result.ProfilePicture = dataStream.ToArray();

				_response.StatusCode = HttpStatusCode.Created;
				_response.IsSuccess = true;
				_response.Result = result;
				return _response;
			}
			catch (Exception ex)
			{
				// Log the exception for debugging purposes
				// You can use a logging framework or Console.WriteLine for debugging
				Console.WriteLine($"Exception during registration: {ex}");

				// Return an error response
				_response.StatusCode = HttpStatusCode.InternalServerError;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("An error occurred during registration.");
				return _response;
			}
		}


		[HttpPost("Login")]
		public async Task<ActionResult<APIResponse>> GetTokenAsync([FromForm] TokenRequestModel model)
		{
			if (!ModelState.IsValid)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Email or password is incorrect !");
				return BadRequest(_response);
			}

			var result = await _authService.GetTokenAsync(model);

			if (!result.IsAuthenticated)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Email or password is incorrect !");
				return BadRequest(_response);
			}

			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result = result;
			return Ok(_response);
		}
	}
}
