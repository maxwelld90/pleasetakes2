using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Alerts {

	internal sealed class Alert {
		private string _id;
		private string _message;
		private Colours _colour;
		private bool _startHidden;
		private bool _showCloseBox;
		private bool _noScript;

		public Alert(string id) {
			this._id = id;
		}

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string Message {
			get {
				return this._message;
			}
			set {
				this._message = value;
			}
		}

		public Colours Colour {
			get {
				return this._colour;
			}
			set {
				this._colour = value;
			}
		}

		public bool StartHidden {
			get {
				return this._startHidden;
			}
			set {
				this._startHidden = value;
			}
		}

		public bool ShowCloseBox {
			get {
				return this._showCloseBox;
			}
			set {
				this._showCloseBox = value;
			}
		}

		public bool NoScript {
			get {
				return this._noScript;
			}
			set {
				this._noScript = value;
			}
		}

		public override string ToString() {
			Constructor alertConstructor = new Constructor("/Templates/Elements/Alerts/alert.html");

			if (this._showCloseBox)
				alertConstructor.SetVariable("CloseBox", new Constructor("/Templates/Elements/Alerts/closebox.html"));
			else
				alertConstructor.DeleteVariable("CloseBox");

			switch (this._colour) {
				case Colours.None:
					alertConstructor.SetVariable("Colour", "Blue");
					break;
				case Colours.Red:
					alertConstructor.SetVariable("Colour", "Red");
					break;
				case Colours.Yellow:
					alertConstructor.SetVariable("Colour", "Yellow");
					break;
				case Colours.Green:
					alertConstructor.SetVariable("Colour", "Green");
					break;
			}

			if (this._startHidden)
				alertConstructor.SetVariable("StartHidden", " Hidden");
			else
				alertConstructor.DeleteVariable("StartHidden");

			alertConstructor.SetVariable("Message", this._message);
			alertConstructor.SetVariable("Id", this._id);

			if (this._noScript) {
				alertConstructor.SetVariable("NoScript", "<noscript>");
				alertConstructor.SetVariable("NoScriptEnd", "</noscript>");
			}
			else {
				alertConstructor.DeleteVariable("NoScript");
				alertConstructor.DeleteVariable("NoScriptEnd");
			}

			return alertConstructor.ToString();
		}

	}

}