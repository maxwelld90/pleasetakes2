using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Handlers.Arrange.Attendance.Ajax.Periods {

	internal sealed class Status : Core.Helpers.BaseHandlers.AjaxHandler {

		public Status(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string message = "";

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("ArrangePeriodCount")) {
				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangePeriodCount"] == null)
					message = "I don't know what happened!";
				else {
					int count = (int)Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangePeriodCount"];

					if (count.Equals(0))
						message = "<strong>Couldnae find anything!</strong>";
					else
						if (count.Equals(1))
							message = "Found <strong>1</strong> attendance record";
						else
							message = "Found <strong>" + count + "</strong> attendance records";

					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Remove("ArrangePeriodCount");
				}
			}
			else {
				message = "<strong>Couldnae find anything!</strong>";
			}

			this.Page.Contents = message;
		}

	}

}
