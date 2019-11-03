using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class Row {
		public Form _parent;
		private string _description;
		private string _text;
		private FormElement _formElement;

		public Row(Form parent) {
			this._parent = parent;
		}

		public string Description {
			get {
				return this._description;
			}
			set {
				this._description = value;
			}
		}

		public string Text {
			get {
				return this._text;
			}
			set {
				this._text = value;
			}
		}

		public FormElement FormElement {
			get {
				return this._formElement;
			}
			set {
				this._formElement = value;
			}
		}

		public void SetToTextField(string name, string id, string defaultValue, int tabIndex, int maxLength, bool isPassword, bool isItalic) {
			TextField newTextField = new TextField(this._parent);
			newTextField.Name = name;
			newTextField.Id = id;
			newTextField.DefaultValue = defaultValue;
			newTextField.TabIndex = tabIndex;
			newTextField.MaxLength = maxLength;
			newTextField.IsPassword = isPassword;
			newTextField.IsItalic = isItalic;

			this._formElement = newTextField;
		}

		public void SetToRadioGroup() {
			RadioGroup newRadioGroup = new RadioGroup(this._parent);

			this._formElement = newRadioGroup;
		}

		public override string ToString() {
			Constructor row = new Constructor("/Templates/Elements/Forms/row.html");

			if (string.IsNullOrEmpty(this._description))
				row.SetVariable("Description", "&nbsp;");
			else
				row.SetVariable("Description", this._description);

			if ((this._formElement == null) && (string.IsNullOrEmpty(this._text)))
				row.SetVariable("Contents", "&nbsp;");
			else
				if (this._formElement != null)
					row.SetVariable("Contents", this._formElement.ToString());
				else
					row.SetVariable("Contents", "<span class=\"NoInput\">" + this._text + "</span>");

			return row.ToString();
		}
	}

}