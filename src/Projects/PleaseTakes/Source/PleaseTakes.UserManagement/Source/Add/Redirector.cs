using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Add {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "teaching":
						if (path.HasNext() && path.Next().Equals("timetable"))
							new Timetable(path);
						else
							new Teaching();
						break;
					case "nonteaching":
						Core.WebServer.Response.Write("non teaching ADD");
						break;
					case "outside":
						Core.WebServer.Response.Write("outside ADD");
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