using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Entities
{
	public class UserTasks
	{
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public int TaskId { get; set; }
		public Tasks Task	{ get; set;}

	}
}
