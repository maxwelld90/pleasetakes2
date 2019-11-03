using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Selection.Ajax.Requests {

	internal sealed class Standard : Core.Helpers.BaseHandlers.AjaxHandler {
		private DateTime _selectedDate;
		private int _timetableWeek;
		private int _periods;

		public Standard(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			
			if (this.CheckInputValidity()) {
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@Date", this._selectedDate);
				paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
				paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)(this._selectedDate.DayOfWeek + 1));

				if (this.Path.HasNext() && this.Path.Peek().Equals("notrequired")) {
					this.Path.Next();
					paramBuilder.AddParameter(SqlDbType.Bit, "@GetNoCoverRequests", 1);
				}
				else {
					paramBuilder.AddParameter(SqlDbType.Bit, "@GetNoCoverRequests", 0);
				}

				if (this.Path.HasNext())
					paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", this.Path.Next());
				else
					paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", "");

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/Arrange/Selection/Requests/list.sql", paramBuilder.Parameters)) {
					if (dataReader.HasRows) {
						Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();
						int requestsCount = 0;

						while (dataReader.Read()) {
							Records.RequestBase record = new Records.RequestBase();

							if ((bool)dataReader["IsBusy"]) {
								if ((!dataReader["IsYeargroupAway"].Equals(System.DBNull.Value)) && ((bool)dataReader["IsYeargroupAway"])) {
									Records.NoCoverRequired newRecord = new Records.NoCoverRequired();
									newRecord.CommitmentName = dataReader["CommitmentName"] as string;
									newRecord.IsYeargroupAway = true;
									record = newRecord;
								}
								else if (!(bool)dataReader["TeachingClass"]) {
									Records.NoCoverRequired newRecord = new Records.NoCoverRequired();
									newRecord.CommitmentName = dataReader["CommitmentName"] as string;
									newRecord.IsNotTeaching = true;
									record = newRecord;
								}
								else {
									if ((bool)dataReader["HasCover"]) {
										if ((bool)dataReader["IsInternalCover"]) {
											Records.CoverSelected newRecord = new Records.CoverSelected();
											newRecord.IsInternalCover = true;
											newRecord.Id = "Request" + (int)dataReader["CoverId"];
											newRecord.OnClick = "switchToSelection(" + (int)dataReader["CoverId"] + ", " + this._selectedDate.Year + ", " + this._selectedDate.Month + ", " + this._selectedDate.Day + ", true)";
											newRecord.CommitmentName = dataReader["CommitmentName"] as string;

											UserManagement.StaffNameFormatter coveringStaffNameFormatter = new UserManagement.StaffNameFormatter();
											coveringStaffNameFormatter.Forename = dataReader["CoverForename"] as string;
											coveringStaffNameFormatter.Surname = dataReader["CoverSurname"] as string;
											coveringStaffNameFormatter.HoldingName = dataReader["CoverHoldingName"] as string;

											newRecord.CoverStaffName = coveringStaffNameFormatter;

											//newRecord.CoverStaffName = Core.Utils.FormatStaffName(dataReader["CoverForename"] as string, dataReader["CoverSurname"] as string, dataReader["CoverHoldingName"] as string, false, false, false);
											record = newRecord;
										}
										else {
											Records.CoverSelected newRecord = new Records.CoverSelected();
											newRecord.IsInternalCover = false;
											newRecord.Id = "Request" + (int)dataReader["CoverId"];
											newRecord.OnClick = "switchToSelection(" + (int)dataReader["CoverId"] + ", " + this._selectedDate.Year + ", " + this._selectedDate.Month + ", " + this._selectedDate.Day + ", true)";
											newRecord.CommitmentName = dataReader["CommitmentName"] as string;

											UserManagement.StaffNameFormatter coveringStaffNameFormatter = new UserManagement.StaffNameFormatter();
											coveringStaffNameFormatter.Forename = dataReader["CoverForename"] as string;
											coveringStaffNameFormatter.Surname = dataReader["CoverSurname"] as string;

											newRecord.CoverStaffName = coveringStaffNameFormatter;

											//newRecord.CoverStaffName = Core.Utils.FormatStaffName(dataReader["CoverForename"] as string, dataReader["CoverSurname"] as string, null, false, false, false);
											record = newRecord;
										}
									}
									else {
										Records.NoCoverSelected newRecord = new Records.NoCoverSelected();
										newRecord.Id = "Request" + (int)dataReader["CoverId"];
										newRecord.OnClick = "switchToSelection(" + (int)dataReader["CoverId"] + ", " + this._selectedDate.Year + ", " + this._selectedDate.Month + ", " + this._selectedDate.Day + ", true)";
										newRecord.CommitmentName = dataReader["CommitmentName"] as string;
										record = newRecord;
									}
								}
							}
							else {
								Records.NoCoverRequired newRecord = new Records.NoCoverRequired();
								newRecord.IsYeargroupAway = false;
								record = newRecord;
							}

							UserManagement.StaffNameFormatter absenteeNameFormatter = new UserManagement.StaffNameFormatter();
							absenteeNameFormatter.Forename = dataReader["AbsentForename"] as string;
							absenteeNameFormatter.Surname = dataReader["AbsentSurname"] as string;
							absenteeNameFormatter.HoldingName = dataReader["AbsentHoldingName"] as string;

							record.AbsenteeName = absenteeNameFormatter;
							record.AbsenteeNameToolTip = absenteeNameFormatter;

							//record.AbsenteeName = Core.Utils.FormatStaffName(dataReader["AbsentForename"] as string, dataReader["AbsentSurname"] as string, dataReader["AbsentHoldingName"] as string, false, false, false);
							//record.AbsenteeNameToolTip = Core.Utils.FormatStaffName(dataReader["AbsentForename"] as string, dataReader["AbsentSurname"] as string, dataReader["AbsentHoldingName"] as string, false, false, false);
							record.Period = (int)dataReader["Period"];
							recordCollection.Add(record.Record);
							requestsCount++;
						}

						if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("ArrangeRequestsCount"))
							Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangeRequestsCount"] = requestsCount;
						else
							Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("ArrangeRequestsCount", requestsCount);

						this.Page.Contents = recordCollection.ToString();
					}
					else {
						Core.Helpers.Elements.Alerts.Alert a = new Core.Helpers.Elements.Alerts.Alert("NoRequests");
						a.Colour = PleaseTakes.Core.Helpers.Elements.Alerts.Colours.Yellow;
						a.NoScript = false;
						a.ShowCloseBox = false;
						a.StartHidden = false;
						a.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Selection/Requests/norequests.html").ToString();

						this.Page.Contents = a.ToString();
					}
				}
			}
			else {
				Core.Helpers.Elements.Alerts.Alert alert = new Core.Helpers.Elements.Alerts.Alert("Error");
				alert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
				alert.NoScript = false;
				alert.ShowCloseBox = false;
				alert.StartHidden = false;
				alert.Message = new Core.Helpers.Constructor("/Alerts/Ajax/error.html").ToString();

				this.Page.Contents = alert.ToString();
			}
		}

		private bool CheckInputValidity() {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

			this.Path.Reset();

			for (int i = 0; i <= 4; i++)
				this.Path.Next();

			for (int i = 0; i <= 2; i++) {
				if (this.Path.HasNext())
					switch (i) {
						case 0:
							validYear = int.TryParse(this.Path.Next(), out year);
							break;
						case 1:
							validMonth = int.TryParse(this.Path.Next(), out month);
							break;
						case 2:
							validDay = int.TryParse(this.Path.Next(), out day);
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

					if (Standard.IsDateInSessionRange(year, month, day)) {
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