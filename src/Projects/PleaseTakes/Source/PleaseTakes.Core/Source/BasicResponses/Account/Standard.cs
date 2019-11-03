using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Account {

	internal sealed class Standard : Helpers.BaseHandlers.StandardHandler {
		private string _alert;

		public Standard(Helpers.Path.Parser path)
			: base(path, true, false, false, false, true) {
			this.Output.Send();
		}

		protected override void InitialChecks() {

		}

		protected override void SetHeaderTags() {

		}

		protected override void SetTabs() {
			this.Tabs.Add("Landing", "Account Settings", "/Templates/Specific/Account/landing.html");
			this.Tabs.Add("Password", "Password", "/Templates/Specific/Account/password.html");
		}

		protected override void SetAlerts() {

		}

		protected override void SetTabSpecific() {
			this.SetAlert();

			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Password":
						Core.Helpers.Elements.Forms.BasicForm passwordForm = new Core.Helpers.Elements.Forms.BasicForm();
						passwordForm.Id = "Password";
						passwordForm.HasTopSpace = true;
						passwordForm.PostUrl = "/account/update/password/";

						switch (this._alert) {
							case "passwordmissing":
								Helpers.Elements.Alerts.Alert missingAlert = new Helpers.Elements.Alerts.Alert("PasswordMissing");
								missingAlert.Colour = Helpers.Elements.Alerts.Colours.Red;
								missingAlert.Message = new Helpers.Constructor("/Alerts/Specific/Account/Password/missing.html").ToString();

								passwordForm.RightPane.Contents = missingAlert.ToString();
								break;
							case "passwordold":
								Helpers.Elements.Alerts.Alert oldAlert = new Helpers.Elements.Alerts.Alert("PasswordOld");
								oldAlert.Colour = Helpers.Elements.Alerts.Colours.Red;
								oldAlert.Message = new Helpers.Constructor("/Alerts/Specific/Account/Password/old.html").ToString();

								passwordForm.RightPane.Contents = oldAlert.ToString();
								break;
							case "passwordmatch":
								Helpers.Elements.Alerts.Alert matchAlert = new Helpers.Elements.Alerts.Alert("PasswordMatch");
								matchAlert.Colour = Helpers.Elements.Alerts.Colours.Red;
								matchAlert.Message = new Helpers.Constructor("/Alerts/Specific/Account/Password/match.html").ToString();

								passwordForm.RightPane.Contents = matchAlert.ToString();
								break;
							case "passwordlength":
								Helpers.Elements.Alerts.Alert lengthAlert = new Helpers.Elements.Alerts.Alert("PasswordLength");
								Helpers.Constructor lengthConstructor = new Helpers.Constructor("/Alerts/Specific/Account/Password/length.html");
								lengthConstructor.SetVariable("NumChars", Consts.MinimumPasswordLength.ToString());

								lengthAlert.Colour = Helpers.Elements.Alerts.Colours.Red;
								lengthAlert.Message = lengthConstructor.ToString();

								passwordForm.RightPane.Contents = lengthAlert.ToString();
								break;
							case "passwordsuccess":
								Helpers.Elements.Alerts.Alert successAlert = new Helpers.Elements.Alerts.Alert("PasswordSuccess");
								successAlert.Colour = Helpers.Elements.Alerts.Colours.Green;
								successAlert.Message = new Helpers.Constructor("/Alerts/Specific/Account/Password/success.html").ToString();

								passwordForm.RightPane.Contents = successAlert.ToString();
								break;
							default:
								passwordForm.RightPane.Contents = "&nbsp;";
								break;
						}

						passwordForm.AddButton(null, "Clear", null, 5, Core.Helpers.Elements.Forms.ButtonTypes.Reset);
						passwordForm.AddButton(null, "Change", null, 4, Core.Helpers.Elements.Forms.ButtonTypes.Submit);

						Core.Helpers.Elements.Forms.Row oldRow = passwordForm.AddRow();
						oldRow.Description = "<strong>Old Password</strong>";
						oldRow.SetToTextField("Old", null, null, 1, 0, true, false);

						Core.Helpers.Elements.Forms.Row newRow = passwordForm.AddRow();
						newRow.Description = "<strong>New Password</strong>";
						newRow.SetToTextField("New", null, null, 2, 0, true, false);

						Core.Helpers.Elements.Forms.Row confirmRow = passwordForm.AddRow();
						confirmRow.Description = "<strong>Confirm Password</strong>";
						confirmRow.SetToTextField("Confirm", null, null, 3, 0, true, false);

						tab.Content.SetVariable("NumChars", Consts.MinimumPasswordLength.ToString());
						tab.Content.SetVariable("PasswordForm", passwordForm.ToString());
						break;
				}
			}
		}

		protected override void SetBreadcrumbTrails() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Landing":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Account Settings");
						break;
					case "Password":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Account Settings", "#Landing", "switchToTab('Landing');");
						tab.BreadcrumbTrail.Add("Password");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetMenu90() {
			// None required
		}

		protected override void SpecificCommands() {

		}

		private void SetAlert() {
			this._alert = "";

			if (this.Path.HasNext())
				this._alert = this.Path.Next();
		}

	}

}