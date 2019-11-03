using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Records {

	internal abstract class StaffRecordBase {
		protected Core.Helpers.Elements.RecordLists.Record _record;

		public StaffRecordBase() {
			this._record = new Core.Helpers.Elements.RecordLists.Record();
		}

		public StaffNameFormatter Name {
			set {
				this._record.LeftSide.MainText = value.ToString();
				value.IsToolTip = true;
				this._record.LeftSide.Image.ToolTip = value.ToString();
			}
		}

		public Core.Helpers.Elements.RecordLists.Record Record {
			get {
				return this._record;
			}
		}
	}

}