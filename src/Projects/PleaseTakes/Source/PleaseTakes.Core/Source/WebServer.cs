using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core {

	internal static class WebServer {

		public static System.Web.HttpApplicationState Application {
			get {
				return System.Web.HttpContext.Current.Application;
			}
		}

		public static System.Web.Caching.Cache Cache {
			get {
				return System.Web.HttpContext.Current.Cache;
			}
		}

		public static System.Web.HttpRequest Request {
			get {
				return System.Web.HttpContext.Current.Request;
			}
		}

		public static System.Web.HttpResponse Response {
			get {
				return System.Web.HttpContext.Current.Response;
			}
		}

		public static System.Web.HttpServerUtility Server {
			get {
				return System.Web.HttpContext.Current.Server;
			}
		}

		public static System.Web.SessionState.HttpSessionState Session {
			get {
				return System.Web.HttpContext.Current.Session;
			}
		}

		internal static class PleaseTakes {

			public static void Redirect(string path) {
				WebServer.Response.Redirect("?path=" + path);
			}

			internal static class Application {

				public static Persistence.Application CurrentInstance {
					get {
						return (Persistence.Application)WebServer.Application["PleaseTakes.Application"];
					}
				}

				public static void CreateNewInstance() {
					WebServer.Application["PleaseTakes.Application"] = new Persistence.Application();
				}

				public static void ScrubInstance() {
					WebServer.Application["PleaseTakes.Application"] = null;
				}

				public static bool Exists() {
					try {
						if (!CurrentInstance.GetType().ToString().Equals("PleaseTakes.Core.Persistence.Application"))
							return false;
					}
					catch (System.NullReferenceException) {
						return false;
					}

					return true;
				}

			}

			internal static class Session {

				public static Persistence.Session CurrentInstance {
					get {
						return (Persistence.Session)WebServer.Session["PleaseTakes.Session"];
					}
				}

				public static void CreateNewInstance(Schools.School school) {
					WebServer.Session["PleaseTakes.Session"] = new Persistence.Session(school);
					WebServer.Session.Timeout = WebServer.PleaseTakes.Application.CurrentInstance.Defaults.Timeout;
				}

				public static void ScrubInstance() {
					WebServer.Session["PleaseTakes.Session"] = null;
				}

				public static bool Exists() {
					try {
						if (!WebServer.PleaseTakes.Session.CurrentInstance.GetType().ToString().Equals("PleaseTakes.Core.Persistence.Session"))
							return false;
					}
					catch (System.NullReferenceException) {
						return false;
					}

					return true;
				}

			}

		}

	}

}
