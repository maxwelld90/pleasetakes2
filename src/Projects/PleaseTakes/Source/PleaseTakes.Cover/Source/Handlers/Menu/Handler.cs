using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Handlers.Menu {

	internal sealed class Handler : Core.Helpers.BaseHandlers.StandardHandler {

		public Handler(Core.Helpers.Path.Parser path)
			: base(path, true, false, false, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {

		}

		protected override void SetTabs() {
			this.Tabs.Add("CoverManagement", "Cover Management", "/Templates/Specific/Cover/Menu/covermanagement.html");
		}

		protected override void SetAlerts() {
			// None Required
		}

		protected override void SetTabSpecific() {

		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "CoverManagement":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Cover Management");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetMenu90() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "CoverManagement":
						tab.Menu90.Add("?path=/cover/arrange/", "arrange.png", "Arrange Staff Cover", "Arrange internal or outside cover for absent members of teaching staff.", "Arrange Staff Cover");
						tab.Menu90.Add("?path=/cover/slips/", "printpaper.png", "Cover Slips", "Print cover slips for any date in the current session.", "Cover Slips");
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