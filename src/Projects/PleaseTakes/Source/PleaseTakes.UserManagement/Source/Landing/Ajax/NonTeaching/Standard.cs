using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Landing.Ajax.NonTeaching {

	internal sealed class Standard : Core.Helpers.BaseHandlers.AjaxHandler {

		public Standard(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string searchTerm = this.GetSearchTerm();
			string alertMessage = this.GetAlertMessage();
			List<Accounts.NonTeachingAccount> searchResults = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.NonTeachingAccounts.Search(searchTerm);

			if ((!string.IsNullOrEmpty(alertMessage)) && (alertMessage.Equals("unknownnonteaching"))) {
				Core.Helpers.Elements.Alerts.Alert unknownTeachingAlert = new Core.Helpers.Elements.Alerts.Alert("UnknownTeaching");
				unknownTeachingAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
				unknownTeachingAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Landing/Nonteaching/unknown.html").ToString();
				unknownTeachingAlert.NoScript = false;
				unknownTeachingAlert.ShowCloseBox = true;
				unknownTeachingAlert.StartHidden = false;

				this.Page.Contents = unknownTeachingAlert.ToString();
			}

			if (searchResults.Count > 0) {
				Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();

				foreach (Accounts.NonTeachingAccount account in searchResults) {
					Records.NonTeachingStaffMember nonTeachingStaffRecord = new Records.NonTeachingStaffMember();
					StaffNameFormatter staffName = new StaffNameFormatter();

					staffName.DisplayForenameFirst = true;
					staffName.Forename = account.Forename;
					staffName.Surname = account.Surname;

					nonTeachingStaffRecord.Username = account.Username;
					nonTeachingStaffRecord.Name = staffName;
					nonTeachingStaffRecord.IsAccountActive = account.IsActive;

					recordCollection.Add(nonTeachingStaffRecord.Record);
				}

				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffNonTeachingCount"))
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffNonTeachingCount"] = searchResults.Count;
				else
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("StaffNonTeachingCount", searchResults.Count);

				this.Page.Contents += recordCollection.ToString();
			}
			else {
				Core.Helpers.Elements.Alerts.Alert noNonTeachingAlert = new Core.Helpers.Elements.Alerts.Alert("NoNonTeaching");
				noNonTeachingAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
				noNonTeachingAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Landing/Nonteaching/none.html").ToString();
				noNonTeachingAlert.NoScript = false;
				noNonTeachingAlert.ShowCloseBox = false;
				noNonTeachingAlert.StartHidden = false;

				this.Page.Contents += noNonTeachingAlert.ToString();
			}


		}

		private string GetSearchTerm() {
			if (this.Path.HasNext())
				return this.Path.Next();

			return "";
		}

		private string GetAlertMessage() {
			Core.Helpers.Path.Parser sourcePath = new Core.Helpers.Path.Parser(Core.WebServer.Request["sourcepath"]);

			if (sourcePath.HasNext()) {
				sourcePath.Next();

				if (sourcePath.HasNext())
					return sourcePath.Next();
			}

			return null;
		}
	}

}