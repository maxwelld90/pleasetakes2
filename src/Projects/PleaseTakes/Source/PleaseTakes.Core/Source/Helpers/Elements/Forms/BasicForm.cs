using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class BasicForm : Form {
		private Constructor _rightPane;
		private List<Row> _rows;

		public BasicForm() {
			this._rightPane = new Constructor();
			this._rows = new List<Row>();
		}

		public BasicForm(string rightPaneTemplatePath) {
			this._rightPane = new Constructor(rightPaneTemplatePath);
			this._rows = new List<Row>();
		}

		public Constructor RightPane {
			get {
				return this._rightPane;
			}
		}

		public Row AddRow() {
			Row newRow = new Row(this);
			this._rows.Add(newRow);

			return newRow;
		}

		public override string ToString() {
			Constructor baseConstructor = new Constructor();
			baseConstructor.Contents = base.ToString();
			string rows = "";

			if (!this._rows.Count.Equals(0))
				foreach (Row row in this._rows)
					rows += row;
			
			if (!this._rightPane.IsEmpty) {
				Constructor panes = new Constructor("/Templates/Elements/Forms/panes.html");
				panes.DeleteVariable("Id");
				panes.SetVariable("Class", " class=\"BasicPaneContainer\"");
				panes.SetVariable("Rows", rows);
				panes.SetVariable("RightPane", this._rightPane);

				baseConstructor.SetVariable("Contents", panes.ToString());
				return baseConstructor.ToString();
			}
			else {
				baseConstructor.SetVariable("Contents", rows);
				return baseConstructor.ToString();
			}
		}
	}

}