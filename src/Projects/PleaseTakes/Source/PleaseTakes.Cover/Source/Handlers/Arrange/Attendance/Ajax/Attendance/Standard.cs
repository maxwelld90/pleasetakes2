using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Attendance.Ajax.Attendance {

	internal sealed class Standard : Core.Helpers.BaseHandlers.AjaxHandler {
		private DateTime _selectedDate;
		private int _timetableWeek;
		private int _periods;

		public Standard(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {

			if (this.CheckInputValidity()) {
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
				paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)(this._selectedDate.DayOfWeek + 1));
				paramBuilder.AddParameter(SqlDbType.Int, "@MaxPeriods", this._periods);
				paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@Date", this._selectedDate);

				if (this.Path.HasNext())
					paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", this.Path.Next());
				else
					paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", "");

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/Arrange/Attendance/Attendance/attendance.sql", paramBuilder.Parameters)) {
					if (dataReader.HasRows) {
						Core.Helpers.Elements.RecordLists.Collection collection = new Core.Helpers.Elements.RecordLists.Collection();
						collection.Id = "Attendance";
						int recordCount = 0;

						while (dataReader.Read()) {
							Core.Helpers.Elements.RecordLists.Record record = new Core.Helpers.Elements.RecordLists.Record();

							int staffId = (int)dataReader["StaffId"];

							UserManagement.StaffNameFormatter nameFormatter = new UserManagement.StaffNameFormatter();
							nameFormatter.Forename = dataReader["Forename"] as string;
							nameFormatter.Surname = dataReader["Surname"] as string;
							nameFormatter.HoldingName = dataReader["HoldingName"] as string;

							int attendanceRecord = (int)dataReader["AttendanceRecord"];
							record.Id = "Staff" + staffId.ToString();

							if (attendanceRecord > 0)
								record.Colour = Core.Helpers.Elements.RecordLists.Colours.Red;

							record.LeftSide.MainText = nameFormatter.ToString();
							nameFormatter.IsToolTip = true;
							record.LeftSide.SetImage("dude.png", nameFormatter.ToString());
							record.OnClick = "getResponse('AttendanceStaff" + staffId + "', '?path=/cover/arrange/attendance/ajax/attendance/modify/" + this._selectedDate.Year.ToString() + "/" + this._selectedDate.Month.ToString() + "/" + this._selectedDate.Day.ToString() + "/" + staffId + "/', false, false, true); resetSearch('Periods');";
							collection.Add(record);
							recordCount++;
						}

						if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("ArrangeAttendanceCount"))
							Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangeAttendanceCount"] = recordCount;
						else
							Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("ArrangeAttendanceCount", recordCount);

						this.Page.Contents = collection.ToString();
					}
					else {
						Core.Helpers.Elements.Alerts.Alert alert = new Core.Helpers.Elements.Alerts.Alert("NoResults");
						alert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
						alert.NoScript = false;
						alert.ShowCloseBox = false;
						alert.StartHidden = false;
						alert.Message = new Core.Helpers.Constructor("/Alerts/Ajax/noresults.html").ToString();

						this.Page.Contents = alert.ToString();
					}
				}

			}
			else {
				Core.Helpers.Elements.Alerts.Alert alert = new Core.Helpers.Elements.Alerts.Alert("Error");
				alert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
				alert.NoScript = false;
				alert.ShowCloseBox = false;
				alert.StartHidden = false;
				alert.Message = new Core.Helpers.Constructor("/Alerts/Ajax/error.html").ToString();

				this.Page.Contents = alert.ToString();
			}
		}

		private bool CheckInputValidity() {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

			this.Path.Reset();

			for (int i = 0; i <= 4; i++)
				this.Path.Next();

			for (int i = 0; i <= 2; i++) {
				if (this.Path.HasNext())
					switch (i) {
						case 0:
							validYear = int.TryParse(this.Path.Next(), out year);
							break;
						case 1:
							validMonth = int.TryParse(this.Path.Next(), out month);
							break;
						case 2:
							validDay = int.TryParse(this.Path.Next(), out day);
							break;
					}
				else
					return false;
			}

			if ((!validYear) || (!validMonth) || (!validDay))
				return false;
			else {
				try {
					year.RequireThat("year").IsInRange(2000, 3000);
					month.RequireThat("month").IsInRange(1, 12);
					day.RequireThat("day").IsInRange(1, DateTime.DaysInMonth(year, month));

					if (Standard.IsDateInSessionRange(year, month, day)) {
						DateTime selectedDate = new DateTime(year, month, day);
						this._timetableWeek = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(selectedDate).TimetableWeek;

						try {
							this._periods = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this._timetableWeek][(int)selectedDate.DayOfWeek + 1];

							if (this._periods.Equals(0))
								return false;
							else {
								this._selectedDate = selectedDate;
								return true;
							}
						}
						catch (Core.Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) {
							return false;
						}
					}
					else
						return false;
				}
				catch (IndexOutOfRangeException) {
					return false;
				}
			}
		}

		private static bool IsDateInSessionRange(int year, int month, int day) {
			DateTime date = new DateTime(year, month, day);
			DateTime low = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.StartDate;
			DateTime high = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.EndDate;

			if (Core.Utils.IsDateInRange(low, high, date))
				return true;

			return false;
		}

	}

}
