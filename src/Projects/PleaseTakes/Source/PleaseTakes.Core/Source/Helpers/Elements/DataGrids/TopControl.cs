using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids {

	internal sealed class TopControl {
		private TopControlPositions _position;
		private string _value;
		private string _href;
		private string _onClick;

		public TopControl(TopControlPositions position) {
			this._position = position;
		}

		public bool IsEmpty {
			get {
				return string.IsNullOrEmpty(this._value);
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
			Constructor text = new Constructor("/Templates/Elements/Datagrids/topcontrol.html");

			switch (this._position) {
				case TopControlPositions.Centre:
					text.SetVariable("Position", "Centre");
					break;
				case TopControlPositions.Left:
					text.SetVariable("Position", "Left");
					break;
				case TopControlPositions.Right:
					text.SetVariable("Position", "Right");
					break;
			}

			if (!(string.IsNullOrEmpty(this._href)) || !(string.IsNullOrEmpty(this._onClick))) {
				string linkStart = "<a";

				if ((string.IsNullOrEmpty(this._href)) && (!string.IsNullOrEmpty(this._onClick)))
					linkStart += " href=\"javascript:void(0);\"";

				if (!string.IsNullOrEmpty(this._href))
					linkStart += " href=\"" + this._href + "\"";

				if (!string.IsNullOrEmpty(this._onClick))
					linkStart += " onclick=\"" + this._onClick + "\"";

				linkStart += ">";

				text.SetVariable("LinkStart", linkStart);
				text.SetVariable("LinkEnd", "</a>");
			}
			else {
				text.DeleteVariable("LinkStart");
				text.DeleteVariable("LinkEnd");
			}

			if (string.IsNullOrEmpty(this._value))
				text.SetVariable("Value", "&nbsp;");
			else
				text.SetVariable("Value", this._value);

			return text.ToString();
		}
	}

}