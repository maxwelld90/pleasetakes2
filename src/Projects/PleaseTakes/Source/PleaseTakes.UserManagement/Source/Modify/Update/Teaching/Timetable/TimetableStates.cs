﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching.Timetable {

	internal enum TimetableStates {
		Unavailable,
		Free,
		Teaching,
		Busy
	}

}