using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Landing.Ajax.Outside {

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

			if ((!string.IsNullOrEmpty(alertMessage)) && (alertMessage.Equals("unknownoutside"))) {
				Core.Helpers.Elements.Alerts.Alert unknownTeachingAlert = new Core.Helpers.Elements.Alerts.Alert("UnknownTeaching");
				unknownTeachingAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
				unknownTeachingAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Landing/Outside/unknown.html").ToString();
				unknownTeachingAlert.NoScript = false;
				unknownTeachingAlert.ShowCloseBox = true;
				unknownTeachingAlert.StartHidden = false;

				this.Page.Contents = unknownTeachingAlert.ToString();
			}

			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Landing/outside.sql", paramBuilder.Parameters)) {
				if (dataReader.HasRows) {
					Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();
					int recordCount = 0;

					while (dataReader.Read()) {
						Records.OutsideCoverStaffMember outsideCoverStaffRecord = new Records.OutsideCoverStaffMember();
						StaffNameFormatter staffName = new StaffNameFormatter();
						staffName.Forename = dataReader["Forename"] as string;
						staffName.Surname = dataReader["Surname"] as string;

						outsideCoverStaffRecord.Id = (int)dataReader["OutsideCoverStaffId"];
						outsideCoverStaffRecord.Name = staffName;

						recordCollection.Add(outsideCoverStaffRecord.Record);
						recordCount++;
					}

					if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffOutsideCount"))
						Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffOutsideCount"] = recordCount;
					else
						Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("StaffOutsideCount", recordCount);

					this.Page.Contents += recordCollection.ToString();
				}
				else {
					Core.Helpers.Elements.Alerts.Alert noOutsideStaffAlert = new Core.Helpers.Elements.Alerts.Alert("NoOutsideStaff");
					noOutsideStaffAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
					noOutsideStaffAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Landing/Outside/none.html").ToString();
					noOutsideStaffAlert.NoScript = false;
					noOutsideStaffAlert.ShowCloseBox = false;
					noOutsideStaffAlert.StartHidden = false;

					this.Page.Contents += noOutsideStaffAlert.ToString();
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