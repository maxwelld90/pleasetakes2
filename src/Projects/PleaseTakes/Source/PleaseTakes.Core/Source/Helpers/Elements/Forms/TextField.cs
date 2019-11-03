using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class TextField : FormElement {
		private bool _isPassword;
		private bool _isItalic;
		private string _defaultValue;
		private int _width;
		private int _maxLength;

		public TextField(Form parent) {
			this._width = parent.FieldWidths;
		}

		public bool IsPassword {
			get {
				return this._isPassword;
			}
			set {
				this._isPassword = value;
			}
		}

		public bool IsItalic {
			get {
				return this._isItalic;
			}
			set {
				this._isItalic = value;
			}
		}

		public string DefaultValue {
			get {
				return this._defaultValue;
			}
			set {
				this._defaultValue = value;
			}
		}

		public int Width {
			get {
				return this._width;
			}
			set {
				this._width = value;
			}
		}

		public int MaxLength {
			get {
				return this._maxLength;
			}
			set {
				this._maxLength = value;
			}
		}

		public override string ToString() {
			Constructor textField = new Constructor("/Templates/Elements/Forms/textfield.html");
			string classes = "";

			if (this._isPassword) {
				textField.SetVariable("Type", "password");
				classes = "PasswordField ";
			}
			else
				textField.SetVariable("Type", "text");

			if (!this._isPassword && this._isItalic)
				classes += "Italic";

			textField.SetVariable("TabIndex", this.TabIndex.ToString());

			if (string.IsNullOrEmpty(this.Name))
				textField.DeleteVariable("Name");
			else
				textField.SetVariable("Name", "name=\"" + this.Name + "\" ");

			if (this._maxLength.Equals(0))
				textField.DeleteVariable("MaxLength");
			else
				textField.SetVariable("MaxLength", "maxlength=\"" + this._maxLength + "\" ");

			if (string.IsNullOrEmpty(this.Id))
				textField.DeleteVariable("Id");
			else
				textField.SetVariable("Id", "id=\"" + this.Id + "\" ");

			if (string.IsNullOrEmpty(this._defaultValue))
				textField.DeleteVariable("Value");
			else
				textField.SetVariable("Value", "value=\"" + this._defaultValue + "\" ");

			if (this._width.Equals(0))
				textField.DeleteVariable("Style");
			else
				textField.SetVariable("Style", "style=\"width: " + this._width + "px;\" ");

			if (string.IsNullOrEmpty(classes))
				textField.DeleteVariable("Class");
			else
				textField.SetVariable("Class", "class=\"" + classes.TrimEnd(' ') + "\" ");

			return textField.ToString();
		}
	}

}