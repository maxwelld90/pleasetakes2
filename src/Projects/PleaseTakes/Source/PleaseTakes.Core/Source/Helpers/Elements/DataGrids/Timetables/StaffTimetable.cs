using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Timetables {

	internal sealed partial class StaffTimetable : Timetable {
		private StaffTimetableEvents _events;
		private int _staffId;
		private int _databaseResponse;
		private DataTable _periodsTable;

		public StaffTimetable(int staffId) {
			this._events = new StaffTimetableEvents();
			this._staffId = staffId;
		}

		public StaffTimetableEvents Events {
			get {
				return this._events;
			}
		}

		private void SetTimetable() {
			Database.ParameterBuilder paramBuilder = new Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
			paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this.WeekNo);
			paramBuilder.AddParameter(SqlDbType.Bit, "@CurrentSession", true);

			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Elements/Datagrids/Timetables/stafftimetable.sql", paramBuilder.Parameters)) {
				dataReader.Read();
				this._databaseResponse = (int)dataReader["Status"];

				dataReader.NextResult();
				this._periodsTable = Database.Provider.GetDataTable(dataReader);

				this.SetCells();
			}
		}

		private void SetCells() {
			int rowCounter = 1;

			for (int i = 1; i <= 7; i++) {
				int periodCount;

				try {
					periodCount = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this.WeekNo][i];

					for (int j = 1; j <= periodCount; j++) {
						try {
							DataRow periodRecord = this._periodsTable.Select("DayNo = " + i + " AND Period = " + j)[0];

							if ((bool)periodRecord["IsUnavailable"]) {
								this.Rows[rowCounter][j].Colour = this._events.Unavailable.Colour;
								this.Rows[rowCounter][j].Type = this._events.Unavailable.Type;
								this.Rows[rowCounter][j].Value = this._events.Unavailable.Value;
								this.Rows[rowCounter][j].Href = this.SetCellLinkValues(true, this._events.Unavailable, i, j);
								this.Rows[rowCounter][j].OnClick = this.SetCellLinkValues(false, this._events.Unavailable, i, j);
							}
							else {
								switch ((int)(periodRecord["PeriodType"])) {
									case 2:
										this.Rows[rowCounter][j].Colour = this._events.BusyOther.Colour;
										this.Rows[rowCounter][j].Type = this._events.BusyOther.Type;
										this.Rows[rowCounter][j].Value = periodRecord["CommitmentName"] as string;
										this.Rows[rowCounter][j].Href = this.SetCellLinkValues(true, this._events.BusyOther, i, j);
										this.Rows[rowCounter][j].OnClick = this.SetCellLinkValues(false, this._events.BusyOther, i, j);
										break;
									default:
										this.Rows[rowCounter][j].Colour = this._events.BusyTeaching.Colour;

										if (string.IsNullOrEmpty(periodRecord["RoomName"] as string))
											this.Rows[rowCounter][j].Type = CellTypes.Standard;
										else
											this.Rows[rowCounter][j].Type = CellTypes.StandardWithSpan;

										this.Rows[rowCounter][j].Value = periodRecord["CommitmentName"] as string;
										this.Rows[rowCounter][j].SpanValue = periodRecord["RoomName"] as string;
										this.Rows[rowCounter][j].Href = this.SetCellLinkValues(true, this._events.BusyTeaching, i, j);
										this.Rows[rowCounter][j].OnClick = this.SetCellLinkValues(false, this._events.BusyTeaching, i, j);

										break;
								}
							}
						}
						catch (IndexOutOfRangeException) {
							this.Rows[rowCounter][j].Colour = this._events.Free.Colour;
							this.Rows[rowCounter][j].Type = this._events.Free.Type;
							this.Rows[rowCounter][j].Value = this._events.Free.Value;
							this.Rows[rowCounter][j].Href = this.SetCellLinkValues(true, this._events.Free, i, j);
							this.Rows[rowCounter][j].OnClick = this.SetCellLinkValues(false, this._events.Free, i, j);
						}
					}

					rowCounter++;
				}
				catch (Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) { }
			}
		}

		private string SetCellLinkValues(bool isHref, Cell eventCell, int dayNo, int period) {
			string eventContents;

			if (isHref)
				eventContents = eventCell.Href;
			else
				eventContents = eventCell.OnClick;

			if (!string.IsNullOrEmpty(eventContents)) {
				eventContents = eventContents.Replace("StaffId", this._staffId.ToString());
				eventContents = eventContents.Replace("WeekNo", this.WeekNo.ToString());
				eventContents = eventContents.Replace("DayNo", dayNo.ToString());
				eventContents = eventContents.Replace("Period", period.ToString());
			}

			return eventContents;
		}

		public override string ToString() {
			this.SetTimetable();

			switch (this._databaseResponse) {
				case 0:
					throw new InvalidStaffIdException();
				case 2:
					throw new NoTimetableException();
			}

			return base.ToString();
		}
	}

	public class InvalidStaffIdException : Exception {
		public InvalidStaffIdException() : base() { }
	}

	public class NoTimetableException : Exception {
		public NoTimetableException() : base() { }
	}

}