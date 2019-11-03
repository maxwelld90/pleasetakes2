using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching {

	internal sealed class Department : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _staffId;
		private int _departmentId;

		public Department(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.SetContentType("text/xml");
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			Core.Helpers.Elements.Ajax.Xml.Collection responseXml = new Core.Helpers.Elements.Ajax.Xml.Collection();

			if (this.HasStaffId() && this.HasDepartmentId()) {
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
				paramBuilder.AddParameter(SqlDbType.Int, "@DepartmentId", this._departmentId);

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Update/Teaching/department.sql", paramBuilder.Parameters)) {
					dataReader.Read();

					if (((int)dataReader["Status"]).Equals(2)) {
						dataReader.NextResult();

						while (dataReader.Read()) {
							responseXml.Add("Department" + (int)dataReader["DepartmentId"], dataReader["ClassName"] as string, null);
						}
					}
					else
						responseXml.Add("Department" + this._departmentId, "Yellow", null);
				}
			}

			this.Page.Contents = responseXml.ToString();
		}

		private bool HasStaffId() {
			if (this.Path.HasNext())
				return int.TryParse(this.Path.Next(), out this._staffId);

			return false;
		}

		private bool HasDepartmentId() {
			if (this.Path.HasNext())
				return int.TryParse(this.Path.Next(), out this._departmentId);

			return false;
		}
	}

}