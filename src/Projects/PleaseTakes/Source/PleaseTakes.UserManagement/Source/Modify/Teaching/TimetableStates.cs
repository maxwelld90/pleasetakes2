using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.UserManagement.Modify.Teaching {

	internal enum TimetableStates{
		Unavailable,
		Free,
		Teaching,
		Busy
	}

}