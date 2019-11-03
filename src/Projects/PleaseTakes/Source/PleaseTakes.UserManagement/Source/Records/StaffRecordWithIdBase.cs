using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Records {

	internal abstract class StaffRecordWithIdBase : StaffRecordBase {

		public abstract int Id {
			set;
		}

	}

}