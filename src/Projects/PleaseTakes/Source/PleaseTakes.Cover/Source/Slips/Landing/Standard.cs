using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Slips.Landing {

	internal sealed class Standard : Core.Helpers.BaseHandlers.StandardHandler {

		public Standard(Core.Helpers.Path.Parser path)
			: base(path, true, true, true, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {
			Core.Helpers.Elements.HeaderTags.Script slipScripts = new Core.Helpers.Elements.HeaderTags.Script();
			slipScripts.Conditional = Core.Helpers.Elements.HeaderTags.Conditionals.None;
			slipScripts.Source = "?path=/resources/javascript/specific/cover/slips.js";

			this.HeaderTags.Add(slipScripts);
		}

		protected override void SetTabs() {
			this.Tabs.Add("Calendar", "Date Selection", "/Templates/Specific/Cover/Slips/Standard/Landing/calendar.html");
			this.Tabs.Add("Requests", "Cover Requests", "/Templates/Specific/Cover/Slips/Standard/Landing/requests.html");
		}

		protected override void SetAlerts() {
			// None Required
		}

		protected override void SetTabSpecific() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Calendar":
						Core.Helpers.Elements.Ajax.Element calendar = new Core.Helpers.Elements.Ajax.Element("Calendar");
						calendar.Url = "/cover/slips/ajax/calendar/";
						calendar.ShowLoadingMessage = true;
						calendar.GetPath = true;

						tab.Content.SetVariable("AjaxCalendar", calendar.ToString());
						break;
					case "Requests":
						Core.Helpers.Elements.Search.SearchArea requestsSearch = new Core.Helpers.Elements.Search.SearchArea("Requests");
						requestsSearch.AjaxUrl = "/cover/slips/ajax/requests/";
						requestsSearch.AjaxStatusUrl = "/cover/slips/ajax/requests/status/";
						requestsSearch.GetSourcePath = true;
						requestsSearch.AddCustomField("Date", null);
						requestsSearch.AddButton("search.png", null, "doSearch('Requests');", "Click to search.");
						requestsSearch.AddButton("refresh.png", null, "resetSearch('Requests');", "Click to reset the cover requests list below.");
						requestsSearch.AddButton("printpaper.png", null, "printSlips();", "Print slips for all the cover requests listed below.");
						
						tab.Content.SetVariable("SearchRequests", requestsSearch.ToString());
						break;
				}
			}
		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Calendar":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Cover Slips", "?path=/cover/slips/", null);
						tab.BreadcrumbTrail.Add("Date Selection");
						break;
					case "Requests":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Cover Slips", "?path=/cover/slips/", null);
						tab.BreadcrumbTrail.Add("Date Selection", "#Calendar", "switchToTab('Calendar');");
						tab.BreadcrumbTrail.Add("Cover Requests");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetMenu90() {
			// None Required
		}

		protected override void SpecificCommands() {
			// None Required
		}

	}

}
