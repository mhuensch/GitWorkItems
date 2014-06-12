using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.GitWorkItems
{
	internal static class ExtensionsForIServiceProvider
	{
		public static T GetService<T>(this IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				return default(T);

			return (T)serviceProvider.GetService(typeof(T));
		}
	}
}
