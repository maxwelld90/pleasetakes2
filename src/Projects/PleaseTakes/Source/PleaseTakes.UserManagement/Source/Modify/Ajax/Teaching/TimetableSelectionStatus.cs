using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Modify.Ajax.Teaching {

	internal sealed class TimetableSelectionStatus : Core.Helpers.BaseHandlers.AjaxHandler {

		public TimetableSelectionStatus(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string message = "";

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffTeachingTimetableSelection")) {
				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingTimetableSelection"] == null)
					message = "I don't know what happened!";
				else {
					message = Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingTimetableSelection"] as string;
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Remove("StaffTeachingTimetableSelection");
				}
			}

			this.Page.Contents = message;
		}

	}

}