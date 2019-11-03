using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Validation.Specific {

	internal static class Settings {

		public static void DatabaseConnectionString(string value) {
			value.RequireThat("database connection string").IsNotNullOrEmpty();
		}

		public static void SchoolID(string value) {

		}

		public static void Timeout(int value) {
			value.RequireThat("timeout value").IsInRange((int)Consts.SessionTimeout.Minimum, (int)Consts.SessionTimeout.Maximum);
		}

		public static void StartDate(DateTime value, DateTime endDate) {

		}

		public static void EndDate(DateTime value, DateTime startDate) {

		}

	}

}