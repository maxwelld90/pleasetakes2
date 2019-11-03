using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.Core.BasicResponses.Login {

	internal static class Action {

		public static void AttemptLogin() {
			if (BothFieldsContainData()) {
				if (UsernameValid(true) && UsernameValid(false))
					Action.Redirects.Duplicate();

				if (UsernameValid(true))
					Action.Checks(true);
				else if (UsernameValid(false))
					Action.Checks(false);
				else
					Action.Redirects.Invalid();
			}
		}

		private static string Username {
			get {
				return WebServer.Request["Username"];
			}
		}

		private static string Password {
			get {
				return WebServer.Request["Password"];
			}
		}

		private static bool BothFieldsContainData() {
			if ((string.IsNullOrEmpty(Action.Username)) || (string.IsNullOrEmpty(Action.Password))) {
				Action.Redirects.Missing();
				return false;
			}

			return true;
		}

		private static bool UsernameValid(bool inDatabase) {
			if (inDatabase) {
				Helpers.Database.ParameterBuilder paramBuilder = new Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.VarChar, "@Username", Action.Username);

				using (SqlDataReader dataReader = Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Login/usernamecheck.sql", paramBuilder.Parameters)) {
					if (dataReader.Read())
						return true;
					else
						return false;
				}
			}
			else
				return (WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.NonTeachingAccounts.Exists(Action.Username));
		}

		private static void Checks(bool inDatabase) {
			if (inDatabase) {
				Helpers.Database.ParameterBuilder paramBuilder = new Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.VarChar, "@Username", Action.Username);
				paramBuilder.AddParameter(SqlDbType.VarChar, "@Password", Action.Password);

				using (SqlDataReader dataReader = Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Login/retrieve.sql", paramBuilder.Parameters)) {
					int timetableId;

					if (dataReader.Read())
						if (!int.TryParse(dataReader["TimetableId"].ToString(), out timetableId))
							Redirects.MissingTimetableId();
						else
							if ((bool)dataReader["IsActive"]) {
								// Temporary
								if ((bool)dataReader["IsAdmin"]) {
									UserManagement.Accounts.TeachingAccount account = new UserManagement.Accounts.TeachingAccount(
										dataReader["Username"] as string,
										dataReader["Password"] as string,
										dataReader["Title"] as string,
										dataReader["Forename"] as string,
										dataReader["Surname"] as string,
										dataReader["HoldingName"] as string,
										(int)dataReader["StaffId"],
										(int)dataReader["TimetableId"],
										(bool)dataReader["IsActive"],
										(bool)dataReader["IsAdmin"]);

									Action.LoginUser(account);
								}
								// Temporary
								else
									Action.Redirects.NonAdmin();
							}
							else
								Action.Redirects.Disabled();
					else
						Action.Redirects.Invalid();
				}
			}
			else {
				UserManagement.Accounts.Account account = WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.NonTeachingAccounts[Action.Username];

				if (account.Password.Equals(Action.Password))
					if (account.IsActive)
						Action.LoginUser(account);
					else
						Action.Redirects.Disabled();
				else
					Action.Redirects.Invalid();
			}
		}

		private static void LoginUser(UserManagement.Accounts.Account account) {
			WebServer.PleaseTakes.Session.CurrentInstance.Account = account;
			Action.Redirects.Success();
		}

		public static void LogoutUser() {
			WebServer.PleaseTakes.Session.CurrentInstance.Account = null;
			Action.Redirects.LoggedOut();
		}

		private static class Redirects {

			public static void Missing() {
				WebServer.PleaseTakes.Redirect("/login/missing/");
			}

			public static void Duplicate() {
				WebServer.PleaseTakes.Redirect("/login/duplicate/");
			}

			public static void Invalid() {
				WebServer.PleaseTakes.Redirect("/login/invalid/");
			}

			public static void MissingTimetableId() {
				WebServer.PleaseTakes.Redirect("/login/missingtimetable/");
			}

			public static void Disabled() {
				WebServer.PleaseTakes.Redirect("/login/disabled/");
			}

			// Temporary
			public static void NonAdmin() {
				WebServer.PleaseTakes.Redirect("/login/nonadmin/");
			}

			public static void Success() {
				WebServer.PleaseTakes.Redirect("/home/");
			}

			public static void LoggedOut() {
				WebServer.PleaseTakes.Redirect("/login/loggedout/");
			}

		}

	}

}
