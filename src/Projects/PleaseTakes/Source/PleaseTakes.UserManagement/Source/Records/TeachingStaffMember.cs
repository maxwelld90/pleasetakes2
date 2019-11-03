using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Records {

	internal sealed class TeachingStaffMember : StaffRecordWithIdBase {

		public TeachingStaffMember() {
			this._record.LeftSide.Image.Source = "dude.png";
		}

		public override int Id {
			set {
				this._record.Href = "?path=/staff/modify/teaching/" + value + "/";
			}
		}

		public string Department {
			set {
				if (string.IsNullOrEmpty(value)) {
					this._record.RightSide.Image.Source = null;
					this._record.RightSide.Image.ToolTip = null;
					this._record.RightSide.MainText = null;
				}
				else {
					this._record.RightSide.Image.Source = "books.png";
					this._record.RightSide.Image.ToolTip = value;
					this._record.RightSide.MainText = value;
				}
			}
		}

	}

}