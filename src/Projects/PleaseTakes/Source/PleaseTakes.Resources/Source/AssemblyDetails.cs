using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Resources {

	internal static class AssemblyDetails {

		private static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

		public static int Major {
			get {
				return version.Major;
			}
		}

		public static int Minor {
			get {
				return version.Minor;
			}
		}

		public static int Revision {
			get {
				return version.Revision;
			}
		}

		public static int Build {
			get {
				return version.Build;
			}
		}

		public static string Combined {
			get {
				return Major + "." + Minor + "." + Build + "." + Revision;
			}
		}

	}

}
