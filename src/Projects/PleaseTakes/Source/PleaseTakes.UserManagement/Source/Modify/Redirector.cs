using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Modify {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "teaching":
						if (path.HasNext() && path.Peek().Equals("timetable"))
							new Teaching.Timetable(path);
						else
							new Teaching.Standard(path);
						break;
					case "nonteaching":
						Core.WebServer.Response.Write("non teaching");
						break;
					case "outside":
						Core.WebServer.Response.Write("outside");
						break;
					case "ajax":
						Ajax.Redirector.Go(path);
						break;
					case "update":
						Update.Redirector.Go(path);
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