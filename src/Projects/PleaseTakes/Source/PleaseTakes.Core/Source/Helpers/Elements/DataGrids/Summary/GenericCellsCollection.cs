using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Summary {

	internal sealed partial class Summary : DataGrid {

		internal sealed class GenericCellsCollection {
			private Cell _staffName;
			private Cell _presentAndFree;
			private Cell _presentAndFreeYeargroupAway;
			private Cell _presentIsBusy;
			private Cell _absent;
			private Cell _unavailable;

			public GenericCellsCollection() {
				this._staffName = new Cell();
				this._presentAndFree = new Cell();
				this._presentAndFreeYeargroupAway = new Cell();
				this._presentIsBusy = new Cell();
				this._absent = new Cell();
				this._unavailable = new Cell();

				this._presentAndFree.Value = "P";
				this._presentAndFree.Colour = CellColours.Green;

				this._presentAndFreeYeargroupAway.Value = "P";
				this._presentAndFreeYeargroupAway.Colour = CellColours.Blue;

				this._presentIsBusy.Value = "P";
				this._presentIsBusy.Colour = CellColours.Indigo;

				this._absent.Value = "A";
				this._absent.Colour = CellColours.Red;

				this._unavailable.Value = "U";
				this._unavailable.Colour = CellColours.Orange;
			}

			public Cell StaffName {
				get {
					return this._staffName;
				}
			}

			public Cell PresentAndFree {
				get {
					return this._presentAndFree;
				}
			}

			public Cell PresentAndFreeYeargroupAway {
				get {
					return this._presentAndFreeYeargroupAway;
				}
			}

			public Cell PresentIsBusy {
				get {
					return this._presentIsBusy;
				}
			}

			public Cell Absent {
				get {
					return this._absent;
				}
			}

			public Cell Unavailable {
				get {
					return this._unavailable;
				}
			}
		}

	}

}