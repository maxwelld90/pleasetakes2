using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.MenuBar {

	internal sealed class Button {
		private string _src;
		private string _toolTip;
		private string _href;
		private string _onClick;
		private bool _hasLeftMargin = true;

		public string Src {
			get {
				return this._src;
			}
			set {
				this._src = value;
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

		public bool HasLeftMargin {
			get {
				return this._hasLeftMargin;
			}
			set {
				this._hasLeftMargin = value;
			}
		}

		public override string ToString() {
			Constructor button = new Constructor("/Templates/Elements/Menubar/button.html");
			button.SetVariable("Src", "?path=/resources/images/search/" + this._src);
			button.SetVariable("ToolTip", this._toolTip);

			if ((!string.IsNullOrEmpty(this._href)) || (!string.IsNullOrEmpty(this._onClick))) {
				if ((!string.IsNullOrEmpty(this._href)) && (!string.IsNullOrEmpty(this._onClick))) {
					button.SetVariable("Href", " href=\"" + this._href + "\"");
					button.SetVariable("OnClick", " onclick=\"" + this._onClick + "\"");
				}
				else if (!string.IsNullOrEmpty(this._onClick)) {
					button.SetVariable("Href", " href=\"javascript:void(0);\"");
					button.SetVariable("OnClick", " onclick=\"" + this._onClick + "\"");
				}
				else {
					button.SetVariable("Href", " href=\"" + this._href + "\"");
					button.DeleteVariable("OnClick");
				}
			}
			else {
				button.SetVariable("Href", " href=\"javascript:void(0);\"");
				button.DeleteVariable("OnClick");
			}

			if (this._hasLeftMargin)
				button.DeleteVariable("Style");
			else
				button.SetVariable("Style", " style=\"margin-left: 0;\"");

			return button.ToString();
		}
	}

}