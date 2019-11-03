using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core {

	public static class Landing {

		public static void Launch() {
			Helpers.InitialisationMethods.ApplicationInstanceCheck();
			Helpers.Path.Parser path = new Helpers.Path.Parser(WebServer.Request["path"]);

			if (!Helpers.InitialisationMethods.RequestSessionExempt(path))
				if (WebServer.PleaseTakes.Session.Exists())
					Helpers.InitialisationMethods.SessionExists(path);
				else
					Helpers.InitialisationMethods.NoSessionExists(path);
		}

		public static void Exception() {
			BasicResponses.Exception.Redirector.Go();
		}

	}

}