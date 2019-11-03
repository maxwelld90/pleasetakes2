using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.RightMenu {

	internal sealed class Link {
		private string _href;
		private string _image;
		private string _rollover;
		private string _hiddenText;

		public string Href {
			get {
				return this._href;
			}
			set {
				this._href = value;
			}
		}

		public string Image {
			get {
				return this._image;
			}
			set {
				this._image = value;
			}
		}

		public string Rollover {
			get {
				return this._rollover;
			}
			set {
				this._rollover = value;
			}
		}

		public string HiddenText {
			get {
				return this._hiddenText;
			}
			set {
				this._hiddenText = value;
			}
		}

		public override string ToString() {
			Constructor link = new Constructor("/Templates/Elements/Rightmenu/Link.html");
			link.SetVariable("Href", this._href);
			link.SetVariable("Image", this._image);
			link.SetVariable("Rollover", this._rollover);
			link.SetVariable("HiddenText", this._hiddenText);

			return link.ToString();
		}

	}

}