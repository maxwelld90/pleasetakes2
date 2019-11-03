using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Selection {

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
			Core.Helpers.Elements.HeaderTags.Script scriptTag = new Core.Helpers.Elements.HeaderTags.Script();
			scriptTag.Source = "?path=/resources/javascript/specific/cover/arrange/selection/selection.js";
			scriptTag.Conditional = Core.Helpers.Elements.HeaderTags.Conditionals.None;
			this.HeaderTags.Add(scriptTag);
		}

		protected override void SetTabs() {
			this.Tabs.Add("Requests", "Cover Requests", "/Templates/Specific/Cover/Arrange/Selection/requests.html");
			this.Tabs.Add("Selection", "Staff Selection", "/Templates/Specific/Cover/Arrange/Selection/selection.html");
		}

		protected override void SetAlerts() {

		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Requests":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Arrange", "?path=/cover/arrange/", null);
						tab.BreadcrumbTrail.Add("Calendar", "?path=/cover/arrange/calendar/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/", null);
						tab.BreadcrumbTrail.Add("Attendance", "?path=/cover/arrange/attendance/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/#Attendance", null);
						tab.BreadcrumbTrail.Add("Periods", "?path=/cover/arrange/attendance/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/#Periods", null);
						tab.BreadcrumbTrail.Add("Requests", null, null);
						break;
					case "Selection":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Arrange", "?path=/cover/arrange/", null);
						tab.BreadcrumbTrail.Add("Calendar", "?path=/cover/arrange/calendar/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/", null);
						tab.BreadcrumbTrail.Add("Attendance", "?path=/cover/arrange/attendance/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/#Attendance", null);
						tab.BreadcrumbTrail.Add("Periods", "?path=/cover/arrange/attendance/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/#Periods", null);
						tab.BreadcrumbTrail.Add("Requests", "#Requests", "switchToTab('Requests');");
						tab.BreadcrumbTrail.Add("Selection", null, null);
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetTabSpecific() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Requests":
						Core.Helpers.Elements.Search.SearchArea requestsSearch = new Core.Helpers.Elements.Search.SearchArea("Requests");
						requestsSearch.AjaxUrl = "/cover/arrange/selection/ajax/requests/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/";
						requestsSearch.AjaxStatusUrl = "/cover/arrange/selection/ajax/requests/status/";
						requestsSearch.AddButton("search.png", null, "doSearch('Requests');", "Click to search.");
						requestsSearch.AddButton("refresh.png", null, "resetSearch('Requests');", "Click to reset the record list below.");
						requestsSearch.AddButton("back.png", "?path=/cover/arrange/attendance/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/#Periods", null, "Click to jump back to the period selection tab.");
						requestsSearch.AddButton("dudeabsent.png", null, "switchAjaxUrl('Requests', '?path=/cover/arrange/selection/ajax/requests/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/'); resetSearch('Requests');", "Show only requests that do require cover.");
						requestsSearch.AddButton("thumbsup.png", null, "switchAjaxUrl('Requests', '?path=/cover/arrange/selection/ajax/requests/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/notrequired/'); resetSearch('Requests');", "Show only requests that do not require cover.");
						requestsSearch.AddButton("printpaper.png", "?path=/cover/slips/printouts/day/" + this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/", null, "Click here to print cover request slips.");
						tab.Content.SetVariable("SearchRequests", requestsSearch.ToString());
						break;
					case "Selection":
						Core.Helpers.Elements.Search.SearchArea staffSearch = new Core.Helpers.Elements.Search.SearchArea("Selection");
						staffSearch.AjaxUrl = "/cover/arrange/selection/ajax/selection/";
						staffSearch.AjaxStatusUrl = "/cover/arrange/selection/ajax/selection/status/";
						staffSearch.AddButton("search.png", null, "doSearch('Selection');", "Click to search.");
						staffSearch.AddButton("refresh.png", null, "resetSearch('Selection');", "Click to reset the record list below.");
						staffSearch.AddButton("back.png", "#Requests", "resetSelection();", "Click to jump back to the Cover Requests tab.");
						staffSearch.AddButton("covertype.png", null, "setStaffType();", "Alternate between showing internal staff and outside cover staff.");
						staffSearch.AddButton("coverentitlements.png", null, "setZeroEntitlements();", "Alternate between showing staff who have an entitlement value of 0 or less, or not.");
						tab.Content.SetVariable("SearchStaff", staffSearch.ToString());
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
