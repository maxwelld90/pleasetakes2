using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Landing.Ajax.Teaching {

	internal sealed class Standard : Core.Helpers.BaseHandlers.AjaxHandler {

		public Standard(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string searchTerm = this.GetSearchTerm();
			string alertMessage = this.GetAlertMessage();

			Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", searchTerm);

			if (!string.IsNullOrEmpty(alertMessage))
				switch (alertMessage) {
					case "unknownteaching":
						Core.Helpers.Elements.Alerts.Alert unknownTeachingAlert = new Core.Helpers.Elements.Alerts.Alert("UnknownTeaching");
						unknownTeachingAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
						unknownTeachingAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Landing/Teaching/unknown.html").ToString();
						unknownTeachingAlert.NoScript = false;
						unknownTeachingAlert.ShowCloseBox = true;
						unknownTeachingAlert.StartHidden = false;

						this.Page.Contents = unknownTeachingAlert.ToString();
						break;
					case "inputteaching":
						Core.Helpers.Elements.Alerts.Alert invalidTeachingAlert = new Core.Helpers.Elements.Alerts.Alert("InputTeaching");
						invalidTeachingAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
						invalidTeachingAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Landing/Teaching/invalid.html").ToString();
						invalidTeachingAlert.NoScript = false;
						invalidTeachingAlert.ShowCloseBox = true;
						invalidTeachingAlert.StartHidden = false;

						this.Page.Contents = invalidTeachingAlert.ToString();
						break;
				}

			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Landing/teaching.sql", paramBuilder.Parameters)) {
				if (dataReader.HasRows) {
					Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();
					int recordCount = 0;

					while (dataReader.Read()) {
						Records.TeachingStaffMember teachingStaffRecord = new Records.TeachingStaffMember();
						StaffNameFormatter staffName = new StaffNameFormatter();
						staffName.Forename = dataReader["Forename"] as string;
						staffName.Surname = dataReader["Surname"] as string;
						staffName.HoldingName = dataReader["HoldingName"] as string;

						teachingStaffRecord.Id = (int)dataReader["StaffId"];
						teachingStaffRecord.Name = staffName;
						teachingStaffRecord.Department = dataReader["DepartmentName"] as string;

						recordCollection.Add(teachingStaffRecord.Record);
						recordCount++;
					}

					if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffTeachingCount"))
						Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingCount"] = recordCount;
					else
						Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("StaffTeachingCount", recordCount);

					this.Page.Contents += recordCollection.ToString();
				}
				else {
					Core.Helpers.Elements.Alerts.Alert noTeachingStaffAlert = new Core.Helpers.Elements.Alerts.Alert("NoTeachingStaff");
					noTeachingStaffAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
					noTeachingStaffAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Landing/Teaching/none.html").ToString();
					noTeachingStaffAlert.NoScript = false;
					noTeachingStaffAlert.ShowCloseBox = false;
					noTeachingStaffAlert.StartHidden = false;

					this.Page.Contents += noTeachingStaffAlert.ToString();
				}
			}
		}

		private string GetSearchTerm() {
			if (this.Path.HasNext())
				return this.Path.Next();

			return "";
		}

		private string GetAlertMessage() {
			Core.Helpers.Path.Parser sourcePath = new Core.Helpers.Path.Parser(Core.WebServer.Request["sourcepath"]);

			if (sourcePath.HasNext()) {
				sourcePath.Next();

				if (sourcePath.HasNext())
					return sourcePath.Next();
			}

			return null;
		}
	}

}