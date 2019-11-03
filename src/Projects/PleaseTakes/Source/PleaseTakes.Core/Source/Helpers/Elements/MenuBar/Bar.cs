using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.MenuBar {

	internal sealed class Bar {
		private List<Button> _buttons;
		private string _text;

		public Bar() {
			this._buttons = new List<Button>();
		}

		public string Text {
			get {
				return this._text;
			}
			set {
				this._text = value;
			}
		}

		public void AddButton(string src, string toolTip, string href, string onClick) {
			Button newButton = new Button();
			newButton.Src = src;
			newButton.ToolTip = toolTip;
			newButton.Href = href;
			newButton.OnClick = onClick;

			this._buttons.Add(newButton);
		}

		public override string ToString() {
			Constructor bar = new Constructor("/Templates/Elements/Menubar/bar.html");

			if (this._buttons.Count.Equals(0))
				bar.DeleteVariable("Buttons");
			else {
				string buttonsStr = "";

				for (int i = 0; i <= (this._buttons.Count - 1); i++) {
					if (i.Equals(0))
						this._buttons[0].HasLeftMargin = false;

					buttonsStr += this._buttons[i];
				}

				bar.SetVariable("Buttons", buttonsStr);
			}

			if (string.IsNullOrEmpty(this._text))
				bar.DeleteVariable("Text");
			else
				bar.SetVariable("Text", "<span>" + this._text + "</span>");

			return bar.ToString();
		}
	}

}