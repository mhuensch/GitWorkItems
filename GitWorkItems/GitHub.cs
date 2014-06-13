using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Run00.GitWorkItems
{
	public class GitHub : IGitHub
	{
		public GitHub(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		private readonly IServiceProvider _serviceProvider;
	}
}
