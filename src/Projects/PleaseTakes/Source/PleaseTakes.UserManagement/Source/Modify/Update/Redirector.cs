using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Modify.Update {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "teaching":
						Teaching.Redirector.Go(path);
						break;
					case "nonteaching":
						Core.WebServer.Response.Write("non teaching UPDATE");
						break;
					case "outside":
						Core.WebServer.Response.Write("outside UPDATE");
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