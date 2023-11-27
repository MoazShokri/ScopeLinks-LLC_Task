using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Dtos
{
	public class UserGetDto
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }

		public IEnumerable<string> Roles { get; set; }

	}
}
