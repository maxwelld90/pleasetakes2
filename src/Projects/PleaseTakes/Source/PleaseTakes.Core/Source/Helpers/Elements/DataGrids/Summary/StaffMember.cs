using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Summary {

	internal sealed class StaffMember {
		private int _id;
		private string _forename;
		private string _surname;
		private string _holdingName;

		public int Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string Forename {
			get {
				return this._forename;
			}
			set {
				this._forename = value;
			}
		}

		public string Surname {
			get {
				return this._surname;
			}
			set {
				this._surname = value;
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
	}

}