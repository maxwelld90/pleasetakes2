using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids {

	internal abstract class DataGrid {
		private Constructor _constructor;
		private string _id;
		private int _width;
		private TopControlCollection _topControls;
		private Constructor _textMessage;
		private Rows _rows;
		private Cell _commonCell;

		public DataGrid() {
			this._constructor = new Constructor("/Templates/Elements/Datagrids/datagrid.html");
			this._topControls = new TopControlCollection();
			this._textMessage = new Constructor();
			this._rows = new Rows();
			this._commonCell = new Cell();
		}

		protected Constructor Constructor {
			get {
				return this._constructor;
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

		public TopControlCollection TopControls {
			get {
				return this._topControls;
			}
		}

		public int Width {
			get {
				return this._width;
			}
			set {
				this._width = value;
			}
		}

		protected Constructor TextMessage {
			get {
				return this._textMessage;
			}
			set {
				this._textMessage = value;
			}
		}

		public Rows Rows {
			get {
				return this._rows;
			}
		}

		public Cell CommonCell {
			get {
				return this._commonCell;
			}
			set {
				this._commonCell = value;
			}
		}

		public void AddRow() {
			this._rows.Add(this);
		}

		public override string ToString() {
			if (string.IsNullOrEmpty(this._textMessage.Contents)) {
				this._constructor.SetVariable("Rows", this._rows.ToString());

				if (this._topControls.IsEmpty)
					this._constructor.DeleteVariable("TopControls");
				else
					this._constructor.SetVariable("TopControls", this._topControls.ToString());

				if (string.IsNullOrEmpty(this._id))
					this._constructor.DeleteVariable("Id");
				else
					this._constructor.SetVariable("Id", " id=\"DataGrid" + this._id + "\"");

				return this._constructor.ToString();
			}
			else {
				return this._textMessage.ToString();
			}
		}
		
	}

}