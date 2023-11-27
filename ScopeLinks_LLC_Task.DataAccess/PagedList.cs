using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeLinks_LLC_Task.DataAccess
{
	public class PagedList<T>
	{
		public List<T> Items { get; }
		public int TotalCount { get; }
		public int PageNumber { get; }
		public int PageSize { get; }
		public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

		public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
		{
			Items = items;
			TotalCount = totalCount;
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
	}
}
