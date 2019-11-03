using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PleaseTakes.Core {

	internal static class Utils {

		public static string MappedApplicationPath {
			get {
				string appPath = WebServer.Request.ApplicationPath.ToLower();

				if (appPath.Equals("/"))
					appPath = "/";
				else if (!appPath.EndsWith(@"/"))
					appPath += @"/";

				string mapped = WebServer.Server.MapPath(appPath);

				if (!mapped.EndsWith(@"\"))
					mapped += @"\";

				return mapped;
			}
		}

		public static int RandomNumber(int low, int high) {
			Random random = new Random();
			return random.Next(low, high);
		}

		public static string FormatStackTrace(string stackTrace) {
			stackTrace = stackTrace.ReplaceFirst("PleaseTakes", "<strong>PleaseTakes</strong>");
			stackTrace = stackTrace.ReplaceFirst("at ", "...");

			return stackTrace;
		}

		// USEFUL!!!
		public static string ReplaceFirst(this string text, string search, string replace) {
			int pos = text.IndexOf(search);

			if (pos < 0)
				return text;

			return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}

		public static bool IsDateInRange(DateTime low, DateTime high, DateTime date) {
			if ((date >= low) && (date <= high))
				return true;
			return false;
		}

		public static int CompareDateWithNow(DateTime date) {
			DateTime dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			return (DateTime.Compare(date, dateNow));
		}

		public static DateTime GetWeekDates(bool getStartOfWeek, DateTime date) {
			int dateWeekDay = (int)date.DayOfWeek;
			int toEndOfWeek = (int)DayOfWeek.Saturday - (int)date.DayOfWeek;

			if (getStartOfWeek)
				return date.Subtract(new TimeSpan(dateWeekDay, 0, 0, 0));
			else
				return date.AddDays(toEndOfWeek);
		}

		public static string GetWeekDayAbbreviation(DayOfWeek dayOfWeek) {
			DateTimeFormatInfo dateTimeInfo = new DateTimeFormatInfo();
			return (dateTimeInfo.GetAbbreviatedDayName(dayOfWeek));
		}

		public static string GetMonthName(int month) {
			DateTimeFormatInfo dateTimeInfo = new DateTimeFormatInfo();
			return (dateTimeInfo.GetMonthName(month));
		}

		public static void SetTemporaryStorageObject(string key, object value) {
			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey(key))
				Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage[key] = value;
			else
				Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add(key, value);
		}

		public static object ReturnTemporaryStorageObject(string key, bool deleteAfter) {
			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey(key)) {
				object temp = Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage[key];
				
				if (deleteAfter)
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Remove(key);
				
				return temp;
			}

			return null;
		}

		public static string GetRandomPassword() {
			Random randomNumber = new Random();
			string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
			string returnStr = "";

			for (int i = 0; i <= (Consts.MinimumPasswordLength - 1); i++)
				returnStr += chars[randomNumber.Next(chars.Length)];

			return returnStr;
		}

	}

}