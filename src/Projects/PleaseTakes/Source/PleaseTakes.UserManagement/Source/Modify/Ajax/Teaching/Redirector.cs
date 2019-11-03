using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Modify.Ajax.Teaching {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext())
				switch (path.Next()) {
					case "department":
						if (path.HasNext() && path.Peek().Equals("status"))
							new DepartmentSelectionStatus(path);
						else
							new DepartmentSelection(path);
						break;
					case "timetable":
						if (path.HasNext() && path.Peek().Equals("selection")) {
							path.Next();

							if (path.HasNext() && path.Peek().Equals("status"))
								new TimetableSelectionStatus(path);
							else
								new TimetableSelection(path);
						}
						else
							new Timetable(path);
						break;
					default:
						Core.WebServer.PleaseTakes.Redirect("/staff/");
						break;
				}
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/");
		}

	}

}