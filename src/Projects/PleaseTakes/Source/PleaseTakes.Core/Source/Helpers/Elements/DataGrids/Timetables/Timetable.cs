using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Timetables {

	internal abstract class Timetable : DataGrid {
		private int _weekNo;
		private bool _isCurrentSession;

		public int WeekNo {
			get {
				return this._weekNo;
			}
			set {
				this._weekNo = value;
				this.Width = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this._weekNo].HighestPeriodValue + 1;
				this.SetCells();
			}
		}

		public bool IsCurrentSession {
			get {
				return this._isCurrentSession;
			}
			set {
				this._isCurrentSession = value;
			}
		}

		private void SetCells() {
			this.SetHeaderCells();

			int rowCounter = 1;

			for (int i = 1; i <= 7; i++) {
				int periodCount;

				try {
					periodCount = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this._weekNo][i];
					this.Rows.Add(this);
					this.Rows[rowCounter].Add(this.CreateDayCell(i));

					for (int j = 1; j <= periodCount; j++) {
						Cell dayCell = new Cell();
						dayCell.Type = CellTypes.Standard;

						this.Rows[rowCounter].Add(dayCell);
					}

					rowCounter++;
				}
				catch (Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) {}
			}

		}

		private Cell CreateDayCell(int dayNo) {
			Cell dayCell = new Cell();
			dayCell.Type = CellTypes.Header;
			dayCell.Value = Core.Utils.GetWeekDayAbbreviation((DayOfWeek)(dayNo- 1));

			return dayCell;
		}

		private void SetHeaderCells() {
			this.Rows.Add(this);

			for (int i = 0; i <= Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this._weekNo].HighestPeriodValue; i++) {
				Cell headerCell = new Cell();
				headerCell.Type = CellTypes.Header;

				if (!i.Equals(0))
					headerCell.Value = i.ToString();

				this.Rows[0].Add(headerCell);
			}
		}

		private void SetControls() {
			this.TopControls.Centre.Value = "Rotation Week " + this._weekNo;

			if (this._weekNo > 1) {
				this.TopControls.Left.Value = "Week " + (this._weekNo - 1);
				this.SetValues(this.TopControls.Left, false);
			}

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoWeeks > this._weekNo) {
				this.TopControls.Right.Value = "Week " + (this._weekNo + 1);
				this.SetValues(this.TopControls.Right, true);
			}
		}

		private void SetValues(TopControl control, bool add) {
			int linkValue = this._weekNo;

			if (add)
				linkValue++;
			else
				linkValue--;

			if (!string.IsNullOrEmpty(control.Href))
				control.Href = control.Href.Replace("WeekNo", linkValue.ToString());

			if (!string.IsNullOrEmpty(control.OnClick))
				control.OnClick = control.OnClick.Replace("WeekNo", linkValue.ToString());
		}

		public override string ToString() {
			this.SetControls();
			


			return base.ToString();
		}
	}

}