using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class HiddenField : FormElement {
		private string _value;

		public string Value {
			get {
				return this._value;
			}
			set {
				this._value = value;
			}
		}

		public override string ToString() {
			Constructor hiddenField = new Constructor("/Templates/Elements/Forms/hiddenfield.html");

			if (string.IsNullOrEmpty(this.Name))
				hiddenField.DeleteVariable("Name");
			else
				hiddenField.SetVariable("Name", "name=\"" + this.Name + "\" ");

			if (string.IsNullOrEmpty(this.Id))
				hiddenField.DeleteVariable("Id");
			else
				hiddenField.SetVariable("Id", "id=\"" + this.Id + "\" ");

			if (string.IsNullOrEmpty(this._value))
				hiddenField.DeleteVariable("Value");
			else
				hiddenField.SetVariable("Value", "value=\"" + this._value + "\" ");

			return hiddenField.ToString();
		}
	}

}