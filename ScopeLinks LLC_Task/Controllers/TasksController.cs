
namespace ScopeLinks_LLC_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TasksController : ControllerBase
	{
		protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;

		public TasksController(IAuthService authService , IUnitOfWork unitOfWork, IMapper mapper,
			UserManager<ApplicationUser> userManager)
		{
			this._unitOfWork = unitOfWork;
			this._mapper = mapper;
			this._userManager = userManager;
			_response = new();
		}



		[HttpGet("GetAllTasksToAdmin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Policy = Roles.Admin)]

		public async Task<IActionResult> GetAllTasksToAdmin()
		{
			try
			{
				var task = await _unitOfWork.Tasks.GetAllAsync();
				_response.Result = _mapper.Map<List<TaskGetDto>>(task);
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
		public async Task<IActionResult> CreateTask([FromBody] TaskAddDto task)
		{
			var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (!ModelState.IsValid)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new() { "Model State is Not Valid" };
				_response.IsSuccess = false;
				return BadRequest(_response);
			}
			try
			{
				Tasks tsk = _mapper.Map<Tasks>(task);
				tsk.CreatorId = currentUserId;
				if(tsk.CreatorId == null)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages = new() { "not exist User Create the task" };
					_response.IsSuccess = false;
					return BadRequest(_response);
				}
				_response.Result = tsk;
				await _unitOfWork.Tasks.AddAsync(tsk);
				_unitOfWork.Save();
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(_response);
			}
		}

		[HttpDelete("deleteTask/{taskId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize]
		public async Task<IActionResult> DeleteTask(int taskId)
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

				// Retrieve the task by taskId
				var task = await _unitOfWork.Tasks.GetAsync(x => x.Id == taskId);

				if (task == null)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Task not found");
					return BadRequest(_response);
				}
				
				// Check if the user is the owner of the task
				if (task.CreatorId != userId && !User.IsInRole(Roles.Admin))
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("You are not the owner of this task.");
					return BadRequest(_response);
				}

				// Delete the task
				await _unitOfWork.Tasks.DeleteAsync(task);
				_unitOfWork.Save();

				_response.StatusCode = HttpStatusCode.OK;
				_response.IsSuccess = true;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				return BadRequest(ex.Message);
			}
		}


		[HttpGet("searchTasksByDueDate")]
		public async Task<IActionResult> SearchTasksByDueDate([FromQuery] DateTime dueDate)
		{
			try
			{
				// Get the current user's ID
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

				if (string.IsNullOrEmpty(userId))
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages = new() { "User ID not found" };
					_response.IsSuccess = false;
					return BadRequest(_response);
				}

				// Replace "username" with the actual user identifier you have
				var user = await _userManager.FindByNameAsync(userId); 

				if (user == null)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("User not found");
					return BadRequest(_response);
				}

				// Retrieve tasks where the user is either the creator or assigned and due on the specified date
				var tasks = await _unitOfWork.Tasks.SearchTasksByDueDate(user.Id, dueDate);

				// Map the tasks to the DTO
				var taskDtos = _mapper.Map<List<TaskGetDto>>(tasks);

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



	}
}
