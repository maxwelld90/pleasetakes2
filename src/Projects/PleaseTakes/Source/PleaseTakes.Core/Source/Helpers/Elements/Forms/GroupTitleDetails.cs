using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class GroupTitleDetails {
		private Group _parent;
		private GroupTitleImageDetails _image;
		private string _text;

		public GroupTitleDetails(Group parent) {
			this._parent = parent;
			this._image = new GroupTitleImageDetails();
		}

		public GroupTitleImageDetails Image {
			get {
				return this._image;
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

		public override string ToString() {
			Constructor groupTitleDetails = new Constructor("/Templates/Elements/Forms/grouptitle.html");

			string formId = this._parent.Parent.Id;
			string groupName = this._parent.Name;

			groupTitleDetails.SetVariable("FormId", this._parent.Parent.Id);
			groupTitleDetails.SetVariable("GroupName", this._parent.Name);

			if (this._parent.IsDefault)
				groupTitleDetails.SetVariable("IsDefault", " checked=\"checked\"");
			else
				groupTitleDetails.DeleteVariable("IsDefault");

			if (this._image.IsDefined)
				groupTitleDetails.SetVariable("Image", this._image.ToString());
			else
				groupTitleDetails.DeleteVariable("Image");

			groupTitleDetails.SetVariable("RadioGroupName", this._parent.Parent.RadioGroupName);
			groupTitleDetails.SetVariable("RadioValue", this._parent.RadioValue);

			if (string.IsNullOrEmpty(this._text))
				groupTitleDetails.SetVariable("Text", "&nbsp;");
			else
				groupTitleDetails.SetVariable("Text", this._text);

			return groupTitleDetails.ToString();
		}
	}

}