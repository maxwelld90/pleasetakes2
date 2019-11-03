using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses {

	internal abstract class Printable : Helpers.BaseHandlers.StandardHandler {

		public Printable(Helpers.Path.Parser path)
			: base(path, true, false, false, true, false) {
			this.SetPrintContent();
		}

		protected abstract void SetPrintContent();

		protected override void SetBreadcrumbTrails() {
			
		}

		protected override void SetHeaderTags() {
			
		}

		protected override void SetTabs() {
			this.Tabs.Add("Sending", "Sending Print Request", "/Templates/Specific/Printable/sending.html");
		}

		protected override void SetTabSpecific() {
			
		}

		protected override void InitialChecks() {
			
		}

		protected override void SetAlerts() {
			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Sending":
						tab.Alerts.Add("Instructions", new Helpers.Constructor("/Alerts/Specific/Printable/printinstructions.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Yellow, false, false);
						tab.Alerts.Add("Instructions", new Helpers.Constructor("/Alerts/Specific/Printable/back.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Yellow, false, false);
						break;
				}

				tab.Content.SetVariable("Alerts", tab.Alerts.ToString());
			}
		}

		protected override void SetMenu90() {
			
		}

		protected override void SpecificCommands() {
			
		}
	}

}
