using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Add {

	internal sealed class Timetable : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _staffId;

		public Timetable(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			if (this.HasStaffId()) {
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Add/timetable.sql", paramBuilder.Parameters)) {
					dataReader.Read();

					switch ((int)dataReader["Status"]) {
						case 0:
							this.SetToBadStaffId();
							break;
						case 1:
							this.SetToTimetableExists();
							break;
						case 2:
							Core.WebServer.PleaseTakes.Redirect("/staff/modify/ajax/teaching/timetable/" + this._staffId + "/");
							break;
					}
				}
			}
			else
				this.SetToBadStaffId();
		}

		private void SetToBadStaffId() {
			Core.Helpers.Elements.Alerts.Alert badStaffIdAlert = new Core.Helpers.Elements.Alerts.Alert("BadStaffId");
			badStaffIdAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
			badStaffIdAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Timetable/badstaffid.html").ToString();

			this.Page.Contents = badStaffIdAlert.ToString();
		}

		private void SetToTimetableExists() {
			Core.Helpers.Constructor timetableExistsConstructor = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Timetable/timetableexists.html");
			timetableExistsConstructor.SetVariable("StaffId", this._staffId.ToString());

			Core.Helpers.Elements.Alerts.Alert timetableExistsAlert = new Core.Helpers.Elements.Alerts.Alert("TimetableExists");
			timetableExistsAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
			timetableExistsAlert.Message = timetableExistsConstructor.ToString();

			this.Page.Contents = timetableExistsAlert.ToString();
		}

		private bool HasStaffId() {
			if (this.Path.HasNext())
				return int.TryParse(this.Path.Next(), out this._staffId);

			return false;
		}

	}

}
