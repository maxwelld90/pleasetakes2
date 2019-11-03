using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Slips {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "ajax":
						Ajax.Redirector.Go(path);
						break;
					case "printouts":
						if (path.HasNext())
							switch (path.Next()) {
								case "day":
									new Printouts.Day(path);
									break;
								default:
									Core.WebServer.PleaseTakes.Redirect("/cover/slips/");
									break;
							}
						else
							Core.WebServer.PleaseTakes.Redirect("/cover/slips/");
						break;
					default:
						Core.WebServer.Response.Write("Coming soon! To print slips, click the printer in the final step of arranging cover.");
						//new Landing.Standard(path);
						break;
				}
			else
				Core.WebServer.Response.Write("Coming soon! To print slips, click the printer in the final step of arranging cover.");
				//new Landing.Standard(path);
		}

	}

}
