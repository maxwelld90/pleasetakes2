using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Attendance {

	internal sealed class Standard : Core.Helpers.BaseHandlers.StandardHandler {
		private DateTime _selectedDate;

		public Standard(Core.Helpers.Path.Parser path)
			: base(path, true, true, true, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {
			this.CheckInputValidity();
		}

		private void CheckInputValidity() {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

			this.Path.Reset();

			for (int i = 0; i <= 2; i++)
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
					Standard.InvalidRedirect();
			}

			if ((!validYear) || (!validMonth) || (!validDay))
				Standard.InvalidRedirect();
			else {
				try {
					year.RequireThat("year").IsInRange(2000, 3000);
					month.RequireThat("month").IsInRange(1, 12);
					day.RequireThat("day").IsInRange(1, DateTime.DaysInMonth(year, month));

					if (Standard.IsDateInSessionRange(year, month, day)) {
						DateTime selectedDate = new DateTime(year, month, day);
						int timetableWeek = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(selectedDate).TimetableWeek;

						try {
							int periods = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[timetableWeek][(int)selectedDate.DayOfWeek + 1];
							
							if (periods.Equals(0))
								Standard.NoPeriodsRedirect();
							else
								this._selectedDate = selectedDate;
						}
						catch (Core.Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) {
							Standard.NoPeriodsRedirect();
						}
					}	
					else
						Standard.OutOfRangeRedirect();
				}
				catch (IndexOutOfRangeException) {
					Standard.InvalidRedirect();
				}
			}
		}

		private static void InvalidRedirect() {
			Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/invalid/");
		}

		private static void OutOfRangeRedirect() {
			Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/outofrange/");
		}

		private static void NoPeriodsRedirect() {
			Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/noperiods/");
		}

		private static bool IsDateInSessionRange(int year, int month, int day) {
			DateTime date = new DateTime(year, month, day);
			DateTime low = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.StartDate;
			DateTime high = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.EndDate;

			if (Core.Utils.IsDateInRange(low, high, date))
				return true;

			return false;
		}

		protected override void SetHeaderTags() {

		}

		protected override void SetTabs() {
			this.Tabs.Add("Attendance", "Staff Attendance", "/Templates/Specific/Cover/Arrange/Attendance/attendance.html");
			this.Tabs.Add("Periods", "Period Selection", "/Templates/Specific/Cover/Arrange/Attendance/periods.html");
		}

		protected override void SetAlerts() {
			
		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Attendance":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Arrange", "?path=/cover/arrange/", null);
						tab.BreadcrumbTrail.Add("Calendar", "?path=/cover/arrange/calendar/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/", null);
						tab.BreadcrumbTrail.Add("Attendance");
						break;
					case "Periods":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Arrange", "?path=/cover/arrange/", null);
						tab.BreadcrumbTrail.Add("Calendar", "?path=/cover/arrange/calendar/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/", null);
						tab.BreadcrumbTrail.Add("Attendance", "#Attendance", "switchToTab('Attendance');");
						tab.BreadcrumbTrail.Add("Periods");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetTabSpecific() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Attendance":
						Core.Helpers.Elements.Search.SearchArea attendanceSearch = new Core.Helpers.Elements.Search.SearchArea("Attendance");
						attendanceSearch.AjaxUrl = "/cover/arrange/attendance/ajax/attendance/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/";
						attendanceSearch.AjaxStatusUrl = "/cover/arrange/attendance/ajax/attendance/status/";
						attendanceSearch.AddButton("search.png", null, "doSearch('Attendance');", "Click to search.");
						attendanceSearch.AddButton("refresh.png", null, "resetSearch('Attendance');", "Click to reset the record list below.");
						attendanceSearch.AddButton("back.png", "?path=/cover/arrange/calendar/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/", null, "Click here to jump back to the calendar.");
						attendanceSearch.AddButton("continue.png", "#Periods", "switchToTab('Periods');", "Click here to see periods listings for the selected staff in this list.");
						tab.Content.SetVariable("SearchAttendance", attendanceSearch.ToString());
						break;
					case "Periods":
						Core.Helpers.Elements.Search.SearchArea periodsSearch = new Core.Helpers.Elements.Search.SearchArea("Periods");
						periodsSearch.AjaxUrl = "/cover/arrange/attendance/ajax/periods/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/";
						periodsSearch.AjaxStatusUrl = "/cover/arrange/attendance/ajax/periods/status/";
						periodsSearch.AddButton("search.png", null, "doSearch('Periods');", "Click to search the period listings.");
						periodsSearch.AddButton("refresh.png", null, "resetSearch('Periods');", "Click to reset the period listings below.");
						periodsSearch.AddButton("back.png", "#Attendance", "switchToTab('Attendance');", "Click here to jump back to the staff selection tab.");
						periodsSearch.AddButton("continue.png", "?path=/cover/arrange/selection/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/", null, "Click to proceed to the final cover arrangement step.");
						tab.Content.SetVariable("SearchPeriods", periodsSearch.ToString());
						break;
				}
			}
		}

		protected override void SetMenu90() {

		}

		protected override void SpecificCommands() {
			
		}

	}

}