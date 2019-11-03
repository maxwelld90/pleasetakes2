using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids {

	internal sealed class Row : CollectionBase<Cell> {
		private DataGrid _parent;

		public DataGrid Parent {
			set {
				this._parent = value;
			}
		}

		public override string ToString() {
			Constructor row = new Constructor("/Templates/Elements/Datagrids/row.html");
			string cells = "";
			int remaining = this._parent.Width - 1;

			for (int i = 0; (i <= (this.Count - 1)); i++) {
				cells += this[i];
				remaining--;
			}

			for (int i = 0; (i <= remaining); i++)
				cells += Cell.BlankCell();

			row.SetVariable("Cells", cells);
			return row.ToString();
		}
	}

}