using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Modify.Ajax.Teaching {

	internal sealed class DepartmentSelectionStatus : Core.Helpers.BaseHandlers.AjaxHandler {

		public DepartmentSelectionStatus(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string message = "";

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffTeachingDepartmentSelection")) {
				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingDepartmentSelection"] == null)
					message = "I don't know what happened!";
				else {
					int count = (int)Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingDepartmentSelection"];

					if (count.Equals(0))
						message = "<strong>Couldnae find anything!</strong>";
					else
						if (count.Equals(1))
							message = "Found <strong>1</strong> department";
						else
							message = "Found <strong>" + count + "</strong> departments";

					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Remove("StaffTeachingDepartmentSelection");
				}
			}
			else {
				message = "<strong>Couldnae find anything!</strong>";
			}

			this.Page.Contents = message;
		}

	}

}