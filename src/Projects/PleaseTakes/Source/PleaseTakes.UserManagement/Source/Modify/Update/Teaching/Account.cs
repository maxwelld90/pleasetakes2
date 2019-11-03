using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching {

	internal sealed class Account {
		private bool _hasAccount;
		private int _staffId;
		private string _username;
		private bool _isAdmin;
		private bool _isActive;

		public Account() {
			if (this.HasStaffId() && this.HasAccount())
				this.DoUpdate();
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
		}

		private void DoUpdate() {
			string randomPassword = Core.Utils.GetRandomPassword();

			Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
			paramBuilder.AddParameter(SqlDbType.Bit, "@HasAccount", this._hasAccount);

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.IsTeachingAccount) {
				Accounts.TeachingAccount account = (Accounts.TeachingAccount)Core.WebServer.PleaseTakes.Session.CurrentInstance.Account;
				paramBuilder.AddParameter(SqlDbType.Int, "@CurrentStaffId", account.StaffId);
			}
			else
				paramBuilder.AddParameter(SqlDbType.Int, "@CurrentStaffId", null);

			if (this._hasAccount)
				if (this.HasUsername() && this.HasAccountType() && this.HasAccountStatus()) {
					paramBuilder.AddParameter(SqlDbType.VarChar, "@Username", this._username);
					paramBuilder.AddParameter(SqlDbType.VarChar, "@Password", randomPassword);
					paramBuilder.AddParameter(SqlDbType.Bit, "@IsAdmin", this._isAdmin);
					paramBuilder.AddParameter(SqlDbType.Bit, "@IsActive", this._isActive);
				}
				else {
					Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "missing");
					Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
				}
			else {
				paramBuilder.AddParameter(SqlDbType.VarChar, "@Username", "");
				paramBuilder.AddParameter(SqlDbType.VarChar, "@Password", "");
				paramBuilder.AddParameter(SqlDbType.Bit, "@IsAdmin", false);
				paramBuilder.AddParameter(SqlDbType.Bit, "@IsActive", false);
				Core.WebServer.Response.Write("No account");
			}

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.NonTeachingAccounts.Exists(this._username)) {
				Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "exists");
				Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
			}
			else {
				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Update/Teaching/account.sql", paramBuilder.Parameters)) {
					dataReader.Read();

					switch ((int)dataReader["Status"]) {
						case 0:
							Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
							break;
						case 1:
							Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "exists");
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
							break;
						case 2:
							if (Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.IsTeachingAccount) {
								Accounts.TeachingAccount account = (Accounts.TeachingAccount)Core.WebServer.PleaseTakes.Session.CurrentInstance.Account;

								if (account.StaffId.Equals(this._staffId))
									account.Password = randomPassword;
							}

							Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "updated");
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
							break;
						case 3:
							if (Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.IsTeachingAccount) {
								Accounts.TeachingAccount account = (Accounts.TeachingAccount)Core.WebServer.PleaseTakes.Session.CurrentInstance.Account;

								if (account.StaffId.Equals(this._staffId))
									account.Password = randomPassword;
							}

							Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountPassword", randomPassword);
							Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "created");
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
							break;
						case 4:
							Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "deleted");
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
							break;
						case 5:
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
							break;
						case 6:
							Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "loggedin");
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
							break;
					}
				}
			}
		}

		private bool HasStaffId() {
			string fromForm = Core.WebServer.Request["StaffId"];

			if (int.TryParse(fromForm, out this._staffId))
				return true;

			return false;
		}

		private bool HasAccount() {
			string fromForm = Core.WebServer.Request["HasAccount"];

			if (!string.IsNullOrEmpty(fromForm)) {
				fromForm = fromForm.ToLower();

				if (fromForm.Equals("enabled")) {
					this._hasAccount = true;
					return true;
				}
				else if (fromForm.Equals("disabled"))
					return true;
			}

			return false;
		}

		private bool HasUsername() {
			string fromForm = Core.WebServer.Request["Username"];

			if (!string.IsNullOrEmpty(fromForm)) {
				this._username = fromForm;
				return true;
			}

			return false;
		}

		private bool HasAccountType() {
			string fromForm = Core.WebServer.Request["AccountType"];

			if (!string.IsNullOrEmpty(fromForm)) {
				fromForm = fromForm.ToLower();

				if (fromForm.Equals("administrative")) {
					this._isAdmin = true;
					return true;
				}
				else if (fromForm.Equals("standard"))
					return true;
			}

			return false;
		}

		private bool HasAccountStatus() {
			string fromForm = Core.WebServer.Request["Active"];

			if (!string.IsNullOrEmpty(fromForm)) {
				fromForm = fromForm.ToLower();

				if (fromForm.Equals("yes")) {
					this._isActive = true;
					return true;
				}
				else if (fromForm.Equals("no"))
					return true;
			}

			return false;
		}

	}

}