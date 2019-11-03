using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Records {

	internal sealed class NoCoverRequired : RequestBase {

		public NoCoverRequired() {
			this._record.RightSide.Image.Source = "thumbsup.png";
			this._record.RightSide.Image.ToolTip = "S'all good!";
			this._record.RightSide.MainText = "<strong>No cover required</strong>";
		}

		public bool IsYeargroupAway {
			set {
				if (value)
					this._record.RightSide.SmallText = "Yeargroup is away";
				else
					this._record.RightSide.SmallText = null;
			}
		}

		public bool IsNotTeaching {
			set {
				if (value)
					this._record.RightSide.SmallText = "Not teaching";
				else
					this._record.RightSide.SmallText = null;
			}
		}

	}

}