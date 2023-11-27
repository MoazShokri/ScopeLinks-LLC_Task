namespace ScopeLinks_LLC_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]

	public class UsersController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IAuthService _authService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		protected APIResponse _response;


		public UsersController(UserManager<ApplicationUser> userManager,
		   SignInManager<ApplicationUser> signInManager,
			 RoleManager<IdentityRole> roleManager,
		   IAuthService authService,
		   IUnitOfWork unitOfWork,
		   IMapper mapper)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._roleManager = roleManager;
			this._authService = authService;
			this._unitOfWork = unitOfWork;
			this._mapper = mapper;
			_response = new();
		}

		[HttpGet]
		public async Task<ActionResult<APIResponse>> GetListUsers()
		{
			var users = await _userManager.Users.ToListAsync();

			if (users == null || users.Count == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new() { "Not Exist Users !" };
				_response.IsSuccess = false;
				return BadRequest(_response);
			}

			var userViewModels = new List<UserGetDto>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);


				var userViewModel = new UserGetDto
				{
					Id = user.Id,
					UserName = user.UserName,
					Email = user.Email,
					Roles = roles.ToList(),
				
				};

				userViewModels.Add(userViewModel);
			}
			
			_response.StatusCode = HttpStatusCode.OK;
			_response.Result = userViewModels;
			return Ok(_response);

		}
		[HttpPost("addUser")]
		public async Task<ActionResult<APIResponse>> AddUser(UserAddDto model)
		{
			if (!ModelState.IsValid)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("User is not authorized");
				return BadRequest(_response);
			}

			if (!model.Roles.Any(r => r.IsSelected))
			{
				ModelState.AddModelError("Roles", "Please select at least one role");
			}

			var userByEmail = await _userManager.FindByEmailAsync(model.Email);
			if (userByEmail != null)
			{
				ModelState.AddModelError("Email", "Email already exists");
			}

			var userByUserName = await _userManager.FindByNameAsync(model.UserName);
			if (userByUserName != null)
			{
				ModelState.AddModelError("UserName", "User Name already exists");
			}


			var user = new ApplicationUser
			{
				FristName = model.FristName,
				LastName = model.LastName,
				Email = model.Email,
				UserName = model.UserName,

			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("Password", error.Description);
				}
			}
			else
			{
				await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.Name));
			}

			if (!ModelState.IsValid)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
				return BadRequest(_response);
			}

			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result = model;
			return Ok(_response);
		}

		[HttpGet("GetAllRoles")]
		public async Task<IActionResult> GetAllRoles()
		{

			var Roles = await _roleManager.Roles.ToListAsync();
			if (Roles == null || Roles.Count == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Role is not authorized or does not exist.");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result = Roles;
			return Ok(_response);

		}

	}
}

