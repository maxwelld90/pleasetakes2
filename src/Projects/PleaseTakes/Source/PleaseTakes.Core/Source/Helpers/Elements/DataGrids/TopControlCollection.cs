using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids {

	internal sealed class TopControlCollection {
		private TopControl _left;
		private TopControl _centre;
		private TopControl _right;

		public TopControlCollection() {
			this._left = new TopControl(TopControlPositions.Left);
			this._centre = new TopControl(TopControlPositions.Centre);
			this._right = new TopControl(TopControlPositions.Right);
		}

		public TopControl Left {
			get {
				return this._left;
			}
		}

		public TopControl Centre {
			get {
				return this._centre;
			}
		}

		public TopControl Right {
			get {
				return this._right;
			}
		}

		public bool IsEmpty {
			get {
				return (this._left.IsEmpty && this._centre.IsEmpty && this._right.IsEmpty);
			}
		}

		public override string ToString() {
			if (this.IsEmpty)
				return null;
			else {
				string returnStr = "<div class=\"Top\">";

				if (!this._left.IsEmpty)
					returnStr += this._left.ToString();

				if (!this._centre.IsEmpty)
					returnStr += this._centre.ToString();

				if (!this._right.IsEmpty)
					returnStr += this._right.ToString();

				return returnStr + "</div>";
			}
		}
	}

}