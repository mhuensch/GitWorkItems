using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.GitWorkItems.Providers
{
	[ImplementPropertyChanged]
	public class Query
	{
		public string Title { get; set; }
	}
}
