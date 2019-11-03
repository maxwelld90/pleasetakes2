using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.TopRight {

	internal sealed class Area {
		private Types _type;

		public Area(Types type) {
			this._type = type;
		}

		public override string ToString() {
			switch (this._type) {
				case Types.Login:
					Constructor login = new Constructor("/Templates/Elements/Topright/login.html");
					login.SetVariable("SchoolId", WebServer.PleaseTakes.Session.CurrentInstance.School.SchoolID);

					if (string.IsNullOrEmpty(WebServer.PleaseTakes.Session.CurrentInstance.School.Authority))
						login.DeleteVariable("Authority");
					else
						login.SetVariable("Authority", "<br />" + WebServer.PleaseTakes.Session.CurrentInstance.School.Authority);

					return login.ToString();
				case Types.Standard:
					Constructor standard = new Constructor("/Templates/Elements/Topright/standard.html");

					if (WebServer.PleaseTakes.Session.CurrentInstance.Account.IsTeachingAccount) {
						UserManagement.Accounts.TeachingAccount account = (UserManagement.Accounts.TeachingAccount)WebServer.PleaseTakes.Session.CurrentInstance.Account;
						string replacement = "<span class=\"Teaching\"><strong>";

						if (!((string.IsNullOrEmpty(account.Forename)) || (string.IsNullOrEmpty(account.Surname))))
							replacement += account.Forename + " " + account.Surname;

						replacement += "</strong>";

						if (!string.IsNullOrEmpty(account.HoldingName))
							replacement += " (" + account.HoldingName + ")";

						replacement += "</span>";

						standard.SetVariable("CurrentUser", replacement);
					}
					else
						standard.SetVariable("CurrentUser", "<strong class=\"NonTeaching\">" + WebServer.PleaseTakes.Session.CurrentInstance.Account.Forename + " " + WebServer.PleaseTakes.Session.CurrentInstance.Account.Surname + "</strong>");

					// sort out "teaching" name. then get to work on cover :)


					return standard.ToString();
			}

			return "";
		}
	}

}