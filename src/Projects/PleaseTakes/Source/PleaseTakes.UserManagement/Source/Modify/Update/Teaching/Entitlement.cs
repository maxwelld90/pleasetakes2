using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching {

	internal sealed class Entitlement {
		private int _staffId;
		private int _entitlement;

		public Entitlement() {
			if (this.HasStaffId()) {
				if (this.SetEntitlement())
					this.DoUpdate();
				else
					Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/entitlementbad/#Entitlement");
			}
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
		}

		private void DoUpdate() {
			Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
			paramBuilder.AddParameter(SqlDbType.Int, "@Entitlement", this._entitlement);

			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Update/Teaching/entitlement.sql", paramBuilder.Parameters)) {
				dataReader.Read();

				if ((bool)dataReader["Status"])
					Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/entitlementsuccess/#Entitlement");
				else
					Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");
			}
		}

		private bool HasStaffId() {
			string fromForm = Core.WebServer.Request["StaffId"];

			if (int.TryParse(fromForm, out this._staffId))
				return true;

			return false;
		}

		private bool SetEntitlement() {
			string fromForm = Core.WebServer.Request["Entitlement"];

			if ((int.TryParse(fromForm, out this._entitlement)) && (this._entitlement >= 0) && (this._entitlement <= Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoPeriods))
				return true;

			return false;
		}

	}

}