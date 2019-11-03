using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Landing.Ajax.NonTeaching {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {

			if (path.HasNext())
				switch (path.Next()) {
					case "search":
						new Standard(path);
						break;
					case "status":
						new Status(path);
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