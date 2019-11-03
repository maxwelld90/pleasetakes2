using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids {

	internal sealed class Rows : CollectionBase<Row> {

		public void Add(DataGrid parent) {
			Row newRow = new Row();
			newRow.Parent = parent;

			this.Add(newRow);
		}

		public Row AddAndReturn(DataGrid parent) {
			Row newRow = new Row();
			newRow.Parent = parent;

			this.Add(newRow);
			return newRow;
		}

		public override string ToString() {
			string returnStr = "";

			foreach (Row row in this.Collection) {
				returnStr += row;
			}

			return returnStr;
		}

	}

}