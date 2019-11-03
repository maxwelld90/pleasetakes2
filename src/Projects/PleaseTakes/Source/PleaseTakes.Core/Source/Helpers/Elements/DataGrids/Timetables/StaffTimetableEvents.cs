using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Timetables {

	internal sealed partial class StaffTimetable : Timetable {

		internal sealed class StaffTimetableEvents {
			private Cell _unavailable;
			private Cell _free;
			private Cell _busyTeaching;
			private Cell _busyOther;

			public StaffTimetableEvents() {
				this._unavailable = new Cell();
				this._free = new Cell();
				this._busyTeaching = new Cell();
				this._busyOther = new Cell();

				this._unavailable.Colour = CellColours.Orange;
				this._unavailable.Type = CellTypes.StandardLarge;
				this._unavailable.Value = "U";

				this._free.Colour = CellColours.Green;
				this._free.Type = CellTypes.StandardSmall;
				this._free.Value = "Free";

				this._busyTeaching.Colour = CellColours.Red;
				this._busyTeaching.Type = CellTypes.Standard;

				this._busyOther.Colour = CellColours.Indigo;
				this._busyOther.Type = CellTypes.Standard;
			}

			public Cell Unavailable {
				get {
					return this._unavailable;
				}
			}

			public Cell Free {
				get {
					return this._free;
				}
			}

			public Cell BusyTeaching {
				get {
					return this._busyTeaching;
				}
			}

			public Cell BusyOther {
				get {
					return this._busyOther;
				}
			}
		}

	}

}