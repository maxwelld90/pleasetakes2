using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers {

	internal static class Redirector {
		static Persistence.Session session = WebServer.PleaseTakes.Session.CurrentInstance;

		public static void Base(Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					// Temporary
					case "test":
						new BasicResponses.Test(path);
						break;
					case "home":
						if (session.IsLoggedIn)
							new BasicResponses.Home.Handler(path);
						else
							throw new NotLoggedInException();
						break;
					case "login":
						if (session.IsLoggedIn)
							WebServer.PleaseTakes.Redirect("/home/");
						else
							BasicResponses.Login.Redirector.Go(path);
						break;
					case "logout":
						if (session.IsLoggedIn)
							BasicResponses.Login.Action.LogoutUser();
						else
							WebServer.PleaseTakes.Redirect("/login/badlogout/");
						break;
					case "account":
						BasicResponses.Account.Redirector.Go(path);
						break;
					case "cover":
						if (session.IsLoggedIn)
							if (session.Account.IsAdmin)
								Cover.Redirector.Go(path);
							else
								throw new NotAdministrativeException(path.ToString());
						else
							throw new NotLoggedInException();
						break;
					case "staff":
						if (session.IsLoggedIn)
							if (session.Account.IsAdmin)
								UserManagement.Redirector.Go(path);
							else
								throw new NotAdministrativeException(path.ToString());
						else
							throw new NotLoggedInException();
						break;
					default:
						if (session.IsLoggedIn)
							WebServer.PleaseTakes.Redirect("/home/");	// Perhaps with a message?
						else
							WebServer.PleaseTakes.Redirect("/login/");
						break;
				}
			else {
				if (session.IsLoggedIn)
					WebServer.PleaseTakes.Redirect("/home/");
				else
					WebServer.PleaseTakes.Redirect("/login/");
			}
		}

	}

	public class NotLoggedInException : Exception {
		public NotLoggedInException() : base() { }
	}

	public class NotAdministrativeException : Exception {
		public NotAdministrativeException(string path) : base(path) { }
	}

}