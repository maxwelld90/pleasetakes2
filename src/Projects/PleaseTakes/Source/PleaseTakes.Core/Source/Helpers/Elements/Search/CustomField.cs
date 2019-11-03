using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Search {

	internal sealed class CustomField {
		private string _id;
		private string _value;

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string Value {
			get {
				return this._value;
			}
			set {
				this._value = value;
			}
		}

		public override string ToString() {
			return "<input type=\"hidden\" id=\"AjaxCustom" + this._id + "{$Id}\" value=\"" + this._value + "\" />";
		}
	}

}
