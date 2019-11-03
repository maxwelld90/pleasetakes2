using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.RecordLists {

	internal sealed class Image {
		private string _source;
		private string _toolTip;

		public string Source {
			get {
				return this._source;
			}
			set {
				this._source = value;
			}
		}

		public string ToolTip {
			get {
				return this._toolTip;
			}
			set {
				this._toolTip = value;
			}
		}

		public bool IsEmpty {
			get {
				return ((string.IsNullOrEmpty(this._source)) && (string.IsNullOrEmpty(this._toolTip)));
			}
		}

		public override string ToString() {
			Constructor image = new Constructor("/Templates/Elements/Recordlists/image.html");
			image.SetVariable("Source", "?path=/resources/images/recordlists/" + this._source);
			image.SetVariable("ToolTip", this._toolTip);

			return image.ToString();
		}
	}

}