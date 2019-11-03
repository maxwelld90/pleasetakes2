using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace PleaseTakes.Core.BasicResponses.Account {

	internal sealed class PasswordChange {
		private string _old;
		private string _new;
		private string _confirm;

		public PasswordChange() {
			if (this.HasAllThreeFields() && this.OldPasswordMatches() && this.NewPasswordsMatch() && this.NewPasswordsConform())
				this.SetPassword();
			else
				Core.WebServer.PleaseTakes.Redirect("/account/passwordmissing/#Password");
		}

		private bool HasAllThreeFields() {
			this._old = Core.WebServer.Request["Old"];
			this._new = Core.WebServer.Request["New"];
			this._confirm = Core.WebServer.Request["Confirm"];

			return ((!string.IsNullOrEmpty(this._old)) && (!string.IsNullOrEmpty(this._new)) && (!string.IsNullOrEmpty(this._confirm)));
		}

		private bool OldPasswordMatches() {
			if (this._old.Equals(Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.Password))
				return true;

			Core.WebServer.PleaseTakes.Redirect("/account/passwordold/#Password");
			return false;
		}

		private bool NewPasswordsMatch() {
			if (this._new.Equals(this._confirm))
				return true;

			Core.WebServer.PleaseTakes.Redirect("/account/passwordmatch/#Password");
			return false;
		}

		private bool NewPasswordsConform() {
			if (this._new.Length >= Consts.MinimumPasswordLength)
				return true;

			Core.WebServer.PleaseTakes.Redirect("/account/passwordlength/#Password");
			return false;
		}

		private void SetPassword() {
			Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.Password = this._new;

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.Account.IsTeachingAccount) {
				Helpers.Database.ParameterBuilder paramBuilder = new Helpers.Database.ParameterBuilder();
				UserManagement.Accounts.TeachingAccount account = (UserManagement.Accounts.TeachingAccount)Core.WebServer.PleaseTakes.Session.CurrentInstance.Account;
				
				paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", account.StaffId);
				paramBuilder.AddParameter(SqlDbType.VarChar, "@NewPassword", account.Password);

				using (TransactionScope transactionScope = new TransactionScope()) {
					Helpers.Database.Provider.ExecuteNonQuery("/Sql/Specific/Account/password.sql", paramBuilder.Parameters);
					transactionScope.Complete();
				}
			}

			Core.WebServer.PleaseTakes.Redirect("/account/passwordsuccess/#Password");
		}
	}

}