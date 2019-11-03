using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class RadioButton : FormElement {
		private RadioGroup _parent;
		private bool _isChecked;
		private string _label;
		private string _onClick;
		private string _value;

		public RadioButton(RadioGroup parent) {
			this._parent = parent;
		}

		public bool IsChecked {
			get {
				return this._isChecked;
			}
			set {
				this._isChecked = value;
			}
		}

		public string Label {
			get {
				return this._label;
			}
			set {
				this._label = value;
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

		public string Value {
			get {
				return this._value;
			}
			set {
				this._value = value;
			}
		}

		public override string ToString() {
			Constructor radioButton = new Constructor("/Templates/Elements/Forms/radiobutton.html");

			radioButton.SetVariable("FormName", this._parent.Parent.Id);
			radioButton.SetVariable("GroupName", this._parent.Name);
			radioButton.SetVariable("Value", this._value);
			radioButton.SetVariable("Label", this._label);

			if (this._isChecked)
				radioButton.SetVariable("Checked", " checked=\"checked\"");
			else
				radioButton.DeleteVariable("Checked");

			if (string.IsNullOrEmpty(this._onClick))
				radioButton.DeleteVariable("OnClick");
			else
				radioButton.SetVariable("OnClick", " " + this._onClick);

			return radioButton.ToString();
		}
	}

}