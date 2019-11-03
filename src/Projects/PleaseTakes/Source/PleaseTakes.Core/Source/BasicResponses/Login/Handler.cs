using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Login {

	internal sealed class Handler : Helpers.BaseHandlers.StandardHandler {

		public Handler(Helpers.Path.Parser path)
			: base(path, false, false, false, false, true) {
			WebServer.PleaseTakes.Session.CurrentInstance.TestDatabaseConnection();
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {
			Helpers.Elements.HeaderTags.Script loginScripts = new Helpers.Elements.HeaderTags.Script();
			loginScripts.Source = "?path=/resources/javascript/specific/login.js";

			Helpers.Elements.HeaderTags.Stylesheet loginCss = new Helpers.Elements.HeaderTags.Stylesheet();
			loginCss.Source = "?path=/resources/stylesheets/specific/login.css";
			loginCss.Media = "screen";

			this.HeaderTags.Add(loginScripts);
			this.HeaderTags.Add(loginCss);
		}

		protected override void SetTabs() {
			this.Tabs.Add("Login", "Login", "/Templates/Specific/Login/login.html");
			this.Tabs.Add("About", "About", "/Templates/Specific/Login/about.html");
			this.Tabs.Add("Updates", "Updates", "/Templates/Specific/Login/updates.html");
		}

		protected override void SetAlerts() {
			string alertsFolder = "/Alerts/Specific/Login/";

			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Login":
						if (this.Path.HasNext())
							switch (this.Path.Peek()) {
								case "denied":
									tab.Alerts.Add("Denied", new Helpers.Constructor(alertsFolder + "denied.html").ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
									break;
								case "disabled":
									tab.Alerts.Add("Disabled", new Helpers.Constructor(alertsFolder + "disabled.html").ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
									break;
								case "invalid":
									tab.Alerts.Add("Invalid", new Helpers.Constructor(alertsFolder + "invalid.html").ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
									break;
								case "missingtimetable":
									tab.Alerts.Add("MissingTimetable", new Helpers.Constructor(alertsFolder + "missingtimetable.html").ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
									break;
								case "missing":
									tab.Alerts.Add("Missing", new Helpers.Constructor(alertsFolder + "missing.html").ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
									break;
								case "duplicate":
									tab.Alerts.Add("Duplicate", new Helpers.Constructor(alertsFolder + "duplicate.html").ToString(), Helpers.Elements.Alerts.Colours.Red, false, false);
									break;
								case "badlogout":
									tab.Alerts.Add("BadLogout", new Helpers.Constructor(alertsFolder + "badlogout.html").ToString(), Helpers.Elements.Alerts.Colours.Yellow, false, false);
									break;
								case "notloggedin":
									tab.Alerts.Add("NotLoggedIn", new Helpers.Constructor(alertsFolder + "notloggedin.html").ToString(), Helpers.Elements.Alerts.Colours.Yellow, false, false);
									break;
								case "expired":
									tab.Alerts.Add("Expired", new Helpers.Constructor(alertsFolder + "expired.html").ToString(), Helpers.Elements.Alerts.Colours.Yellow, false, true);
									break;
								// Temporary
								case "nonadmin":
									tab.Alerts.Add("NonAdmin", new Helpers.Constructor(alertsFolder + "nonadmin.html").ToString(), Helpers.Elements.Alerts.Colours.Yellow, false, false);
									break;
								case "loggedout":
									tab.Alerts.Add("Success", new Helpers.Constructor(alertsFolder + "loggedout.html").ToString(), Helpers.Elements.Alerts.Colours.Green, false, false);
									break;
								default:
									tab.Alerts.Add("Unknown", new Helpers.Constructor(alertsFolder + "unknown.html").ToString(), Helpers.Elements.Alerts.Colours.Yellow, false, true);
									break;
							}

						tab.Alerts.AddNoScriptAlert();
						break;
				}

				tab.Content.SetVariable("Alerts", tab.Alerts.ToString());
			}
		}

		protected override void SetTabSpecific() {
			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Updates":
						Helpers.Elements.RecordLists.Collection updateCollection = new Helpers.Elements.RecordLists.Collection();
						updateCollection.TopSpace = true;

						UpdateRecord record1 = new UpdateRecord();
						record1.Href = "2010-08-18-1.pdf";
						record1.Build = "2010-08-18";
						record1.AddAssembly("Core");
						record1.AddAssembly("Cover");
						record1.Description = "Minor bug fixes";

						UpdateRecord record2 = new UpdateRecord();
						record2.Href = "2010-09-08-1.pdf";
						record2.Build = "2010-09-08";
						record2.AddAssembly("Core");
						record2.AddAssembly("UserManagement");
						record2.Description = "Introduction of teaching staff editing facilities";

						UpdateRecord record3 = new UpdateRecord();
						record3.Href = "2010-09-08-2.pdf";
						record3.Build = "2010-09-08";
						record3.AddAssembly("Core");
						record3.AddAssembly("UserManagement");
						record3.Description = "Introduction of timetable editing facilities";

						UpdateRecord record4 = new UpdateRecord();
						record4.Href = "2010-09-08-3.pdf";
						record4.Build = "2010-09-08";
						record4.AddAssembly("Core");
						record4.Description = "Account password changing";

						UpdateRecord record5 = new UpdateRecord();
						record5.Href = "2010-09-10-1.pdf";
						record5.Build = "2010-09-10";
						record5.AddAssembly("UserManagement");
						record5.Description = "Changing which department teaching staff \"belong to\"";

						UpdateRecord record6 = new UpdateRecord();
						record6.Href = "2010-09-15-1.pdf";
						record6.Build = "2010-09-15";
						record6.AddAssembly("UserManagement");
						record6.Description = "Adding a new member of teaching staff";

						UpdateRecord record7 = new UpdateRecord();
						record7.Href = "2010-09-15-2.pdf";
						record7.Build = "2010-09-15";
						record7.AddAssembly("UserManagement");
						record7.Description = "Ability to create/modify/delete teaching staff accounts";

						updateCollection.Add(record1.Record);
						updateCollection.Add(record2.Record);
						updateCollection.Add(record3.Record);
						updateCollection.Add(record4.Record);
						updateCollection.Add(record5.Record);
						updateCollection.Add(record6.Record);
						updateCollection.Add(record7.Record);

						tab.Content.SetVariable("UpdateCollection", updateCollection.ToString());
						break;
				}
			}
		}

		protected override void SetBreadcrumbTrails() {
			foreach (Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "About":
						tab.BreadcrumbTrail.Add("PleaseTakes Login", "#Login", "toLogin();");
						tab.BreadcrumbTrail.Add("About PleaseTakes");
						break;
					case "Updates":
						tab.BreadcrumbTrail.Add("PleaseTakes Login", "#Login", "toLogin();");
						tab.BreadcrumbTrail.Add("Updates");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetMenu90() {
			// None required
		}

		protected override void SpecificCommands() {
			this.SetAboutVariables();
		}

		private void SetAboutVariables() {
			this.Page.SetVariable("BuildDate", Consts.BuildDate);
			this.SetAssemblyVersions();
		}

		private void SetAssemblyVersions() {
			Helpers.Constructor versionsConstructor = new Helpers.Constructor("/Templates/Specific/Login/assemblyversions.html");
			versionsConstructor.SetVariable("Core", AssemblyDetails.Combined);
			versionsConstructor.SetVariable("Cover", Cover.AssemblyDetails.Combined);
			versionsConstructor.SetVariable("UserManagement", UserManagement.AssemblyDetails.Combined);
			versionsConstructor.SetVariable("Resources", Resources.AssemblyDetails.Combined);

			this.Page.SetVariable("AssemblyVersions", versionsConstructor.ToString());
		}

	}

}