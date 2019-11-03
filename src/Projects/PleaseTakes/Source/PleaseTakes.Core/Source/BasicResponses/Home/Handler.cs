using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Home {

	internal sealed class Handler : Helpers.BaseHandlers.StandardHandler {

		public Handler(Helpers.Path.Parser path)
			: base(path, true, false, false, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {
			
		}

		protected override void SetTabs() {
			this.Tabs.Add("Home", "Home", "/Templates/Specific/Home/home.html");
		}

		protected override void SetAlerts() {
			//string alertsFolder = "/Alerts/Specific/Home/";

			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				tab.Content.DeleteVariable("Alerts");
			}
		}

		protected override void SetTabSpecific() {

		}

		protected override void SetBreadcrumbTrails() {

		}

		protected override void SetMenu90() {
			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Home":
						if (WebServer.PleaseTakes.Session.CurrentInstance.Account.IsAdmin) {
							tab.Menu90.Add("?path=/cover/", "covermanagement.png", "Cover Management", "Arrange cover, print cover slips, and view the cover summary.", "Cover Management");
							tab.Menu90.Add("?path=/staff/", "staff.png", "Staff Management", "Add, delete or modify staff records, as well as their individual timetables.", "Staff Management");
						}
						else {

						}

						tab.Menu90.Add("?path=/logout/", "logout.png", "Logout", "Ends your PleaseTakes session and returns you to the login screen.<br />You'll have to re-enter your details to get back in.", "Logout");

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