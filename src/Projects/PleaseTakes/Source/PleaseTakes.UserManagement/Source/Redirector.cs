using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext()) {
				switch (path.Next()) {
					case "ajax":
						Landing.Ajax.Redirector.Go(path);
						break;
					case "modify":
						Modify.Redirector.Go(path);
						break;
					case "add":
						Add.Redirector.Go(path);
						break;
					default:
						new Landing.Standard(path);
						break;
				}
			}
			else
				new Landing.Standard(path);
		}

	}

}