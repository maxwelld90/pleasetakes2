using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.CoverSlips {

	internal sealed class StaffDetails {
		private string _mainRoom;
		private string _department;
		private UserManagement.StaffNameFormatter _name;

		public StaffDetails() {
			this._name = new UserManagement.StaffNameFormatter();
			this._name.AbbreviateForename = true;
			this._name.DisplayForenameFirst = true;
		}

		public string Forename {
			get {
				return this._name.Forename;
			}
			set {
				this._name.Forename = value;
			}
		}

		public string Surname {
			get {
				return this._name.Surname;
			}
			set {
				this._name.Surname = value;
			}
		}

		public string HoldingName {
			get {
				return this._name.HoldingName;
			}
			set {
				this._name.HoldingName = value;
			}
		}

		public UserManagement.StaffNameFormatter NameFormatter {
			get {
				return this._name;
			}
		}

		public string MainRoom {
			get {
				return this._mainRoom;
			}
			set {
				this._mainRoom = value;
			}
		}

		public string Department {
			get {
				return this._department;
			}
			set {
				this._department = value;
			}
		}
	}

}
