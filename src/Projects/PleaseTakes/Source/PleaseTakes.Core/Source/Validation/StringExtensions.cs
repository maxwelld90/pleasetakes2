using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Validation {

	internal static class StringExtensions {

		public static ArgumentWrapper<string> IsNotNullOrEmpty(this ArgumentWrapper<string> argument) {
			if (string.IsNullOrEmpty(argument.Value))
				throw new NullReferenceException(argument.Name);

			return argument;
		}

		public static ArgumentWrapper<string> RemovePattern(this ArgumentWrapper<string> argument, string pattern) {
			argument.Value.Replace(pattern, "");
			return argument;
		}

		public static ArgumentWrapper<string> ReplaceWithPattern(this ArgumentWrapper<string> argument, string oldValue, string newPattern) {
			argument.Value.Replace(oldValue, newPattern);
			return argument;
		}

	}

}