using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Landing.Ajax.Outside {

	internal sealed class Status : Core.Helpers.BaseHandlers.AjaxHandler {

		public Status(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string message = "";

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffOutsideCount")) {
				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffOutsideCount"] == null)
					message = "I don't know what happened!";
				else {
					int count = (int)Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffOutsideCount"];

					if (count.Equals(0))
						message = "<strong>Couldnae find anyone!</strong>";
					else
						if (count.Equals(1))
							message = "Found <strong>1</strong> outside cover staff member";
						else
							message = "Found <strong>" + count + "</strong> outside cover staff members";

					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Remove("StaffOutsideCount");
				}
			}
			else {
				message = "<strong>Couldnae find anyone!</strong>";
			}

			this.Page.Contents = message;
		}

	}

}