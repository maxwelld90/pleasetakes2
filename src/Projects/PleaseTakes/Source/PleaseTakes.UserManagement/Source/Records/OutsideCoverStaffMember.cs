using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Records {

	internal sealed class OutsideCoverStaffMember : StaffRecordWithIdBase {

		public OutsideCoverStaffMember() {
			this._record.LeftSide.Image.Source = "dudettedoor.png";
		}

		public override int Id {
			set {
				this._record.Href = "?path=/staff/modify/outside/" + value + "/";
			}
		}

	}

}