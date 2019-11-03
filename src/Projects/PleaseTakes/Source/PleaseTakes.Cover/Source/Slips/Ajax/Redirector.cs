using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Slips.Ajax {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "calendar":
						new Ajax.Landing.Calendar(path);
						break;
					case "isvalid":
						new Landing.IsValid(path);
						break;
					case "requests":
						if (path.HasNext() && path.Peek().Equals("status"))
							Core.WebServer.Response.Write("requests status");
						else
							new Landing.Requests(path);
						break;
					default:
						Core.WebServer.PleaseTakes.Redirect("/cover/slips/");
						break;
				}
			else
				Core.WebServer.PleaseTakes.Redirect("/cover/slips/");
		}

	}

}
