using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Helpers.Elements.Forms;

namespace PleaseTakes.UserManagement.Modify.Teaching {

	internal sealed class Standard : Core.Helpers.BaseHandlers.StandardHandler {
		private int _staffId;
		private int _entitlement;
		private StaffNameFormatter _name;
		private bool _hasAccount;
		private bool _isAdmin;
		private bool _isActive;
		private string _username;
		private string _alert;

		public Standard(Core.Helpers.Path.Parser path)
			: base(path, true, true, true, false, true) {
			this.Output.Send();
		}

		protected override void InitialChecks() {
			if (!this.CheckStaffId())
				Core.WebServer.PleaseTakes.Redirect("/staff/unknownteaching/#Teaching");

			if (this.Path.HasNext())
				this._alert = this.Path.Next();
		}

		private bool CheckStaffId() {
			int readAttempt;

			if (this.Path.HasNext())
				if (int.TryParse(this.Path.Next(), out readAttempt)) {
					Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
					paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", readAttempt);

					using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Teaching/existscheck.sql", paramBuilder.Parameters)) {
						if (dataReader.HasRows) {
							dataReader.Read();
							this._staffId = (int)dataReader["StaffId"];
							this._entitlement = (int)dataReader["Entitlement"];
							this._name = new StaffNameFormatter();
							this._name.Title = dataReader["Title"] as string;
							this._name.Forename = dataReader["Forename"] as string;
							this._name.Surname = dataReader["Surname"] as string;
							this._name.HoldingName = dataReader["HoldingName"] as string;
							this._hasAccount = (bool)dataReader["HasAccount"];
							this._isAdmin = (bool)dataReader["IsAdmin"];
							this._isActive = (bool)dataReader["IsActive"];
							this._username = dataReader["Username"] as string;

							return true;
						}
					}
				}

			return false;
		}

		protected override void SetAlerts() {

		}

		protected override void SetBreadcrumbTrails() {
			this._name.DisplayForenameFirst = true;

			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Name":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "?path=/staff/", null);
						tab.BreadcrumbTrail.Add("Teaching Staff", "?path=/staff/#Teaching", null);
						tab.BreadcrumbTrail.Add("<strong>" + this._name.ToString() + "</strong>");
						tab.BreadcrumbTrail.Add("Name");
						break;
					case "Entitlement":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "?path=/staff/", null);
						tab.BreadcrumbTrail.Add("Teaching Staff", "?path=/staff/#Teaching", null);
						tab.BreadcrumbTrail.Add("<strong>" + this._name.ToString() + "</strong>");
						tab.BreadcrumbTrail.Add("Entitlement");
						break;
					case "Account":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "?path=/staff/", null);
						tab.BreadcrumbTrail.Add("Teaching Staff", "?path=/staff/#Teaching", null);
						tab.BreadcrumbTrail.Add("<strong>" + this._name.ToString() + "</strong>");
						tab.BreadcrumbTrail.Add("Account");
						break;
					case "Department":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "?path=/staff/", null);
						tab.BreadcrumbTrail.Add("Teaching Staff", "?path=/staff/#Teaching", null);
						tab.BreadcrumbTrail.Add("<strong>" + this._name.ToString() + "</strong>");
						tab.BreadcrumbTrail.Add("Department");
						break;
					case "Timetable":
						tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
						tab.BreadcrumbTrail.Add("Staff Management", "?path=/staff/", null);
						tab.BreadcrumbTrail.Add("Teaching Staff", "?path=/staff/#Teaching", null);
						tab.BreadcrumbTrail.Add("<strong>" + this._name.ToString() + "</strong>");
						tab.BreadcrumbTrail.Add("Timetable");
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
			this.Tabs.Add("Name", "Name", "/Templates/Specific/Staff/Modify/Teaching/name.html");
			this.Tabs.Add("Entitlement", "Entitlement", "/Templates/Specific/Staff/Modify/Teaching/entitlement.html");
			this.Tabs.Add("Account", "Account", "/Templates/Specific/Staff/Modify/Teaching/account.html");
			this.Tabs.Add("Department", "Department", "/Templates/Specific/Staff/Modify/Teaching/department.html");
			this.Tabs.Add("Timetable", "Timetable", "/Templates/Specific/Staff/Modify/Teaching/timetable.html");
		}

		protected override void SetTabSpecific() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Name":
						Core.Helpers.Elements.Forms.BasicForm nameForm = new Core.Helpers.Elements.Forms.BasicForm();
						nameForm.Id = "Name";
						nameForm.FieldWidths = 250;
						nameForm.HasTopSpace = true;
						nameForm.PostUrl = "/staff/modify/update/teaching/name/";

						nameForm.AddHiddenField("StaffId", null, this._staffId.ToString());

						nameForm.AddButton(null, "Reset", null, 6, Core.Helpers.Elements.Forms.ButtonTypes.Reset);
						nameForm.AddButton(null, "Update", null, 5, Core.Helpers.Elements.Forms.ButtonTypes.Submit);

						if (!string.IsNullOrEmpty(this._alert)) {
							switch (this._alert) {
								case "nameallblank":
									Core.Helpers.Elements.Alerts.Alert nameAllBlankAlert = new Core.Helpers.Elements.Alerts.Alert("NameAllBlank");
									nameAllBlankAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
									nameAllBlankAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Name/nameallblank.html").ToString();
									nameAllBlankAlert.NoScript = false;
									nameAllBlankAlert.ShowCloseBox = false;
									nameAllBlankAlert.StartHidden = false;

									nameForm.RightPane.Contents = nameAllBlankAlert.ToString();
									break;
								case "nameincomplete":
									Core.Helpers.Elements.Alerts.Alert nameIncompleteAlert = new Core.Helpers.Elements.Alerts.Alert("NameIncomplete");
									nameIncompleteAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
									nameIncompleteAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Name/nameincomplete.html").ToString();
									nameIncompleteAlert.NoScript = false;
									nameIncompleteAlert.ShowCloseBox = false;
									nameIncompleteAlert.StartHidden = false;

									nameForm.RightPane.Contents = nameIncompleteAlert.ToString();
									break;
								case "namesuccess":
									Core.Helpers.Elements.Alerts.Alert nameSuccessAlert = new Core.Helpers.Elements.Alerts.Alert("NameSuccess");
									nameSuccessAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
									nameSuccessAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Name/namesuccess.html").ToString();
									nameSuccessAlert.NoScript = false;
									nameSuccessAlert.ShowCloseBox = false;
									nameSuccessAlert.StartHidden = false;

									nameForm.RightPane.Contents = nameSuccessAlert.ToString();
									break;
								case "namenew":
									Core.Helpers.Elements.Alerts.Alert newStaffMemberAlert = new Core.Helpers.Elements.Alerts.Alert("NewStaffMember");
									newStaffMemberAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
									newStaffMemberAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Name/new.html").ToString();
									newStaffMemberAlert.NoScript = false;
									newStaffMemberAlert.ShowCloseBox = false;
									newStaffMemberAlert.StartHidden = false;

									nameForm.RightPane.Contents = newStaffMemberAlert.ToString();
									break;
								default:
									nameForm.RightPane.Contents = "&nbsp;";
									break;
							}
						}
						else
							nameForm.RightPane.Contents = "&nbsp;";

						Core.Helpers.Elements.Forms.Row titleRow = nameForm.AddRow();
						titleRow.Description = "<strong>Title</strong>";
						titleRow.SetToTextField("Title", null, this._name.Title, 1, 0, false, false);

						Core.Helpers.Elements.Forms.Row forenameRow = nameForm.AddRow();
						forenameRow.Description = "<strong>Forename</strong>";
						forenameRow.SetToTextField("Forename", null, this._name.Forename, 2, 0, false, false);

						Core.Helpers.Elements.Forms.Row surnameRow = nameForm.AddRow();
						surnameRow.Description = "<strong>Surname</strong>";
						surnameRow.SetToTextField("Surname", null, this._name.Surname, 3, 0, false, false);

						Core.Helpers.Elements.Forms.Row holdingNameRow = nameForm.AddRow();
						holdingNameRow.Description = "<strong>Holding Name</strong>";
						holdingNameRow.SetToTextField("HoldingName", null, this._name.HoldingName, 4, 0, false, true);

						tab.Content.SetVariable("NameForm", nameForm.ToString());
						break;
					case "Entitlement":
						Core.Helpers.Elements.Forms.BasicForm entitlementForm = new Core.Helpers.Elements.Forms.BasicForm();
						entitlementForm.Id = "Entitlement";
						entitlementForm.HasTopSpace = true;
						entitlementForm.FieldWidths = 250;
						entitlementForm.PostUrl = "/staff/modify/update/teaching/entitlement/";

						entitlementForm.AddHiddenField("StaffId", null, this._staffId.ToString());

						entitlementForm.AddButton(null, "Reset", null, 3, Core.Helpers.Elements.Forms.ButtonTypes.Reset);
						entitlementForm.AddButton(null, "Update", null, 2, Core.Helpers.Elements.Forms.ButtonTypes.Submit);

						if (!string.IsNullOrEmpty(this._alert)) {
							switch (this._alert) {
								case "entitlementbad":
									Core.Helpers.Elements.Alerts.Alert entitlementBadAlert = new Core.Helpers.Elements.Alerts.Alert("EntitlementBad");
									entitlementBadAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
									entitlementBadAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Entitlement/entitlementbad.html").ToString();
									entitlementBadAlert.NoScript = false;
									entitlementBadAlert.ShowCloseBox = false;
									entitlementBadAlert.StartHidden = false;

									entitlementForm.RightPane.Contents = entitlementBadAlert.ToString();
									break;
								case "entitlementsuccess":
									Core.Helpers.Elements.Alerts.Alert entitlementSuccessAlert = new Core.Helpers.Elements.Alerts.Alert("EntitlementSuccess");
									entitlementSuccessAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
									entitlementSuccessAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Entitlement/entitlementsuccess.html").ToString();
									entitlementSuccessAlert.NoScript = false;
									entitlementSuccessAlert.ShowCloseBox = false;
									entitlementSuccessAlert.StartHidden = false;

									entitlementForm.RightPane.Contents = entitlementSuccessAlert.ToString();
									break;
							}
						}
						else
							entitlementForm.RightPane.Contents = "&nbsp;";

						Core.Helpers.Elements.Forms.Row entitlementRow = entitlementForm.AddRow();
						entitlementRow.Description = "<strong>Entitlement Value</strong>";
						entitlementRow.SetToTextField("Entitlement", null, this._entitlement.ToString(), 1, Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoPeriods.ToString().Length, false, false);

						tab.Content.SetVariable("EntitlementForm", entitlementForm.ToString());
						tab.Content.SetVariable("MaxPeriods", Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoPeriods.ToString());
						break;
					case "Account":
						tab.Content.SetVariable("AccountForm", this.InitialiseAccountForm().ToString());
						break;
					case "Department":
						Core.Helpers.Elements.Search.SearchArea departmentSearch = new Core.Helpers.Elements.Search.SearchArea("Department");
						departmentSearch.AjaxUrl = "/staff/modify/ajax/teaching/department/" + this._staffId + "/search/";
						departmentSearch.AjaxStatusUrl = "/staff/modify/ajax/teaching/department/status/";
						departmentSearch.AddButton("search.png", null, "doSearch('Department');", "Click here to search for departments.");
						departmentSearch.AddButton("refresh.png", null, "resetSearch('Department');", "Click here to reset the department search.");

						tab.Content.SetVariable("DepartmentSearchArea", departmentSearch.ToString());
						break;
					case "Timetable":
						Core.Helpers.Elements.Ajax.Element ajaxTimetableArea = new Core.Helpers.Elements.Ajax.Element("Timetable");
						ajaxTimetableArea.ShowLoadingMessage = true;
						ajaxTimetableArea.GetPath = true;
						ajaxTimetableArea.Url = "/staff/modify/ajax/teaching/timetable/" + this._staffId + "/";

						Core.Helpers.Elements.Alerts.Alert statusAlert = this.GetTimetableStatusMessage();

						if (statusAlert == null)
							tab.Content.DeleteVariable("UpdateAlert");
						else
							tab.Content.SetVariable("UpdateAlert", statusAlert.ToString());

						tab.Content.SetVariable("AjaxTimetable", ajaxTimetableArea.ToString());
						break;
				}
			}
		}

		private Core.Helpers.Elements.Alerts.Alert GetTimetableStatusMessage() {
			if (this.Path.HasNext()) {
				this.Path.Next();

				if (this.Path.HasNext() && this.Path.Peek().Equals("success")) {
					Core.Helpers.Elements.Alerts.Alert successAlert = new Core.Helpers.Elements.Alerts.Alert("UpdateSuccess");
					successAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
					successAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Timetable/success.html").ToString();
					successAlert.ShowCloseBox = true;

					return successAlert;
				}
				else if (this.Path.HasNext() && this.Path.Peek().Equals("failed")) {
					Core.Helpers.Elements.Alerts.Alert failedAlert = new Core.Helpers.Elements.Alerts.Alert("UpdateFailed");
					failedAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
					failedAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Timetable/failed.html").ToString();
					failedAlert.ShowCloseBox = true;

					return failedAlert;
				}
			}

			return null;
		}

		private Form InitialiseAccountForm() {
			GroupedForm accountForm = new GroupedForm();
			accountForm.Id = "Account";
			accountForm.PostUrl = "/staff/modify/update/teaching/account/";
			accountForm.HasTopSpace = true;
			accountForm.RadioGroupName = "HasAccount";
			accountForm.AddHiddenField("StaffId", null, this._staffId.ToString());

			if (this._hasAccount)
				accountForm.OnReset = "setFormTo('Account', 'Enabled');";
			else
				accountForm.OnReset = "setFormTo('Account', 'Disabled'); updateOtherRadios('Account', 'AccountType'); updateOtherRadios('Account', 'Active');";

			if (this._hasAccount) {
				if (this._isAdmin)
					accountForm.OnReset += " switchRadio('Account', 'AccountType', 'Administrative');";
				else
					accountForm.OnReset += " switchRadio('Account', 'AccountType', 'Standard');";

				if (this._isActive)
					accountForm.OnReset += " switchRadio('Account', 'Active', 'Yes');";
				else
					accountForm.OnReset += " switchRadio('Account', 'Active', 'No');";
			}

			accountForm.AddButton(null, "Reset", null, 7, ButtonTypes.Reset);
			accountForm.AddButton(null, "Update", null, 6, ButtonTypes.Submit);

			this.GetAccountEnabledGroup(accountForm);
			this.GetAccountDisabledGroup(accountForm);

			return accountForm;
		}

		private Group GetAccountEnabledGroup(GroupedForm parent) {
			Group enabledGroup = parent.AddGroup("Enabled");
			enabledGroup.RadioValue = "Enabled";
			enabledGroup.TitleDetails.Text = "<strong>A PleaseTakes account exists for this teaching staff member</strong>";
			enabledGroup.TitleDetails.Image.Src = "addkey.png";
			enabledGroup.TitleDetails.Image.Tooltip = "An account exists";
			enabledGroup.IsDefault = this._hasAccount;

			string alertMessage = (string)Core.Utils.ReturnTemporaryStorageObject("StaffTeachingAccountAlert", true);

			if (string.IsNullOrEmpty(alertMessage))
				enabledGroup.RightPane.Contents = "&nbsp;";
			else {
				switch (alertMessage) {
					case "missing":
						Core.Helpers.Elements.Alerts.Alert missingAlert = new Core.Helpers.Elements.Alerts.Alert("MissingDetails");
						missingAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
						missingAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/missing.html").ToString();

						enabledGroup.RightPane.Contents = missingAlert.ToString();
						break;
					case "exists":
						Core.Helpers.Elements.Alerts.Alert alreadyExistsAlert = new Core.Helpers.Elements.Alerts.Alert("AlreadyExists");
						alreadyExistsAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
						alreadyExistsAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/exists.html").ToString();

						enabledGroup.RightPane.Contents = alreadyExistsAlert.ToString();
						break;
					case "updated":
						Core.Helpers.Elements.Alerts.Alert updatedAlert = new Core.Helpers.Elements.Alerts.Alert("Updated");
						updatedAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
						updatedAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/updated.html").ToString();

						enabledGroup.RightPane.Contents = updatedAlert.ToString();
						break;
					case "created":
						Core.Helpers.Constructor createdConstructor = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/created.html");
						createdConstructor.SetVariable("Password", (string)Core.Utils.ReturnTemporaryStorageObject("StaffTeachingAccountPassword", true));

						Core.Helpers.Elements.Alerts.Alert createdAlert = new Core.Helpers.Elements.Alerts.Alert("Created");
						createdAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
						createdAlert.Message = createdConstructor.ToString();

						enabledGroup.RightPane.Contents = createdAlert.ToString();
						break;
					case "deleted":
						Core.Helpers.Elements.Alerts.Alert deletedAlert = new Core.Helpers.Elements.Alerts.Alert("Deleted");
						deletedAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
						deletedAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/deleted.html").ToString();

						enabledGroup.RightPane.Contents = deletedAlert.ToString();
						break;
					case "resetnoaccount":
						Core.Helpers.Elements.Alerts.Alert resetNoAccountAlert = new Core.Helpers.Elements.Alerts.Alert("ResetNoAccount");
						resetNoAccountAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
						resetNoAccountAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/resetnoaccount.html").ToString();

						enabledGroup.RightPane.Contents = resetNoAccountAlert.ToString();
						break;
					case "resetsuccess":
						Core.Helpers.Constructor resetSuccessConstructor = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/resetsuccess.html");
						resetSuccessConstructor.SetVariable("Password", (string)Core.Utils.ReturnTemporaryStorageObject("StaffTeachingAccountPassword", true));

						Core.Helpers.Elements.Alerts.Alert resetSuccessAlert = new Core.Helpers.Elements.Alerts.Alert("ResetSuccess");
						resetSuccessAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
						resetSuccessAlert.Message = resetSuccessConstructor.ToString();

						enabledGroup.RightPane.Contents = resetSuccessAlert.ToString();
						break;
					case "loggedin":
						Core.Helpers.Elements.Alerts.Alert loggedInAlert = new Core.Helpers.Elements.Alerts.Alert("LoggedIn");
						loggedInAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
						loggedInAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Account/loggedin.html").ToString();

						enabledGroup.RightPane.Contents = loggedInAlert.ToString();
						break;
					default:
						enabledGroup.RightPane.Contents = "&nbsp;";
						break;
				}
			}

			Row username = enabledGroup.AddRow();
			username.Description = "<strong>Username</strong>";
			username.SetToTextField("Username", null, this._username, 1, 0, false, false);

			Row password = enabledGroup.AddRow();
			password.Description = "<strong>Password</strong>";

			if (this._hasAccount)
				password.Text = "<a href=\"?path=/staff/modify/update/teaching/account/reset/" + this._staffId + "/\"><em>Click to reset...</em></a>";
			else
				password.Text = "<em>Automatically generated</em>";

			Row accountType = enabledGroup.AddRow();
			accountType.Description = "<strong>Account Type</strong>";
			accountType.SetToRadioGroup();

			RadioGroup typeGroup = (RadioGroup)accountType.FormElement;
			typeGroup.Name = "AccountType";

			RadioButton administrative = typeGroup.AddRadioButton();
			administrative.Value = "Administrative";
			administrative.Label = "Administrative";
			administrative.TabIndex = 2;
			administrative.IsChecked = (this._hasAccount && this._isAdmin);

			RadioButton standard = typeGroup.AddRadioButton();
			standard.Value = "Standard";
			standard.Label = "Standard";
			standard.TabIndex = 3;
			standard.IsChecked = (this._hasAccount && !this._isAdmin);

			Row active = enabledGroup.AddRow();
			active.Description = "<strong>Is Active?</strong>";
			active.SetToRadioGroup();

			RadioGroup activeGroup = (RadioGroup)active.FormElement;
			activeGroup.Name = "Active";

			RadioButton yes = activeGroup.AddRadioButton();
			yes.Value = "Yes";
			yes.Label = "Yes";
			yes.TabIndex = 4;
			yes.IsChecked = (this._hasAccount && this._isActive);

			RadioButton no = activeGroup.AddRadioButton();
			no.Value = "No";
			no.Label = "No";
			no.TabIndex = 5;
			no.IsChecked = (this._hasAccount && !this._isActive);

			return enabledGroup;
		}

		private Group GetAccountDisabledGroup(GroupedForm parent) {
			Group disabledGroup = parent.AddGroup("Disabled");
			disabledGroup.RadioValue = "Disabled";
			disabledGroup.TitleDetails.Text = "<strong>A PleaseTakes account does not exist for this teaching staff member</strong>";
			disabledGroup.TitleDetails.Image.Src = "removekey.png";
			disabledGroup.TitleDetails.Image.Tooltip = "No account exists";
			disabledGroup.IsDefault = !this._hasAccount;

			return disabledGroup;
		}

		protected override void SpecificCommands() {

		}

	}

}