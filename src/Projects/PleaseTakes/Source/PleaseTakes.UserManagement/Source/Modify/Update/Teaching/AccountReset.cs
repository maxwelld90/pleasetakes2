using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching {

	internal sealed class AccountPasswordReset {
		private Core.Helpers.Path.Parser _path;
		private int _staffId;

		public AccountPasswordReset(Core.Helpers.Path.Parser path) {
			this._path = path;

			if (this.HasStaffId())
				this.DoUpdate();
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
		}

		private void DoUpdate() {
			string randomPassword = Core.Utils.GetRandomPassword();

			Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
			paramBuilder.AddParameter(SqlDbType.VarChar, "@Password", randomPassword);

			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Update/Teaching/accountreset.sql", paramBuilder.Parameters)) {
				dataReader.Read();

				switch ((int)dataReader["Status"]) {
					case 0:
						Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
						break;
					case 1:
						Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "resetnoaccount");
						Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
						break;
					case 2:
						if (Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.IsTeachingAccount) {
							Accounts.TeachingAccount account = (Accounts.TeachingAccount)Core.WebServer.PleaseTakes.Session.CurrentInstance.Account;

							if (account.StaffId.Equals(this._staffId))
								account.Password = randomPassword;
						}

						Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountPassword", randomPassword);
						Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "created");

						Core.Utils.SetTemporaryStorageObject("StaffTeachingAccountAlert", "resetsuccess");
						Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/#Account");
						break;
				}
			}
		}

		private bool HasStaffId() {
			if (this._path.HasNext())
				return int.TryParse(this._path.Next(), out this._staffId);

			return false;
		}

	}

}