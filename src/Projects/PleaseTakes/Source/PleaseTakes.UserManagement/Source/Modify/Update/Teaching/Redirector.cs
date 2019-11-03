using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "name":
						new Name();
						break;
					case "entitlement":
						new Entitlement();
						break;
					case "account":
						if (path.HasNext() && path.Next().Equals("reset"))
							new AccountPasswordReset(path);
						else
							new Account();
						break;
					case "department":
						new Department(path);
						break;
					case "timetable":
						new Timetable.Standard(path);
						break;
					default:
						Core.WebServer.PleaseTakes.Redirect("/staff/");
						break;
				}
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/");
		}

	}

}