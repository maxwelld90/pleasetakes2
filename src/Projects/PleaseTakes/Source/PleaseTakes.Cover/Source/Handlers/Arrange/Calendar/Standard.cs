using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Handlers.Arrange.Calendar {

	internal sealed class Standard : Core.Helpers.BaseHandlers.StandardHandler {

		public Standard(Core.Helpers.Path.Parser path)
			: base(path, true, true, false, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {

		}

		protected override void SetTabs() {
			this.Tabs.Add("Calendar", "Select a Date", "/Templates/Specific/Cover/Arrange/Calendar/calendar.html");
		}

		protected override void SetAlerts() {

		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Calendar":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Arrange", "?path=/cover/arrange/", null);
						tab.BreadcrumbTrail.Add("Calendar");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetTabSpecific() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Calendar":
						Core.Helpers.Elements.Ajax.Element calendar = new Core.Helpers.Elements.Ajax.Element("Calendar");
						calendar.Url = "/cover/arrange/calendar/ajax/";
						calendar.ShowLoadingMessage = true;
						calendar.GetPath = true;

						tab.Content.SetVariable("AjaxCalendar", calendar.ToString());
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