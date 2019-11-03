using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal abstract class Form {
		private string _id;
		private bool _postsToAjax;
		private string _postUrl;
		private string _onReset;
		private int _fieldWidths;
		private bool _hasTopSpace;
		private List<HiddenField> _hiddenFields;
		private List<Button> _buttons;

		public Form() {
			this._hiddenFields = new List<HiddenField>();
			this._buttons = new List<Button>();
		}

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public bool PostsToAjax {
			get {
				return this._postsToAjax;
			}
			set {
				this._postsToAjax = value;
			}
		}

		public string PostUrl {
			get {
				return this._postUrl;
			}
			set {
				this._postUrl = value;
			}
		}

		public string OnReset {
			get {
				return this._onReset;
			}
			set {
				this._onReset = value;
			}
		}

		public int FieldWidths {
			get {
				return this._fieldWidths;
			}
			set {
				this._fieldWidths = value;
			}
		}

		public bool HasTopSpace {
			get {
				return this._hasTopSpace;
			}
			set {
				this._hasTopSpace = value;
			}
		}

		public void AddHiddenField(string name, string id, string value) {
			HiddenField newHiddenField = new HiddenField();
			newHiddenField.Name = name;
			newHiddenField.Id = id;
			newHiddenField.Value = value;

			this._hiddenFields.Add(newHiddenField);
		}

		public void AddButton(string id, string name, string onMouseUp, int tabIndex, ButtonTypes buttonType) {
			Button newButton = new Button();
			newButton.Id = id;
			newButton.Name = name;
			newButton.OnMouseUp = onMouseUp;
			newButton.TabIndex = tabIndex;
			newButton.ButtonType = buttonType;

			this._buttons.Add(newButton);
		}

		public override string ToString() {
			Constructor form = new Constructor("/Templates/Elements/Forms/form.html");

			if (string.IsNullOrEmpty(this._id))
				form.DeleteVariable("Id");
			else
				form.SetVariable("Id", "id=\"Form" + this._id + "\" ");

			if (this._postsToAjax && !string.IsNullOrEmpty(this._postUrl))
				form.SetVariable("SubmitUrl", " action=\"javascript:alert('AJAX POST');\"");
			else if (!string.IsNullOrEmpty(this._postUrl))
				form.SetVariable("SubmitUrl", " action=\"?path=" + this._postUrl + "\"");
			else
				form.SetVariable("SubmitUrl", " action=\"#\"");

			if (string.IsNullOrEmpty(this._onReset))
				form.DeleteVariable("OnReset");
			else
				form.SetVariable("OnReset", " onreset=\"" + this._onReset + "\"");

			if (this._hasTopSpace)
				form.SetVariable("Class", " class=\"TopSpace\"");
			else
				form.DeleteVariable("Class");

			if (this._hiddenFields.Count.Equals(0))
				form.DeleteVariable("HiddenFields");
			else {
				string hiddenFieldStr = "<div>";

				foreach (HiddenField hiddenField in this._hiddenFields)
					hiddenFieldStr += hiddenField;

				form.SetVariable("HiddenFields", hiddenFieldStr + "</div>");
			}

			if (this._buttons.Count.Equals(0))
				form.DeleteVariable("Buttons");
			else {
				string buttonStr = "<div class=\"Buttons\">";

				foreach (Button button in this._buttons)
					buttonStr += button + "\n";

				form.SetVariable("Buttons", buttonStr + "</div>");
			}

			return form.ToString();
		}
	}

}