using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Landing {

	internal sealed class Standard : Core.Helpers.BaseHandlers.StandardHandler {

		public Standard(Core.Helpers.Path.Parser path)
			: base(path, true, true, true, false, false) {
			this.Output.Send();
		}

		protected override void InitialChecks() {
			
		}

		protected override void SetAlerts() {

		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Landing":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management");
						break;
					case "Teaching":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "#Landing", "switchToTab('Landing');");
						tab.BreadcrumbTrail.Add("Teaching Staff");
						break;
					case "NonTeaching":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "#Landing", "switchToTab('Landing');");
						tab.BreadcrumbTrail.Add("Non-Teaching Staff");
						break;
					case "Outside":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "#Landing", "switchToTab('Landing');");
						tab.BreadcrumbTrail.Add("Outside Cover Staff");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetHeaderTags() {
			
		}

		protected override void SetMenu90() {
			
		}

		protected override void SetTabs() {
			this.Tabs.Add("Landing", "Staff Management", "/Templates/Specific/Staff/Landing/landing.html");
			this.Tabs.Add("Teaching", "Teaching Staff", "/Templates/Specific/Staff/Landing/teaching.html");
			this.Tabs.Add("NonTeaching", "Non-Teaching Staff", "/Templates/Specific/Staff/Landing/nonteaching.html");
			this.Tabs.Add("Outside", "Outside Cover Staff", "/Templates/Specific/Staff/Landing/outside.html");
		}

		protected override void SetTabSpecific() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Teaching":
						Core.Helpers.Elements.Search.SearchArea searchTeaching = new Core.Helpers.Elements.Search.SearchArea("Teaching");
						searchTeaching.AjaxUrl = "/staff/ajax/teaching/search/";
						searchTeaching.AjaxStatusUrl = "/staff/ajax/teaching/status/";
						searchTeaching.GetSourcePath = true;
						searchTeaching.AddButton("search.png", null, "doSearch('Teaching');", "Click to search.");
						searchTeaching.AddButton("refresh.png", null, "resetSearch('Teaching');", "Click to reset the teaching staff list below.");
						searchTeaching.AddButton("add.png", "?path=/staff/add/teaching/", null, "Click here to add a new member of teaching staff.");

						tab.Content.SetVariable("SearchTeaching", searchTeaching.ToString());
						break;
					case "NonTeaching":
						Core.Helpers.Elements.Search.SearchArea searchNonTeaching = new Core.Helpers.Elements.Search.SearchArea("NonTeaching");
						searchNonTeaching.AjaxUrl = "/staff/ajax/nonteaching/search/";
						searchNonTeaching.AjaxStatusUrl = "staff/ajax/nonteaching/status/";
						searchNonTeaching.GetSourcePath = true;
						searchNonTeaching.AddButton("search.png", null, "doSearch('NonTeaching');", "Click to search.");
						searchNonTeaching.AddButton("refresh.png", null, "resetSearch('NonTeaching');", "Click to reset the non-teaching staff list below.");

						tab.Content.SetVariable("SearchNonTeaching", searchNonTeaching.ToString());
						break;
					case "Outside":
						Core.Helpers.Elements.Search.SearchArea searchOutside = new Core.Helpers.Elements.Search.SearchArea("Outside");
						searchOutside.AjaxUrl = "/staff/ajax/outside/search/";
						searchOutside.AjaxStatusUrl = "/staff/ajax/outside/status/";
						searchOutside.GetSourcePath = true;
						searchOutside.AddButton("search.png", null, "doSearch('Outside');", "Click to search.");
						searchOutside.AddButton("refresh.png", null, "resetSearch('Outside');", "Click to reset the outside cover staff list below.");

						tab.Content.SetVariable("SearchOutside", searchOutside.ToString());
						break;
				}
			}
		}

		protected override void SpecificCommands() {

		}

	}

}