using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Search {

	internal sealed class SearchArea {
		private string _id;
		private string _ajaxUrl;
		private string _ajaxStatusUrl;
		private bool _getSourcePath;
		private List<Button> _buttons;
		private List<CustomField> _customFields;

		public SearchArea(string id) {
			this._id = id;
			this._buttons = new List<Button>();
			this._customFields = new List<CustomField>();
		}

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string AjaxUrl {
			get {
				return this._ajaxUrl;
			}
			set {
				this._ajaxUrl = value;
			}
		}

		public string AjaxStatusUrl {
			get {
				return this._ajaxStatusUrl;
			}
			set {
				this._ajaxStatusUrl = value;
			}
		}

		public bool GetSourcePath {
			get {
				return this._getSourcePath;
			}
			set {
				this._getSourcePath = value;
			}
		}

		public void AddButton(string imageUrl, string href, string onClick, string toolTip) {
			Button newButton = new Button();
			newButton.ImageUrl = "?path=/resources/images/search/" + imageUrl;
			newButton.Href = href;
			newButton.OnClick = onClick;
			newButton.ToolTip = toolTip;

			this._buttons.Add(newButton);
		}

		public void AddCustomField(string id, string value) {
			CustomField newCustomField = new CustomField();
			newCustomField.Id = id;
			newCustomField.Value = value;

			this._customFields.Add(newCustomField);
		}

		public override string ToString() {
			Constructor searchConstructor = new Constructor("/Templates/Elements/Search/searcharea.html");
			string customFields = "";
			string buttons = "";

			foreach (CustomField customField in this._customFields)
				customFields += customField;

			for (int i = 0; (i <= (this._buttons.Count - 1)); i++)
				if (i.Equals(this._buttons.Count - 1))
					buttons += this._buttons[i];
				else
					buttons += this._buttons[i] + "\n" + new string('\t', 12);

			searchConstructor.SetVariable("AjaxUrl", "?path=" + this._ajaxUrl);
			searchConstructor.SetVariable("AjaxStatusUrl", "?path=" + this._ajaxStatusUrl);
			searchConstructor.SetVariable("AjaxGetSourcePath", this._getSourcePath.ToString().ToLower());
			searchConstructor.SetVariable("CustomFields", customFields);
			searchConstructor.SetVariable("Buttons", buttons);
			searchConstructor.SetVariable("Id", this._id);

			return searchConstructor.ToString();
		}
	}

}
