using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Persistence {

	internal sealed class Session : PersistenceBase {
		private UserManagement.Accounts.Account _account;
		private Schools.School _school;
		private bool _testedDatabaseConnection;
		private Dictionary<string, object> _temporaryStorage;

		public Session(Schools.School school) {
			this._school = school;
			this._temporaryStorage = new Dictionary<string, object>();
		}

		public bool IsLoggedIn {
			get {
				return WebServer.PleaseTakes.Session.CurrentInstance._account != null;
				//return (this._account != null); NASTY NASTY NASTY
			}
		}

		public UserManagement.Accounts.Account Account {
			get {
				return WebServer.PleaseTakes.Session.CurrentInstance._account;
				//return this._account;
			}
			set {
				WebServer.PleaseTakes.Session.CurrentInstance._account = value;
				//this._account = value;
			}
		}

		public Schools.School School {
			get {
				return this._school;
				// Does this need modifying too?
			}
		}

		public bool SessionSchoolExists {
			get {
				return (this._school != null);
			}
		}

		public Dictionary<string, object> TemporaryStorage {
			get {
				return this._temporaryStorage;
			}
		}

		// Need to reload school settings on creation of session.
		public void TestDatabaseConnection() {
			if (this.SessionSchoolExists && !this._testedDatabaseConnection) {
				Helpers.Database.Provider.TestConnection();
				this._testedDatabaseConnection = true;
			}
		}
	}

}