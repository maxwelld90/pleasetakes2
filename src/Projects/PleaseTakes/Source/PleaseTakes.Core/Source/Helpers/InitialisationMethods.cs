using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers {

	internal static class InitialisationMethods {

		public static void ApplicationInstanceCheck() {
			if (!WebServer.PleaseTakes.Application.Exists())
				WebServer.PleaseTakes.Application.CreateNewInstance();
		}

		public static bool RequestSessionExempt(Helpers.Path.Parser path) {
			if (!path.IsEmpty) {
				string firstElement = path.Peek();

				switch (firstElement) {
					case "resources":
						path.RemoveAt(0);

						Output.OutputBase response;
						Helpers.Resource resource = new Helpers.Resource(path);

						if (resource.IsBinary)
							response = new Output.BinaryResource(resource);
						else
							response = new Output.StringResource(resource);

						response.PrepareOutput();
						response.Send();
						return true;
					case "selection":
						WebServer.Response.Write("<h1>Selection Screen</h1>");
						return true;
					case "quote":
						new BaseHandlers.QuoteHandler();
						return true;
				}
			}

			return false;
		}

		public static void CreateSessionInstance(Path.Parser path, Schools.School school) {
			WebServer.PleaseTakes.Session.CreateNewInstance(school);

			if (path.IsEmpty)
				Redirects.ToLogin(path);
			else
				if (path.Contains("dologin"))
					BasicResponses.Login.Action.AttemptLogin();
				else
					WebServer.PleaseTakes.Redirect(path.ToString());
			
			// This is what is was before the minor modifications (2010-08-12)
			//Redirects.ToLogin(path);
		}

		public static void SessionExists(Path.Parser path) {
			Redirects.Standard(path);
		}

		public static void NoSessionExists(Path.Parser path) {
			if (SchoolsInformation.MoreThanOneAvailable(path))
				if (SchoolsInformation.NoDefaultExists(path))
					if (SchoolsInformation.QueryStringBlank())
						Redirects.ToSelectionScreen(path, false);
					else
						if (SchoolsInformation.QueryStringIsValid())
							CreateSessionInstance(path, WebServer.PleaseTakes.Application.CurrentInstance.Schools[WebServer.Request["schoolID"]]);
						else
							Redirects.ToSelectionScreen(path, true);
		}

		internal static class SchoolsInformation {

			public static bool IsValid(string schoolID) {
				return WebServer.PleaseTakes.Application.CurrentInstance.Schools.Exists(schoolID);
			}

			public static bool QueryStringBlank() {
				return string.IsNullOrEmpty(WebServer.Request["schoolID"]);
			}

			public static bool QueryStringIsValid() {
				if (!QueryStringBlank())
					return (WebServer.PleaseTakes.Application.CurrentInstance.Schools.Exists(WebServer.Request["schoolID"]));

				return false;
			}

			public static bool MoreThanOneAvailable(Path.Parser path) {
				if (WebServer.PleaseTakes.Application.CurrentInstance.Schools.IsEmpty)
					throw new PleaseTakesInitialisationException("No schools exist in the permitted list. Contact David.");
				else if (WebServer.PleaseTakes.Application.CurrentInstance.Schools.Count.Equals(1)) {
					CreateSessionInstance(path, WebServer.PleaseTakes.Application.CurrentInstance.Schools.OnlySchool);
					return false;
				}

				return true;
			}

			public static bool NoDefaultExists(Path.Parser path) {
				string defaultValue = WebServer.PleaseTakes.Application.CurrentInstance.Defaults.SchoolID;

				if (!string.IsNullOrEmpty(defaultValue)) {
					if (IsValid(defaultValue)) {
						CreateSessionInstance(path, WebServer.PleaseTakes.Application.CurrentInstance.Schools[defaultValue]);
						return false;
					}
				}
				else
					return true;

				throw new PleaseTakesInitialisationException("An unrecognised school ID was supplied as the default school.");
			}

		}

		internal static class Redirects {

			public static void ToSelectionScreen(Path.Parser path, bool showInvalidSchoolIDMessage) {
				if (path.IsAjax)
					WebServer.Response.Write(Helpers.Elements.Ajax.Alerts.NoSchoolExpiration().ToString());
				else
					if (showInvalidSchoolIDMessage)
						WebServer.Response.Write("<H1>REQUEST TO SELECTION SCREEN W/ ERROR</H1>");
					else
						WebServer.Response.Write("<H1>REQUEST TO SELECTION SCREEN</H1>");
			}

			public static void Standard(Path.Parser path) {
				Redirector.Base(path);
			}

			public static void ToLogin(Path.Parser path) {
				if (path.IsAjax)
					WebServer.Response.Write(Helpers.Elements.Ajax.Alerts.Expiration().ToString());
				else
					WebServer.PleaseTakes.Redirect("/login/");
			}

		}

	}

}