using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Attendance.Ajax.Periods {

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
				Core.Helpers.Elements.DataGrids.Summary.Summary summary = new Core.Helpers.Elements.DataGrids.Summary.Summary(this._selectedDate);

				if (this.Path.HasNext())
					summary.SearchTerm = this.Path.Next();

				summary.Id = "Periods";

				summary.GenericCells.StaffName.Id = "StaffId";
				summary.GenericCells.PresentAndFree.Id = "StaffId-Period";
				summary.GenericCells.PresentAndFreeYeargroupAway.Id = "StaffId-Period";
				summary.GenericCells.PresentIsBusy.Id = "StaffId-Period";
				summary.GenericCells.Absent.Id = "StaffId-Period";

				summary.GenericCells.StaffName.OnClick = "getResponse('AttendanceStaffStaffId', '?path=/cover/arrange/attendance/ajax/periods/modify/Year/Month/Day/StaffId/', false, false, true);";
				summary.GenericCells.PresentAndFree.OnClick = "getResponse('AttendanceStaffStaffId', '?path=/cover/arrange/attendance/ajax/periods/modify/Year/Month/Day/StaffId/Period/', false, false, true);";
				summary.GenericCells.PresentAndFreeYeargroupAway.OnClick = "getResponse('AttendanceStaffStaffId', '?path=/cover/arrange/attendance/ajax/periods/modify/Year/Month/Day/StaffId/Period/', false, false, true);";
				summary.GenericCells.PresentIsBusy.OnClick = "getResponse('AttendanceStaffStaffId', '?path=/cover/arrange/attendance/ajax/periods/modify/Year/Month/Day/StaffId/Period/', false, false, true);";
				summary.GenericCells.Absent.OnClick = "getResponse('AttendanceStaffStaffId', '?path=/cover/arrange/attendance/ajax/periods/modify/Year/Month/Day/StaffId/Period/', false, false, true);";

				this.Page.Contents = summary.ToString();

				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("ArrangePeriodCount"))
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangePeriodCount"] = summary.SearchCount;
				else
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("ArrangePeriodCount", summary.SearchCount);
			}
			else
				Core.WebServer.Response.Write("Invalid date supplied");		// PUT IN ALERT!!
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
