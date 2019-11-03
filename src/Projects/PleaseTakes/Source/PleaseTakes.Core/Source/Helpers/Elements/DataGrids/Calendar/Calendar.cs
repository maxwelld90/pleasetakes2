using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Calendar {

	internal sealed partial class Calendar : DataGrid {
		private CalendarEvents _events;
		private int _year;
		private int _month;

		public Calendar() {
			this.Width = 7;
			this._events = new CalendarEvents();
		}

		public CalendarEvents Events {
			get {
				return this._events;
			}
		}

		public int Year {
			get {
				return this._year;
			}
			set {
				this._year = value;
			}
		}

		public int Month {
			get {
				return this._month;
			}
			set {
				this._month = value;
			}
		}

		private void SetControls(Constructor calendar) {
			DateTime centreDate = new DateTime(this._year, this._month, 1);
			DateTime leftDate = centreDate.AddMonths(-1);
			DateTime rightDate = centreDate.AddMonths(1);

			if (DateTime.Compare(centreDate, WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.StartDateMonth).Equals(-1)) {
				this.TopControls.Left.Href = null;
				this.TopControls.Left.OnClick = null;
				rightDate = WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.StartDateMonth;
			}
			else {
				this.SetHref(leftDate, this.TopControls.Left);
				this.SetOnClick(leftDate, this.TopControls.Left);
			}

			if (DateTime.Compare(WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.EndDateMonth, centreDate).Equals(-1)) {
				this.TopControls.Right.Href = null;
				this.TopControls.Right.OnClick = null;
			}
			else {
				this.SetHref(rightDate, this.TopControls.Right);
				this.SetOnClick(rightDate, this.TopControls.Right);
			}

			this.SetValues(centreDate, this.TopControls.Centre);
			this.SetValues(leftDate, this.TopControls.Left);
			this.SetValues(rightDate, this.TopControls.Right);
		}

		private void SetValues(DateTime date, TopControl control) {
			if (!string.IsNullOrEmpty(control.Value)) {
				control.Value = control.Value.Replace("MonthName", Utils.GetMonthName(date.Month));
				control.Value = control.Value.Replace("Year", date.Year.ToString());
			}
		}

		private void SetHref(DateTime date, TopControl control) {
			if (!string.IsNullOrEmpty(control.Href)) {
				control.Href = control.Href.Replace("Month", date.Month.ToString());
				control.Href = control.Href.Replace("Year", date.Year.ToString());
			}
		}

		private void SetOnClick(DateTime date, TopControl control) {
			if (!string.IsNullOrEmpty(control.OnClick)) {
				control.OnClick = control.OnClick.Replace("Month", date.Month.ToString());
				control.OnClick = control.OnClick.Replace("Year", date.Year.ToString());
			}
		}

		private void SetCells() {
			this.SetHeaderCells();
			this.Rows.Add(this);

			int startDayOfWeek = (int)new DateTime(this._year, this._month, 1).DayOfWeek;
			int daysInMonth = DateTime.DaysInMonth(this._year, this._month);
			int dayCounter = 1;
			int rowCounter = 1;
			int rowPosition = 0;

			if (!startDayOfWeek.Equals(0))
				for (int i = 0; (i <= (startDayOfWeek - 1)); i++) {
					this.Rows[1].Add(Cell.BlankCell());
					rowPosition++;
				}

			while (dayCounter <= daysInMonth) {
				DateTime loopDate = new DateTime(this._year, this._month, dayCounter);
				int comparisonWithNow = Utils.CompareDateWithNow(loopDate);

				if (rowPosition.Equals(7)) {
					this.Rows.Add(this);
					rowPosition = 0;
					rowCounter++;
				}

				Cell cell = new Cell();
				cell.Parent = this;

				if (comparisonWithNow.Equals(-1)) {
					this.SetCellToEventCell(ref cell, this._events.Past);
				}
				else {
					try {
						int timetableWeek = WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(loopDate).TimetableWeek;

						try {
							int periods = WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[timetableWeek][rowPosition + 1];

							if (periods.Equals(0))
								this.SetCellToEventCell(ref cell, this._events.NoDay);
							else {
								if (comparisonWithNow.Equals(0))
									this.SetCellToEventCell(ref cell, this._events.TodayInRange);
								else
									this.SetCellToEventCell(ref cell, this._events.InRange);
							}
						}
						catch (Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) {
							this.SetCellToEventCell(ref cell, this._events.NoDay);
						}
					}
					catch (ArgumentOutOfRangeException) {
						this.SetCellToEventCell(ref cell, this._events.OutOfRange);
					}
				}

				this.SetCellValues(ref cell, dayCounter);
				this.Rows[rowCounter].Add(cell);

				rowPosition++;
				dayCounter++;
			}
		}

		private void SetCellToEventCell(ref Cell cell, Cell eventCell) {
			cell.Colour = eventCell.Colour;
			cell.Href = eventCell.Href;
			cell.Id = eventCell.Id;
			cell.OnClick = eventCell.OnClick;
			cell.SpanValue = eventCell.SpanValue;
			cell.Type = eventCell.Type;
			cell.Value = eventCell.Value;
		}

		private void SetCellValues(ref Cell cell, int dayCounter) {
			cell.Href = this.SetCellValue(cell.Href, dayCounter);
			cell.Id = this.SetCellValue(cell.Id, dayCounter);
			cell.OnClick = this.SetCellValue(cell.OnClick, dayCounter);
			cell.SpanValue = this.SetCellValue(cell.SpanValue, dayCounter);
			cell.Value = this.SetCellValue(cell.Value, dayCounter);
		}

		private string SetCellValue(string property, int dayCounter) {
			if (!string.IsNullOrEmpty(property)) {
				property = property.Replace("Year", this._year.ToString());
				property = property.Replace("Month", this._month.ToString());
				property = property.Replace("Day", dayCounter.ToString());
				return property;
			}

			return null;
		}

		private void SetHeaderCells() {
			this.Rows.Add(this);

			for (int i = 0; i <= 6; i++) {
				Cell headerCell = new Cell();
				headerCell.Type = CellTypes.Header;
				headerCell.Value = Utils.GetWeekDayAbbreviation((DayOfWeek)i);

				this.Rows[0].Add(headerCell);
			}
		}

		public override string ToString() {
			this.SetControls(this.Constructor);
			this.SetCells();

			return base.ToString();
		}
	}

}