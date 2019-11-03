using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Account {

	internal static class Redirector {
		static Persistence.Session session = WebServer.PleaseTakes.Session.CurrentInstance;

		public static void Go(Core.Helpers.Path.Parser path) {
			if (session.IsLoggedIn)
				if (path.HasNext())
					switch (path.Peek()) {
						case "update":
							path.Next();

							if (path.HasNext() && path.Next().Equals("password"))
								new PasswordChange();
							else
								Core.WebServer.PleaseTakes.Redirect("/account/");

							break;
						default:
							new Standard(path);
							break;
					}
				else
					new Standard(path);
			else
				throw new Core.Helpers.NotLoggedInException();
		}

	}

}