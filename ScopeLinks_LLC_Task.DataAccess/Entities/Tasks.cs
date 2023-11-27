using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class Tasks
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime DueDate { get; set; }
		public string CreatorId { get; set; }

		public bool Status { get; set; }
		public List<UserTasks> AssignedUsers { get; set; }



	}
}
