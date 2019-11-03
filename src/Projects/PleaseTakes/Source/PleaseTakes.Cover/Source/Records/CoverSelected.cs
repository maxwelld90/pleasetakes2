using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Records {

	internal sealed class CoverSelected : RequestBase {
		private string _coverStaffName;

		public CoverSelected() {
			this._record.Colour = Core.Helpers.Elements.RecordLists.Colours.Green;
			this._record.RightSide.MainText = "<strong>No cover selected</strong>";
			this._record.RightSide.SmallText = "Click here to select a member of staff";
		}

		public UserManagement.StaffNameFormatter CoverStaffName {
			set {
				this._coverStaffName = value.ToString();
				this._record.RightSide.MainText = "<strong>" + value.ToString() + "</strong>";
			}
		}

		public string Id {
			get {
				return this.Record.Id;
			}
			set {
				this.Record.Id = value;
			}
		}

		public bool IsInternalCover {
			set {
				if (value) {
					this._record.RightSide.Image.Source = "group.png";
					this._record.RightSide.Image.ToolTip = this._coverStaffName;
					this._record.RightSide.SmallText = "Internal cover arranged";
				}
				else {
					this._record.RightSide.Image.Source = "dudettedoor.png";
					this._record.RightSide.Image.ToolTip = this._coverStaffName;
					this._record.RightSide.SmallText = "Outside cover arranged";
				}
			}
		}

	}

}