using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Records {

	internal sealed class NonTeachingStaffMember : StaffRecordBase {

		public NonTeachingStaffMember() {
			this._record.LeftSide.Image.Source = "dudette.png";
		}

		public string Username {
			set {
				this._record.Href = "?path=/staff/modify/nonteaching/" + value + "/";
			}
		}

		public bool IsAccountActive {
			set {
				if (value)
					this._record.Colour = Core.Helpers.Elements.RecordLists.Colours.Blue;
				else
					this._record.Colour = Core.Helpers.Elements.RecordLists.Colours.Red;
			}
		}

	}

}