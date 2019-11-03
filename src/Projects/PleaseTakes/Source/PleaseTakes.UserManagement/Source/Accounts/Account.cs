using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Accounts {

	internal abstract class Account {
		protected bool _isTeachingAccount;
		protected string _username;
		protected string _password;
		protected string _forename;
		protected string _surname;
		protected bool _isActive;
		protected bool _isAdmin;

		public Account(bool isTeachingAccount, string username, string password, string forename, string surname, bool isActive, bool isAdmin) {
			Core.Validation.Specific.Account.Username(username);
			Core.Validation.Specific.Account.Password(password);
			Core.Validation.Specific.Account.Forename(forename);
			Core.Validation.Specific.Account.Surname(surname);

			this._isTeachingAccount = isTeachingAccount;
			this._isAdmin = isAdmin;
		}

		public bool IsTeachingAccount {
			get {
				return this._isTeachingAccount;
			}
		}

		public abstract string Username {
			get;
			set;
		}

		public abstract string Password {
			get;
			set;
		}

		public abstract string Forename {
			get;
			set;
		}

		public abstract string Surname {
			get;
			set;
		}

		public abstract bool IsActive {
			get;
			set;
		}

		public bool IsAdmin {
			get {
				return this._isAdmin;
			}
			set {
				this._isAdmin = value;
			}
		}
	}

}