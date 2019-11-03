using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class Group {
		private GroupedForm _parent;
		private string _name;
		private string _radioValue;
		private bool _isDefault;
		public int _cellWidths;
		private Constructor _rightPane;
		private GroupTitleDetails _titleDetails;
		private List<Row> _rows;

		public Group(GroupedForm parent, string name) {
			this._parent = parent;
			this._name = name;
			this._titleDetails = new GroupTitleDetails(this);
			this._rows = new List<Row>();
			this._rightPane = new Constructor();
		}

		public Group(GroupedForm parent, string name, string rightPaneTemplatePath) {
			this._parent = parent;
			this._name = name;
			this._titleDetails = new GroupTitleDetails(this);
			this._rows = new List<Row>();
			this._rightPane = new Constructor(rightPaneTemplatePath);
		}

		public GroupedForm Parent {
			get {
				return this._parent;
			}
		}

		public string Name {
			get {
				return this._name;
			}
		}

		public string RadioValue {
			get {
				return this._radioValue;
			}
			set {
				this._radioValue = value;
			}
		}

		public bool IsDefault {
			get {
				return this._isDefault;
			}
			set {
				this._isDefault = value;
			}
		}

		public int CellWidths {
			get {
				return this._cellWidths;
			}
			set {
				this._cellWidths = value;
			}
		}

		public Constructor RightPane {
			get {
				return this._rightPane;
			}
		}

		public GroupTitleDetails TitleDetails {
			get {
				return this._titleDetails;
			}
		}

		public Row AddRow() {
			Row newRow = new Row(this._parent);
			this._rows.Add(newRow);

			return newRow;
		}

		public override string ToString() {
			Constructor group = new Constructor("/Templates/Elements/Forms/group.html");

			group.SetVariable("FormId", this._parent.Id);
			group.SetVariable("GroupName", this._name);
			group.SetVariable("GroupTitleDetails", this._titleDetails.ToString());
			string containerId = "Form" + this._parent.Id + "-" + this._name + "-Elements";
			string rowsStr = "";

			if (this._rows.Count.Equals(0))
				group.DeleteVariable("Elements");
			else {
				foreach (Row row in this._rows)
					rowsStr += row;

				if (this._rightPane.IsEmpty) {
					Constructor basic = new Constructor("/Templates/Elements/Forms/groupelements.html");
					basic.SetVariable("Id", containerId);
					basic.SetVariable("Rows", rowsStr);

					group.SetVariable("Elements", basic.ToString());
				}
				else {
					Constructor panes = new Constructor("/Templates/Elements/Forms/panes.html");
					panes.SetVariable("Id", " id=\"" + containerId + "\" ");
					panes.SetVariable("Class", "class=\"Elements\"");

					panes.SetVariable("Rows", rowsStr);
					panes.SetVariable("RightPane", this._rightPane);

					group.SetVariable("Elements", panes.ToString());
				}
			}

			return group.ToString();
		}
	}

}