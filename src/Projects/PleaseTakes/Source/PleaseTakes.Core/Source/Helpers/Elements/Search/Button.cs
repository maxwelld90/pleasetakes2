using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Search {

	internal sealed class Button {
		private string _imageUrl;
		private string _href;
		private string _onClick;
		private string _toolTip;

		public string ImageUrl {
			get {
				return this._imageUrl;
			}
			set {
				this._imageUrl = value;
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

		public string ToolTip {
			get {
				return this._toolTip;
			}
			set {
				this._toolTip = value;
			}
		}

		public override string ToString() {
			string returnStr = "<a";

			if (string.IsNullOrEmpty(this._href))
				returnStr += " href=\"javascript:void(0);\"";
			else
				returnStr += " href=\"" + this._href + "\"";

			if (!string.IsNullOrEmpty(this._onClick))
				returnStr += " onclick=\"" + this._onClick + "\"";

			return returnStr += "><img src=\"" + this._imageUrl + "\" alt=\"" + this._toolTip + "\" title=\"" + this._toolTip + "\" style=\"width:25px; height: 25px;\" /></a>";
		}
	}

}
