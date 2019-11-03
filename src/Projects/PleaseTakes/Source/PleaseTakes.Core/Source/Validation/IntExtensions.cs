using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Validation {

	internal static class IntExtensions {

		public static ArgumentWrapper<int> IsInRange(this ArgumentWrapper<int> argument, int low, int high) {
			if ((argument.Value < low) || (argument.Value > high))
				throw new IndexOutOfRangeException(argument.Name);

			return argument;
		}

	}

}