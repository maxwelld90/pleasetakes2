using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.UserManagement.Modify.Teaching {

	internal sealed partial class TimetableText {
		private string _highlightedId;
		private string _highlighted;
		private string _onClick;

		public string HighlightedId {
			get {
				return this._highlightedId;
			}
			set {
				this._highlightedId = value;
			}
		}

		public string Highlighted {
			get {
				return this._highlighted;
			}
			set {
				this._highlighted = value;
			}
		}

		public string OnClick {
			get {
				return this._onClick;
			}
			set {
				this._onClick = value;
			}
		}

		public override string ToString() {
			Core.Helpers.Constructor timetableText = new Core.Helpers.Constructor("/Templates/Specific/Staff/Modify/Teaching/Timetable/timetabletext.html");

			timetableText.SetVariable("HighlightedId", this._highlightedId);
			timetableText.SetVariable("Highlighted", this._highlighted);

			if (string.IsNullOrEmpty(this._onClick))
				timetableText.DeleteVariable("ChangeLink");
			else
				timetableText.SetVariable("ChangeLink", "&nbsp;<a href=\"javascript:void(0);\" onclick=\"" + this._onClick + "\"><em style=\"font-size: 8pt;\">(Change)</em></a>");

			return timetableText.ToString();
		}
	}

}