using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class Button : FormElement {
		private ButtonTypes _buttonType;
		private string _onMouseUp;

		public ButtonTypes ButtonType {
			get {
				return this._buttonType;
			}
			set {
				this._buttonType = value;
			}
		}

		public string OnMouseUp {
			get {
				return this._onMouseUp;
			}
			set {
				this._onMouseUp = value;
			}
		}

		public override string ToString() {
			Constructor button = new Constructor("/Templates/Elements/Forms/button.html");

			button.SetVariable("Type", this._buttonType.ToString().ToLower());

			if (string.IsNullOrEmpty(this.Name))
				button.DeleteVariable("Name");
			else
				button.SetVariable("Name", "value=\"" + this.Name + "\" ");

			button.SetVariable("TabIndex", this.TabIndex.ToString());

			if (string.IsNullOrEmpty(this._onMouseUp))
				button.DeleteVariable("OnMouseUp");
			else
				button.SetVariable("OnMouseUp", " onmouseup=\"" + this._onMouseUp + "\"");

			return button.ToString();
		}
	}

}