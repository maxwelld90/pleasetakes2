using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Calendar {

	internal sealed partial class Calendar : DataGrid {

		internal sealed class CalendarEvents {
			private Cell _past;
			private Cell _noDay;
			private Cell _todayInRange;
			private Cell _inRange;
			private Cell _outOfRange;

			public CalendarEvents() {
				this._past = new Cell();
				this._noDay = new Cell();
				this._todayInRange = new Cell();
				this._inRange = new Cell();
				this._outOfRange = new Cell();
			}

			public Cell Past {
				get {
					return this._past;
				}
			}

			public Cell NoDay {
				get {
					return this._noDay;
				}
			}

			public Cell TodayInRange {
				get {
					return this._todayInRange;
				}
			}

			public Cell InRange {
				get {
					return this._inRange;
				}
			}

			public Cell OutOfRange {
				get {
					return this._outOfRange;
				}
			}
		}

	}

}