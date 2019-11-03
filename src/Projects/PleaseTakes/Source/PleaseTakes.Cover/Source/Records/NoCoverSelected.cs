using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Records {

	internal sealed class NoCoverSelected : RequestBase {

		public NoCoverSelected() {
			this._record.Colour = Core.Helpers.Elements.RecordLists.Colours.Red;
			this._record.RightSide.Image.Source = "cross.png";
			this._record.RightSide.Image.ToolTip = "No cover has been selected for this request.";
			this._record.RightSide.MainText = "<strong>No cover selected</strong>";
			this._record.RightSide.SmallText = "Click here to select a member of staff";
		}

		public string Id {
			get {
				return this.Record.Id;
			}
			set {
				this.Record.Id = value;
			}
		}

	}

}