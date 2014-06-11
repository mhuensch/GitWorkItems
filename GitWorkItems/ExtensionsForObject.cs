using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.GitWorkItems
{
	internal static class ExtensionsForObject
	{
		public static T GetPropertyValue<T>(this object obj, string name)
			where T : class
		{
			if (obj == null)
				return null;

			var prop = obj.GetType().GetProperty(name);
			if (prop == null)
				return null;

			return prop.GetValue(obj) as T;
		}
	}
}
