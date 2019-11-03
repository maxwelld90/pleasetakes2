using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Modify.Ajax {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "teaching":
						Teaching.Redirector.Go(path);
						break;
					case "nonteaching":
						
						break;
					case "outside":
						
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