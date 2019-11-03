using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class GroupTitleImageDetails {
		private string _src;
		private string _tooltip;

		public string Src {
			get {
				return this._src;
			}
			set {
				this._src = value;
			}
		}

		public string Tooltip {
			get {
				return this._tooltip;
			}
			set {
				this._tooltip = value;
			}
		}

		public bool IsDefined {
			get {
				return !string.IsNullOrEmpty(this._src);
			}
		}

		public override string ToString() {
			Constructor image = new Constructor("/Templates/Elements/Forms/grouptitleimage.html");
			image.SetVariable("Src", "?path=/resources/images/recordlists/" + this._src);
			image.SetVariable("Tooltip", this._tooltip);

			return image.ToString();
		}
	}

}