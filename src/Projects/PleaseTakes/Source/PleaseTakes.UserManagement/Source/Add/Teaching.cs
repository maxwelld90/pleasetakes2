using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Add {

	internal sealed class Teaching {
		
		public Teaching() {

			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Add/staff.sql", null)) {
				dataReader.Read();
				int newStaffId = (int)dataReader["NewTeachingStaffId"];

				Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + newStaffId + "/namenew/#Name");
			}
		}

	}

}
