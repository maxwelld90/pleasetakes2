using System;

namespace PleaseTakes.Core.Helpers.Xml {

	internal static class Formatting {

		public static string EncodeString(string value) {
			return WebServer.Server.HtmlEncode(value);
		}

		public static string DecodeString(string value) {
			return WebServer.Server.HtmlDecode(value);
		}

		public static string FormatBool(bool value) {
			return value.ToString().ToLower();
		}

	}

}
