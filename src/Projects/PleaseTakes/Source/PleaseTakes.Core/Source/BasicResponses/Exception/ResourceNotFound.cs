using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Exception {

	internal sealed class ResourceNotFound : Helpers.BaseHandlers.StandardHandler {

		public ResourceNotFound()
			: base(null, false, false, false, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {
			
		}

		protected override void SetHeaderTags() {
			// None required
		}

		protected override void SetTabs() {
			this.Tabs.Add("NotFound", "Resource Not Found", "/Templates/Specific/Exception/Resource/invalid.html");
			this.Tabs.Add("StackTrace", "Stack Trace", "/Templates/Specific/Exception/stacktrace.html");
		}

		protected override void SetAlerts() {
			System.Exception raisedException = Core.WebServer.Server.GetLastError().GetBaseException();
			string alertsFolder = "/Alerts/Specific/Exception/";

			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "NotFound":
						Helpers.Constructor requestedResource = new Helpers.Constructor(alertsFolder + "/Resource/requested.html");
						requestedResource.SetVariable("RequestedResource", raisedException.Message);

						Helpers.Constructor raisedType = new Helpers.Constructor(alertsFolder + "/Resource/raised.html");
						raisedType.SetVariable("RaisedType", raisedException.GetType().ToString());

						tab.Alerts.Add("RequestedResource", requestedResource.ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
						tab.Alerts.Add("RaisedType", raisedType.ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
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
						tab.BreadcrumbTrail.Add("Resource Not Found", "#NotFound", "switchToTab('NotFound');");
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