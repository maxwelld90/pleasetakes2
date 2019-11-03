using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Accounts {

	internal sealed class TeachingAccount : Account {
		private string _title;
		private string _holdingName;
		private int _staffId;
		private int _timetableId;

		public TeachingAccount(string username, string password, string title, string forename, string surname, string holdingName, int staffId, int timetableId, bool isActive, bool isAdmin)
			: base(true, username, password, forename, surname, isActive, isAdmin) {
			this._username = username;
			this._password = password;
			this._title = title;
			this._forename = forename;
			this._surname = surname;
			this._holdingName = holdingName;
			this._staffId = staffId;
			this._timetableId = timetableId;
			this._isActive = isActive;
			this._isAdmin = isAdmin;
		}

		public override string Username {
			get {
				return this._username;
			}
			set {
				this._username = value;
			}
		}

		public override string Password {
			get {
				return this._password;
			}
			set {
				this._password = value;
			}
		}

		public override string Forename {
			get {
				return this._forename;
			}
			set {
				this._forename = value;
			}
		}

		public override string Surname {
			get {
				return this._surname;
			}
			set {
				this._surname = value;
			}
		}

		public override bool IsActive {
			get {
				return this._isActive;
			}
			set {
				this._isActive = value;
			}
		}

		public string Title {
			get {
				return this._title;
			}
			set {
				this._title = value;
			}
		}

		public string HoldingName {
			get {
				return this._holdingName;
			}
			set {
				this._holdingName = value;
			}
		}

		public int StaffId {
			get {
				return this._staffId;
			}
		}

		public int TimetableId {
			get {
				return this._timetableId;
			}
		}
	}

}