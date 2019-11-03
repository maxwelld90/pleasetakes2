using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching {

	internal sealed class Name {
		private int _staffId;
		private StaffNameFormatter _name;
		
		public Name() {
			if (this.HasStaffId()) {
				this.PopulateNameFormatter();

				if (this.AreAllNameFieldsBlank())
					Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/nameallblank/");
				else {
					if (this.IsProperNameBlank())
						this.DoUpdate();
					else
						if (this.IsProperNameIncomplete())
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/nameincomplete/");
						else
							this.DoUpdate();
				}
			}
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
		}

		private void DoUpdate() {
			Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
			paramBuilder.AddParameter(SqlDbType.VarChar, "@Title", this._name.Title);
			paramBuilder.AddParameter(SqlDbType.VarChar, "@Forename", this._name.Forename);
			paramBuilder.AddParameter(SqlDbType.VarChar, "@Surname", this._name.Surname);
			paramBuilder.AddParameter(SqlDbType.VarChar, "@HoldingName", this._name.HoldingName);

			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Update/Teaching/name.sql", paramBuilder.Parameters)) {
				dataReader.Read();

				if ((bool)dataReader["Status"]) {
					if (Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.IsTeachingAccount) {
						UserManagement.Accounts.TeachingAccount account = (UserManagement.Accounts.TeachingAccount)Core.WebServer.PleaseTakes.Session.CurrentInstance.Account;

						if (account.StaffId.Equals(this._staffId)) {
							account.Title = this._name.Title;
							account.Forename = this._name.Forename;
							account.Surname = this._name.Surname;
							account.HoldingName = this._name.HoldingName;
						}
					}

					Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/namesuccess/");
				}
				else
					Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
			}
		}

		private bool HasStaffId() {
			string fromForm = Core.WebServer.Request["StaffId"];

			if (int.TryParse(fromForm, out this._staffId))
				return true;

			return false;
		}

		private void PopulateNameFormatter() {
			this._name = new StaffNameFormatter();
			this._name.Title = Core.WebServer.Request["Title"];
			this._name.Forename = Core.WebServer.Request["Forename"];
			this._name.Surname = Core.WebServer.Request["Surname"];
			this._name.HoldingName = Core.WebServer.Request["HoldingName"];
		}

		private bool AreAllNameFieldsBlank() {
			return ((string.IsNullOrEmpty(this._name.Title)) && (string.IsNullOrEmpty(this._name.Forename)) && (string.IsNullOrEmpty(this._name.Surname)) && (string.IsNullOrEmpty(this._name.HoldingName)));
		}

		private bool IsProperNameBlank() {
			return ((string.IsNullOrEmpty(this._name.Title) && (string.IsNullOrEmpty(this._name.Forename)) && (string.IsNullOrEmpty(this._name.Surname))));
		}

		private bool IsProperNameIncomplete() {
			return ((string.IsNullOrEmpty(this._name.Title) || (string.IsNullOrEmpty(this._name.Forename)) || (string.IsNullOrEmpty(this._name.Surname))));
		}

	}

}