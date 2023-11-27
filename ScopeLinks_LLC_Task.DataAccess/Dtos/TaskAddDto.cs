using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess.Dtos
{
	public class TaskAddDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime DueDate { get; set; }

	}
}
