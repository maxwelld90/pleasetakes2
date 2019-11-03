using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Breadcrumbs {

	internal sealed class Breadcrumb {
		private string _displayText;
		private string _href;
		private string _onClick;

		public string DisplayText {
			get {
				return this._displayText;
			}
			set {
				this._displayText = value;
			}
		}

		public string Href {
			get {
				return this._href;
			}
			set {
				this._href = value;
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
			string returnStr = "<li>";

			if (!((string.IsNullOrEmpty(this._href)) && (string.IsNullOrEmpty(this._onClick)))) {
				returnStr += "<a";

				if (string.IsNullOrEmpty(this._href))
					returnStr += " href=\"javascript:void(0);\"";
				else
					returnStr += " href=\"" + this._href + "\"";

				if (!string.IsNullOrEmpty(this._onClick))
					returnStr += " onclick=\"" + this._onClick + "\"";

				returnStr += ">" + this._displayText + "</a>";
			}
			else
				returnStr += this._displayText;

			return returnStr += Consts.BreadcrumbSeparator + "</li>";
		}
	}

}