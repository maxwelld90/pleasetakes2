using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Handlers.Arrange {

	internal sealed class Landing : Core.Helpers.BaseHandlers.StandardHandler {

		public Landing(Core.Helpers.Path.Parser path)
			: base(path, true, false, false, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {

		}

		protected override void SetTabs() {
			this.Tabs.Add("Landing", "Arrange Staff Cover", "/Templates/Specific/Cover/Arrange/Landing/arrangestaffcover.html");
		}

		protected override void SetAlerts() {
			// None Required
		}

		protected override void SetTabSpecific() {

		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Landing":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management", "?path=/cover/", null);
						tab.BreadcrumbTrail.Add("Arrange");
						tab.BreadcrumbTrail.Add("Calendar", "?path=/cover/arrange/calendar/", null);
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());

				Core.Helpers.Elements.Alerts.Alert reminderAlert = new Core.Helpers.Elements.Alerts.Alert("Reminder");
				reminderAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
				reminderAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Landing/reminder.html").ToString();
				reminderAlert.NoScript = false;
				reminderAlert.ShowCloseBox = true;
				reminderAlert.StartHidden = false;

				tab.Content.SetVariable("AlertReminder", reminderAlert.ToString());
			}
		}

		protected override void SetMenu90() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "CoverManagement":
						tab.Menu90.Add("?path=/cover/arrange/", "arrange.png", "Arrange Staff Cover", "Arrange cover for absent members of teaching staff quickly.", "Arrange Staff Cover");
						break;
				}

				tab.Content.SetVariable("Menu90", tab.Menu90.ToString());
			}
		}

		protected override void SpecificCommands() {
			// None Required
		}

	}

}