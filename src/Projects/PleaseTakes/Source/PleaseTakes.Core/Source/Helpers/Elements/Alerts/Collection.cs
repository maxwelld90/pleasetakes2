using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Alerts {

	internal sealed class Collection : CollectionBase<Alert> {

		public void Add(string id, string message, Colours colour, bool startHidden, bool showCloseBox) {
			Alert newAlert = new Alert(id);
			newAlert.Message = message;
			newAlert.Colour = colour;
			newAlert.StartHidden = startHidden;
			newAlert.ShowCloseBox = showCloseBox;
			newAlert.NoScript = false;

			this.Add(newAlert);
		}

		public void AddNoScriptAlert() {
			Alert noScriptAlert = new Alert("NoScript");
			noScriptAlert.Message = new Constructor("/Alerts/noscript.html").ToString();
			noScriptAlert.Colour = Colours.Yellow;
			noScriptAlert.StartHidden = false;
			noScriptAlert.ShowCloseBox = false;
			noScriptAlert.NoScript = true;

			this.Add(noScriptAlert);
		}

		public override string ToString() {
			string returnStr = "";

			foreach (Alert alert in this.Collection)
				returnStr += alert;

			return returnStr;
		}

	}
	

}