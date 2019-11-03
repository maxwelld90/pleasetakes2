using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Exception {

	internal static class Redirector {

		public static void Go() {
			Helpers.Path.Parser path = new Helpers.Path.Parser(WebServer.Request["path"]);
			System.Exception exception = Core.WebServer.Server.GetLastError().GetBaseException();

			switch (exception.GetType().ToString()) {
				case "PleaseTakes.Core.Helpers.NotLoggedInException":
					WebServer.PleaseTakes.Redirect("/login/notloggedin/");
					break;
				case "PleaseTakes.Core.Helpers.NotAdministrativeException":
					WebServer.Response.Write("ACCESS DENIED, YOU ARE NOT ADMIN.");
					break;
				case "PleaseTakes.Core.Helpers.ResourceNotFoundException":
					new ResourceNotFound();
					break;
				case "PleaseTakes.Core.Helpers.Database.DatabaseConnectionException":
					WebServer.Response.Write("Couldnt create a db connection! " + exception.Message);
					break;
				case "PleaseTakes.Core.Helpers.Database.DatabaseConnectionTestException":
					WebServer.Response.Write("Connection failed to database - " + exception.Message);
					break;
				default:
					if (path.IsAjax)
						Core.WebServer.Response.Write(exception.ToString());
					else
						new Unhandled();
					break;
			}

			//NotAdministrativeException
			//

			Core.WebServer.Server.ClearError();
		}

	}

}