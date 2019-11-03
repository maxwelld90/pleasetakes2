using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Handlers.Arrange.Attendance.Ajax.Attendance {

	internal sealed class Status : Core.Helpers.BaseHandlers.AjaxHandler {

		public Status(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string message = "";

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("ArrangeAttendanceCount")) {
				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangeAttendanceCount"] == null)
					message = "I don't know what happened!";
				else {
					int count = (int)Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangeAttendanceCount"];

					if (count.Equals(0))
						message = "<strong>Couldnae find anyone!</strong>";
					else
						if (count.Equals(1))
							message = "Found <strong>1</strong> teacher";
						else
							message = "Found <strong>" + count + "</strong> teaching staff";

					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Remove("ArrangeAttendanceCount");
				}
			}
			else {
				message = "<strong>Couldnae find anyone!</strong>";
			}

			this.Page.Contents = message;
		}

	}

}
