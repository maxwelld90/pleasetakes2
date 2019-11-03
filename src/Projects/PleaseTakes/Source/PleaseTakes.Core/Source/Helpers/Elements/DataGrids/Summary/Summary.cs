using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.Core.Helpers.Elements.DataGrids.Summary {

	internal sealed partial class Summary : DataGrid {
		private DateTime _selectedDate;
		private int _timetableWeek;
		private int _periods;
		private int _searchCount;
		private string _searchTerm;
		private SelectedStaff _selectedStaff;
		private DataTable _periodsTable;
		private GenericCellsCollection _genericCellsCollection;

		public Summary(DateTime date) {
			this._selectedDate = date;
			this._timetableWeek = WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(date).TimetableWeek;
			this._periods = WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(date).TimetableWeek][(int)this._selectedDate.DayOfWeek + 1];
			this._genericCellsCollection = new GenericCellsCollection();
		}

		private void SetupGrid() {
			if (this.GetData()) {
				this.Width = WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(this._selectedDate).TimetableWeek][(int)this._selectedDate.DayOfWeek + 1] + 1;
				this.SetHeaderCells();
				this.SetGrid();
			}
			else {
				Alerts.Alert noStaffAlert = new Alerts.Alert("NoStaff");
				noStaffAlert.Colour = Alerts.Colours.Yellow;
				noStaffAlert.Message = new Constructor("/Alerts/Specific/Cover/Arrange/Attendance/Periods/nostaff.html").ToString();
				noStaffAlert.NoScript = false;
				noStaffAlert.ShowCloseBox = false;
				noStaffAlert.StartHidden = false;

				this.TextMessage.Contents = noStaffAlert.ToString();
			}
		}

		private bool GetData() {
			Database.ParameterBuilder paramBuilder = new Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@Date", this._selectedDate);
			paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
			paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)this._selectedDate.DayOfWeek + 1);

			if (string.IsNullOrEmpty(this._searchTerm))
				paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", "");
			else
				paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", this._searchTerm);

			using (SqlDataReader dataReader = Database.Provider.ExecuteReader("/Sql/Elements/Datagrids/Summary/results.sql", paramBuilder.Parameters)) {
				if (dataReader.HasRows) {
					this._selectedStaff = new SelectedStaff();

					while (dataReader.Read()) {
						StaffMember staffMember = new StaffMember();
						staffMember.Id = (int)dataReader["StaffId"];
						staffMember.Forename = dataReader["Forename"] as string;
						staffMember.Surname = dataReader["Surname"] as string;
						staffMember.HoldingName = dataReader["HoldingName"] as string;

						this._selectedStaff.Add(staffMember);
						this._searchCount++;
					}

					dataReader.NextResult();
					this._periodsTable = Database.Provider.GetDataTable(dataReader);

					return true;
				}
				else
					return false;
			}
		}

		private void SetHeaderCells() {
			this.Rows.Add(this);

			for (int i = 0; i <= this._periods; i++) {
				Cell headerCell = new Cell();
				headerCell.Type = CellTypes.Header;

				if (i == 0)
					this.Rows[0].Add(headerCell);
				else {
					headerCell.Value = i.ToString();
					this.Rows[0].Add(headerCell);
				}
			}
		}

		private void SetGrid() {
			foreach (StaffMember staffMember in this._selectedStaff) {
				Row staffRow = this.Rows.AddAndReturn(this);

				Cell nameCell = new Cell();
				nameCell.Id = this.SetCellValue(this._genericCellsCollection.StaffName.Id, staffMember.Id, null);
				nameCell.Type = CellTypes.HeaderSmall;
				nameCell.Href = this.SetCellValue(this._genericCellsCollection.StaffName.Href, staffMember.Id, null);
				nameCell.OnClick = this.SetCellValue(this._genericCellsCollection.StaffName.OnClick, staffMember.Id, null);
				//nameCell.Value = Utils.FormatStaffName(staffMember.Forename, staffMember.Surname, staffMember.HoldingName, true, true, true);

				UserManagement.StaffNameFormatter nameFormatter = new UserManagement.StaffNameFormatter();
				nameFormatter.Forename = staffMember.Forename;
				nameFormatter.Surname = staffMember.Surname;
				nameFormatter.HoldingName = staffMember.HoldingName;
				nameFormatter.AbbreviateForename = true;
				nameFormatter.DisplayForenameFirst = true;
				nameFormatter.DisplaySmallHoldingName = true;
				nameFormatter.IsToolTip = false;

				nameCell.Value = nameFormatter.ToString();

				nameCell.Parent = this;
				staffRow.Add(nameCell);

				for (int period = 1; period <= this._periods; period++) {
					try {
						DataRow record = this._periodsTable.Select("StaffId = " + staffMember.Id + " AND Period = " + period)[0];

						switch (record["Status"].ToString().ToLower()) {
							case "unavailable":
								staffRow.Add(this.CreateCell(SummaryCellTypes.Unavailable, staffMember.Id, period));
								break;
							case "absent":
								staffRow.Add(this.CreateCell(SummaryCellTypes.Absent, staffMember.Id, period));
								break;
							case "presentisbusy":
								staffRow.Add(this.CreateCell(SummaryCellTypes.PresentIsBusy, staffMember.Id, period));
								break;
							case "presentandfreeyeargroupaway":
								staffRow.Add(this.CreateCell(SummaryCellTypes.PresentAndFreeYeargroupAway, staffMember.Id, period));
								break;
						}
					}
					catch (IndexOutOfRangeException) {
						staffRow.Add(this.CreateCell(SummaryCellTypes.PresentAndFree, staffMember.Id, period));
					}

				}
			}
		}

		private Cell CreateCell(SummaryCellTypes type, int staffId, int period) {
			Cell newCell = new Cell();
			newCell.Type = CellTypes.StandardLarge;
			newCell.Parent = this;

			switch (type) {
				case SummaryCellTypes.Unavailable:
					newCell.Id = this.SetCellValue(this._genericCellsCollection.Unavailable.Id, staffId, period);
					newCell.Colour = this._genericCellsCollection.Unavailable.Colour;
					newCell.Value = this._genericCellsCollection.Unavailable.Value;
					newCell.Href = this.SetCellValue(this._genericCellsCollection.Unavailable.Href, staffId, period);
					newCell.OnClick = this.SetCellValue(this._genericCellsCollection.Unavailable.OnClick, staffId, period);
					break;
				case SummaryCellTypes.Absent:
					newCell.Id = this.SetCellValue(this._genericCellsCollection.Absent.Id, staffId, period);
					newCell.Colour = this._genericCellsCollection.Absent.Colour;
					newCell.Value = this._genericCellsCollection.Absent.Value;
					newCell.Href = this.SetCellValue(this._genericCellsCollection.Absent.Href, staffId, period);
					newCell.OnClick = this.SetCellValue(this._genericCellsCollection.Absent.OnClick, staffId, period);
					break;
				case SummaryCellTypes.PresentAndFree:
					newCell.Id = this.SetCellValue(this._genericCellsCollection.PresentAndFree.Id, staffId, period);
					newCell.Colour = this._genericCellsCollection.PresentAndFree.Colour;
					newCell.Value = this._genericCellsCollection.PresentAndFree.Value;
					newCell.Href = this.SetCellValue(this._genericCellsCollection.PresentAndFree.Href, staffId, period);
					newCell.OnClick = this.SetCellValue(this._genericCellsCollection.PresentAndFree.OnClick, staffId, period);
					break;
				case SummaryCellTypes.PresentIsBusy:
					newCell.Id = this.SetCellValue(this._genericCellsCollection.PresentIsBusy.Id, staffId, period);
					newCell.Colour = this._genericCellsCollection.PresentIsBusy.Colour;
					newCell.Value = this._genericCellsCollection.PresentIsBusy.Value;
					newCell.Href = this.SetCellValue(this._genericCellsCollection.PresentIsBusy.Href, staffId, period);
					newCell.OnClick = this.SetCellValue(this._genericCellsCollection.PresentIsBusy.OnClick, staffId, period);
					break;
				case SummaryCellTypes.PresentAndFreeYeargroupAway:
					newCell.Id = this.SetCellValue(this._genericCellsCollection.PresentAndFreeYeargroupAway.Id, staffId, period);
					newCell.Colour = this._genericCellsCollection.PresentAndFreeYeargroupAway.Colour;
					newCell.Value = this._genericCellsCollection.PresentAndFreeYeargroupAway.Value;
					newCell.Href = this.SetCellValue(this._genericCellsCollection.PresentAndFreeYeargroupAway.Href, staffId, period);
					newCell.OnClick = this.SetCellValue(this._genericCellsCollection.PresentAndFreeYeargroupAway.OnClick, staffId, period);
					break;
			}

			return newCell;
		}

		private string SetCellValue(string value, int staffId, int? period) {
			if (!string.IsNullOrEmpty(value)) {
				value = value.Replace("Year", this._selectedDate.Year.ToString());
				value = value.Replace("Month", this._selectedDate.Month.ToString());
				value = value.Replace("Day", this._selectedDate.Day.ToString());
				value = value.Replace("StaffId", staffId.ToString());

				if (period != null)
					value = value.Replace("Period", period.ToString());
			}

			return value;
		}

		public string SearchTerm {
			get {
				return this._searchTerm;
			}
			set {
				this._searchTerm = value;
			}
		}

		public int SearchCount {
			get {
				return this._searchCount;
			}
		}

		public GenericCellsCollection GenericCells {
			get {
				return this._genericCellsCollection;
			}
		}

		public override string ToString() {
			this.SetupGrid();
			return base.ToString();
		}
	}

}