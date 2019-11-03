using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Validation {

	internal static class Extensions {

		public static ArgumentWrapper<T> RequireThat<T>(this T argument, string name) {
			return new ArgumentWrapper<T>(argument, name);
		}

		public static ArgumentWrapper<T> RequireThat<T>(this T argument) {
			return new ArgumentWrapper<T>(argument);
		}

		public static ArgumentWrapper<T> IsNotNull<T>(this ArgumentWrapper<T> argument) where T : class {
			if (argument.Value == null)
				throw new ArgumentNullException(argument.Name);

			return argument;
		}

	}

}