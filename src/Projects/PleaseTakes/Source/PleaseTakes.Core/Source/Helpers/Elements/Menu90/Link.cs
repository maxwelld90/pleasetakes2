using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Menu90 {

	internal sealed class Link {
		private string _href;
		private string _image;
		private string _title;
		private string _description;
		private string _rollover;

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

		public string Title {
			get {
				return this._title;
			}
			set {
				this._title = value;
			}
		}

		public string Description {
			get {
				return this._description;
			}
			set {
				this._description = value;
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

		public override string ToString() {
			Constructor link = new Constructor("/Templates/Elements/Menu90/link.html");
			link.SetVariable("Href", this._href);
			link.SetVariable("Image", this._image);
			link.SetVariable("Title", this._title);
			link.SetVariable("Description", this._description);
			link.SetVariable("Rollover", this._rollover);

			return link.ToString();
		}

	}

}