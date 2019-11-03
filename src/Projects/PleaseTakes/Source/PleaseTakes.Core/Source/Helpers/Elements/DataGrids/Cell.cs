using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids {

	internal class Cell {
		private DataGrid _parent;
		private CellTypes _type;
		private CellColours _colour;
		private string _id;
		private string _value;
		private string _spanValue;
		private string _href;
		private string _onClick;

		public static Cell BlankCell() {
			Cell blankCell = new Cell();
			blankCell.Type = CellTypes.None;

			return blankCell;
		}

		public Cell() { }

		public Cell(string id) {
			this._id = id;
		}

		public DataGrid Parent {
			set {
				this._parent = value;
			}
		}

		public CellTypes Type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		public CellColours Colour {
			get {
				return this._colour;
			}
			set {
				this._colour = value;
			}
		}

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string Value {
			get {
				return this._value;
			}
			set {
				this._value = value;
			}
		}

		public string SpanValue {
			get {
				return this._spanValue;
			}
			set {
				this._spanValue = value;
			}
		}

		public string Href {
			get {
				return this._href;
			}
			set {
				this._href = value;
			}
		}

		public string OnClick {
			get {
				return this._onClick;
			}
			set {
				this._onClick = value;
			}
		}

		public override string ToString() {
			Constructor cell = new Constructor("/Templates/Elements/Datagrids/cell.html");

			if ((this._type == CellTypes.Header) || (this._type == CellTypes.HeaderSmall))
				cell.SetVariable("Type", "th");
			else
				cell.SetVariable("Type", "td");

			if (string.IsNullOrEmpty(this._id))
				cell.DeleteVariable("Id");
			else
				if ((this._parent != null) && (!string.IsNullOrEmpty(this._parent.Id)))
					cell.SetVariable("Id", " id=\"" + this._parent.Id + this._id + "\"");

			string classesStr = "";

			if (this._type == CellTypes.StandardWithSpan)
				classesStr = "Span ";

			if (this._type == CellTypes.StandardLarge)
				classesStr += "Large ";

			if (this._type == CellTypes.StandardSmall)
				classesStr += "Small ";

			if (this._type == CellTypes.HeaderSmall)
				classesStr += "Small ";

			switch (this._colour) {
				case CellColours.Red:
					classesStr += "Red";
					break;
				case CellColours.Orange:
					classesStr += "Orange";
					break;
				case CellColours.Yellow:
					classesStr += "Yellow";
					break;
				case CellColours.Green:
					classesStr += "Green";
					break;
				case CellColours.Blue:
					classesStr += "Blue";
					break;
				case CellColours.Indigo:
					classesStr += "Indigo";
					break;
				case CellColours.Violet:
					classesStr += "Violet";
					break;
			}

			classesStr = classesStr.TrimEnd(' ');

			if (string.IsNullOrEmpty(classesStr))
				cell.DeleteVariable("Classes");
			else
				cell.SetVariable("Classes", " class=\"" + classesStr + "\"");

			if (this._type == CellTypes.None) {
				cell.SetVariable("Value", "&nbsp;");
				cell.DeleteVariable("SpanValue");
				cell.DeleteVariable("StartLink");
				cell.DeleteVariable("EndLink");
			}
			else {
				if (!(string.IsNullOrEmpty(this._href)) || !(string.IsNullOrEmpty(this._onClick))) {
					string a = "<a";

					if (string.IsNullOrEmpty(this._href))
						a += " href=\"javascript:void(0);\"";
					else
						a += " href=\"?path=" + this._href + "\"";

					if (!string.IsNullOrEmpty(this._onClick))
						a += " onclick=\"" + this._onClick + "\"";

					cell.SetVariable("StartLink", a + ">");
					cell.SetVariable("EndLink", "</a>");
				}
				else {
					cell.DeleteVariable("StartLink");
					cell.DeleteVariable("EndLink");
				}

				if (string.IsNullOrEmpty(this._value))
					cell.SetVariable("Value", "&nbsp;");
				else
					cell.SetVariable("Value", this._value);

				if (this._type == CellTypes.StandardWithSpan)
					if (string.IsNullOrEmpty(this._spanValue))
						cell.DeleteVariable("SpanValue");
					else
						cell.SetVariable("SpanValue", "<span>" + this._spanValue + "</span>");
				else
					cell.DeleteVariable("SpanValue");
			}

			return cell.ToString();
		}
	}

}