using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class RadioGroup : FormElement {
		private Form _parent;
		private List<RadioButton> _buttons;

		public RadioGroup(Form parent) {
			this._parent = parent;
			this._buttons = new List<RadioButton>();
		}

		public Form Parent {
			get {
				return this._parent;
			}
		}

		public RadioButton AddRadioButton() {
			RadioButton newRadioButton = new RadioButton(this);
			this._buttons.Add(newRadioButton);
			return newRadioButton;
		}

		public override string ToString() {			
			string returnStr = "";

			for (int i = (this._buttons.Count - 1); i >= 0; i--)
				returnStr += this._buttons[i];

			return returnStr;
		}

	}

}