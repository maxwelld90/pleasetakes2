using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core {

	internal static class Consts {

		public const int MinimumPasswordLength = 4;
		public const int SchoolIDLength = 9;
		public const string DateTimeFull = "dddd, dd MMMM yyyy HH:mm:ss";
		public const string DateTimeISO = "yyyy-MM-dd";
		public const string StringVariableMarkerStart = "{$";
		public const string StringVariableMarkerEnd = "}";
		public const string WebPageTitlePrefix = "PleaseTakes 2";
		public const string BuildDate = "2010-09-15";
		public const string BreadcrumbSeparator = "&nbsp;&gt;";

		public enum TimetablingLimits {
			WeekLimit = 3,
			PeriodLimit = 7
		}

		public enum SessionTimeout {
			Minimum = 5,
			Maximum = 30
		}

	}

}