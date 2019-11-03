using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Exception {

	internal sealed class Unhandled : Helpers.BaseHandlers.StandardHandler {

		public Unhandled()
			: base(null, false, false, false, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {
			// None required
		}

		protected override void SetTabs() {
			this.Tabs.Add("Unhandled", "Unhandled Exception", "/Templates/Specific/Exception/Unhandled/unhandled.html");
			this.Tabs.Add("StackTrace", "Stack Trace", "/Templates/Specific/Exception/stacktrace.html");
		}

		protected override void SetAlerts() {
			System.Exception raisedException = Core.WebServer.Server.GetLastError().GetBaseException();
			string alertsFolder = "/Alerts/Specific/Exception/";

			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Unhandled":
						Helpers.Constructor location = new Helpers.Constructor(alertsFolder + "/Unhandled/location.html");
						location.SetVariable("Location", WebServer.Request.Url.ToString());

						Helpers.Constructor message = new Helpers.Constructor(alertsFolder + "/Unhandled/message.html");
						message.SetVariable("Message", raisedException.Message);

						Helpers.Constructor exceptionType = new Helpers.Constructor(alertsFolder + "/Unhandled/exceptiontype.html");
						exceptionType.SetVariable("ExceptionType", raisedException.GetType().ToString());

						tab.Alerts.Add("Location", location.ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
						tab.Alerts.Add("Message", message.ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
						tab.Alerts.Add("ExceptionType", exceptionType.ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
						break;
					case "StackTrace":
						Helpers.Constructor stackTrace = new Helpers.Constructor(alertsFolder + "stacktrace.html");
						stackTrace.SetVariable("StackTrace", Utils.FormatStackTrace(raisedException.StackTrace));

						tab.Alerts.Add("StackTrace", stackTrace.ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
						break;
				}

				tab.Content.SetVariable("Alerts", tab.Alerts.ToString());
			}
		}

		protected override void SetTabSpecific() {

		}

		protected override void SetBreadcrumbTrails() {
			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "StackTrace":
						tab.BreadcrumbTrail.Add("Unhandled Exception", "#Unhandled", "switchToTab('Unhandled');");
						tab.BreadcrumbTrail.Add("Stack Trace");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetMenu90() {
			// None required
		}

		protected override void SpecificCommands() {
			// None required
		}

	}

}