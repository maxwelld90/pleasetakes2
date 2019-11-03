using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.RecordLists {

	internal sealed class Side {
		private Image _image;
		private string _mainText;
		private string _smallText;
		private bool _isOnLeftHandSide;

		public Side(bool isOnLeftHandSide) {
			this._image = new Image();
			this._isOnLeftHandSide = isOnLeftHandSide;
		}

		public void SetImage(string source, string toolTip) {
			this._image.Source = source;
			this._image.ToolTip = toolTip;
		}

		public Image Image {
			get {
				return this._image;
			}
		}

		public string MainText {
			get {
				return this._mainText;
			}
			set {
				this._mainText = value;
			}
		}

		public string SmallText {
			get {
				return this._smallText;
			}
			set {
				this._smallText = value;
			}
		}

		public bool IsEmpty {
			get {
				return ((this._image.IsEmpty) && (string.IsNullOrEmpty(this._mainText)) && (string.IsNullOrEmpty(this._smallText)));
			}
		}

		public override string ToString() {
			Constructor side = new Constructor("/Templates/Elements/Recordlists/side.html");

			if (this._image.IsEmpty)
				side.DeleteVariable("Image");
			else
				side.SetVariable("Image", this._image.ToString());

			if ((string.IsNullOrEmpty(this._mainText)) && (string.IsNullOrEmpty(this._smallText)))
				side.DeleteVariable("Text");
			else {
				Constructor text = new Constructor("/Templates/Elements/Recordlists/text.html");

				if (string.IsNullOrEmpty(this._mainText))
					text.DeleteVariable("MainText");
				else
					text.SetVariable("MainText", this._mainText);

				if (string.IsNullOrEmpty(this._smallText))
					text.DeleteVariable("SmallText");
				else
					text.SetVariable("SmallText", "<span>" + this._smallText + "</span>");

				side.SetVariable("Text", text.ToString());
			}

			if (this._isOnLeftHandSide)
				side.SetVariable("Side", "Left");
			else
				side.SetVariable("Side", "Right");

			return side.ToString();
		}
	}

}
