
namespace ScopeLinks_LLC_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]

	public class UsersTasksController : ControllerBase
	{
		protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;

		public UsersTasksController(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper,
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
		public async Task<ActionResult<APIResponse>> GetAllUserTasks()
		{
			try
			{
				var UserTask = await _unitOfWork.UserTasks.GetAllAsync();
				_response.Result = _mapper.Map<List<UserTaskGetDto>>(UserTask);
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
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> CreateTaskUser([FromForm] TaskUserDto taskUser)
		{
			if (!ModelState.IsValid)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new() { "Model State is Not Valid" };
				_response.IsSuccess = false;
				return BadRequest(_response);
			}
			try
			{
				UserTasks UsrTaskMap = _mapper.Map<UserTasks>(taskUser);
				_response.Result = UsrTaskMap;
				await _unitOfWork.UserTasks.AddAsync(UsrTaskMap);
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
		[HttpGet("myTasks")]
		public async Task<IActionResult> GetMyTasks()
		{
			try
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get the current user's ID

				if (string.IsNullOrEmpty(userId))
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages = new() { "User ID not found" };
					_response.IsSuccess = false;
					return BadRequest(_response);
				}

				// Retrieve tasks where the user is either the creator or assigned
				var tasks = await _unitOfWork.Tasks.GetMyTasks(userId);

				// Map the tasks to the DTO
				var taskDtos = _mapper.Map<IEnumerable<TaskGetDto>>(tasks); // Note the change to IEnumerable<TaskGetDto>

				_response.Result = taskDtos;
				_response.IsSuccess = true;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(_response);
			}
		}

		[HttpPut("updateTaskStatus/{taskId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize] 
		public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] bool isFinished)
		{
			try
			{
				// Get the current user's ID
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

				if (string.IsNullOrEmpty(userId))
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("User ID not found");
					return BadRequest(_response);
				}

				var user = await _userManager.FindByNameAsync(userId); 

				if (user == null)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("User not found");
					return BadRequest(_response);
				}
				// Retrieve the task by taskId
				var task = await _unitOfWork.Tasks.GetAsync(x=>x.Id == taskId);

				if (task == null)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Task not found");
					return BadRequest(_response);
				}

				// Check if the user is assigned to the task
				var isAssignedToTask = await _unitOfWork.UserTasks
					.AnyAsync(ut => ut.TaskId == taskId && ut.UserId == user.Id);

				if (!isAssignedToTask && !User.IsInRole(Roles.Admin))
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("You are not assigned to this task.");
					return BadRequest(_response);
				}

				// Update the task's status
				task.Status = isFinished;

				// Save changes to the database
				_unitOfWork.Tasks.Update(task);
				_unitOfWork.Save();

				_response.StatusCode = HttpStatusCode.OK;
				_response.IsSuccess = true;
				_response.Result = task;
				return Ok(_response);
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
