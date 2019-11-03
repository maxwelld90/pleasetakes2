using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Slips.Ajax.Landing {

	internal sealed class Requests : Core.Helpers.BaseHandlers.AjaxHandler {
		private DateTime _selectedDate;
		private int _timetableWeek;
		private int _periods;

		public Requests(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			Core.Helpers.Path.Parser sourcePath = new Core.Helpers.Path.Parser(Core.WebServer.Request["sourcepath"]);
			bool validSourceDate = this.CheckInputValidity(false, sourcePath);
			bool validAjaxDate = this.CheckInputValidity(true, this.Path);

			if (!validSourceDate && !validAjaxDate) {
				Core.Helpers.Elements.Alerts.Alert noDate = new Core.Helpers.Elements.Alerts.Alert("NoDate");
				noDate.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
				noDate.NoScript = false;
				noDate.ShowCloseBox = false;
				noDate.StartHidden = false;
				noDate.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Slips/Requests/nodate.html").ToString();

				this.Page.Contents = noDate.ToString();
			}
			else {
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				string searchTerm = "";

				if (this.Path.HasNext())
					searchTerm = this.Path.Next();

				paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@Date", this._selectedDate);
				paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
				paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)(this._selectedDate.DayOfWeek + 1));
				paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", searchTerm);

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/slips/selective.sql", paramBuilder.Parameters)) {
					if (dataReader.HasRows) {
						Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();

						while (dataReader.Read()) {
							Records.CoverSelected record = new Records.CoverSelected();
							record.Record.Colour = Core.Helpers.Elements.RecordLists.Colours.Blue;

							record.IsInternalCover = (bool)dataReader["IsInternal"];
							record.Period = (int)dataReader["Period"];
							record.CommitmentName = dataReader["CommitmentName"] as string;

							UserManagement.StaffNameFormatter absenteeNameFormatter = new UserManagement.StaffNameFormatter();
							absenteeNameFormatter.Forename = dataReader["AbsentForename"] as string;
							absenteeNameFormatter.Surname = dataReader["AbsentSurname"] as string;
							absenteeNameFormatter.HoldingName = dataReader["AbsentHoldingName"] as string;

							record.AbsenteeName = absenteeNameFormatter;
							record.AbsenteeNameToolTip = absenteeNameFormatter;

							UserManagement.StaffNameFormatter coveringNameFormatter = new UserManagement.StaffNameFormatter();
							coveringNameFormatter.Forename = dataReader["CoverForename"] as string;
							coveringNameFormatter.Surname = dataReader["CoverSurname"] as string;
							coveringNameFormatter.HoldingName = dataReader["CoverHoldingName"] as string;

							record.CoverStaffName = coveringNameFormatter;

							recordCollection.Add(record.Record);
						}

						this.Page.Contents = recordCollection.ToString();
					}
					else {
						Core.Helpers.Elements.Alerts.Alert noRequests = new Core.Helpers.Elements.Alerts.Alert("NoRequests");
						noRequests.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
						noRequests.NoScript = false;
						noRequests.ShowCloseBox = false;
						noRequests.StartHidden = false;
						noRequests.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Slips/Requests/norequests.html").ToString();

						this.Page.Contents = noRequests.ToString();
					}
				}

				

			}
		}

		private bool CheckInputValidity(bool isAjax, Core.Helpers.Path.Parser path) {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

			if (!isAjax) {
				for (int i = 0; i <= 1; i++) {
					if (!path.HasNext())
						return false;

					path.Next();
				}
			}

			for (int i = 0; i <= 2; i++) {
				if (path.HasNext())
					switch (i) {
						case 0:
							validYear = int.TryParse(path.Peek(), out year);

							if (!validYear)
								return false;
							else
								path.Next();

							break;
						case 1:
							validMonth = int.TryParse(path.Next(), out month);
							break;
						case 2:
							validDay = int.TryParse(path.Next(), out day);
							break;
					}
				else
					return false;
			}

			if ((!validYear) || (!validMonth) || (!validDay))
				return false;
			else {
				try {
					year.RequireThat("year").IsInRange(2000, 3000);
					month.RequireThat("month").IsInRange(1, 12);
					day.RequireThat("day").IsInRange(1, DateTime.DaysInMonth(year, month));

					if (Requests.IsDateInSessionRange(year, month, day)) {
						DateTime selectedDate = new DateTime(year, month, day);
						this._timetableWeek = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(selectedDate).TimetableWeek;

						try {
							this._periods = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this._timetableWeek][(int)selectedDate.DayOfWeek + 1];

							if (this._periods.Equals(0))
								return false;
							else {
								this._selectedDate = selectedDate;
								return true;
							}
						}
						catch (Core.Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) {
							return false;
						}
					}
					else
						return false;
				}
				catch (IndexOutOfRangeException) {
					return false;
				}
			}
		}

		private static bool IsDateInSessionRange(int year, int month, int day) {
			DateTime date = new DateTime(year, month, day);
			DateTime low = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.StartDate;
			DateTime high = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.EndDate;

			if (Core.Utils.IsDateInRange(low, high, date))
				return true;

			return false;
		}

	}

}
