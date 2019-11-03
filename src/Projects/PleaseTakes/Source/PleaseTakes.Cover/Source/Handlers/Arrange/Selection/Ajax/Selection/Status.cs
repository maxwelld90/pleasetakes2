using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Handlers.Arrange.Selection.Ajax.Selection {

	internal sealed class Status : Core.Helpers.BaseHandlers.AjaxHandler {

		public Status(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			string message = "";

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("ArrangeSelectionInfo")) {
				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangeSelectionInfo"] == null)
					message = "I don't know what happened!";
				else
					message = (string)Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangeSelectionInfo"];
			}
			else
				message = "No request selected";

			this.Page.Contents = message;
		}

	}

}