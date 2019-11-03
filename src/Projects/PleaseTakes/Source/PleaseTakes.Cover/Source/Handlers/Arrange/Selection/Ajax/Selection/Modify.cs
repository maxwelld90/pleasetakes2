using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Selection.Ajax.Selection {

	internal sealed class Modify : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _timetableWeek;
		private int _periods;
		private DateTime _selectedDate;

		public Modify(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.SetContentType("text/xml");
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			Core.Helpers.Elements.Ajax.Xml.Collection xmlCollection = new Core.Helpers.Elements.Ajax.Xml.Collection();
			bool isInternalStaff;
			int coverId;
			int staffId;

			if (this.CheckInputValidity() && this.IsInternalStaff(out isInternalStaff) && this.GetValue(out coverId) && this.GetValue(out staffId)) {
				DateTime startDate = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(this._selectedDate).RotationStartDate;
				DateTime endDate = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(this._selectedDate).RotationEndDate;

				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.Bit, "@IsInternalStaff", isInternalStaff);
				paramBuilder.AddParameter(SqlDbType.Int, "@CoverId", coverId);
				paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", staffId);
				paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@StartDate", startDate);
				paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@EndDate", endDate);
				paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
				paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)(this._selectedDate.DayOfWeek + 1));

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/Arrange/Selection/Selection/modify.sql", paramBuilder.Parameters)) {
					dataReader.Read();
					if ((bool)dataReader["Status"]) {
						dataReader.NextResult();

						if (dataReader.HasRows) {
							while (dataReader.Read()) {
								Core.Helpers.Elements.Ajax.Xml.Element xmlElement = new Core.Helpers.Elements.Ajax.Xml.Element();

								if ((bool)dataReader["IsRequest"]) {
									Records.RequestBase record = new Records.RequestBase();
									xmlElement.Id = "Request" + (int)dataReader["Id"];

									if ((bool)dataReader["RequestHasCover"]) {
										Records.CoverSelected newRecord = new Records.CoverSelected();
										xmlElement.ClassName = "Green";

										UserManagement.StaffNameFormatter staffAbsenteeNameFormatter = new UserManagement.StaffNameFormatter();
										staffAbsenteeNameFormatter.Forename = dataReader["RequestAbsentForename"] as string;
										staffAbsenteeNameFormatter.Surname = dataReader["RequestAbsentSurname"] as string;
										staffAbsenteeNameFormatter.HoldingName = dataReader["RequestAbsentHoldingname"] as string;

										newRecord.AbsenteeName = staffAbsenteeNameFormatter;

										//newRecord.AbsenteeName = Core.Utils.FormatStaffName(dataReader["RequestAbsentForename"] as string, dataReader["RequestAbsentSurname"] as string, dataReader["RequestAbsentHoldingname"] as string, false, false, false);
										newRecord.Period = (int)dataReader["RequestPeriod"];
										newRecord.CommitmentName = dataReader["RequestCommitmentName"] as string;
										newRecord.IsInternalCover = (bool)dataReader["IsInternalCover"];

										UserManagement.StaffNameFormatter staffCoveringNameFormatter = new UserManagement.StaffNameFormatter();
										staffCoveringNameFormatter.Forename = dataReader["RequestCoverForename"] as string;
										staffCoveringNameFormatter.Surname = dataReader["RequestCoverSurname"] as string;
										staffCoveringNameFormatter.HoldingName = dataReader["RequestCoverHoldingName"] as string;

										newRecord.CoverStaffName = staffCoveringNameFormatter;
										
										//newRecord.CoverStaffName = Core.Utils.FormatStaffName(dataReader["RequestCoverForename"] as string, dataReader["RequestCoverSurname"] as string, dataReader["RequestCoverHoldingName"] as string, false, false, false);

										record = newRecord;
									}
									else {
										Records.NoCoverSelected newRecord = new Records.NoCoverSelected();
										xmlElement.ClassName = "Red";

										UserManagement.StaffNameFormatter absenteeNameFormatter = new UserManagement.StaffNameFormatter();
										absenteeNameFormatter.Forename = dataReader["RequestAbsentForename"] as string;
										absenteeNameFormatter.Surname = dataReader["RequestAbsentSurname"] as string;
										absenteeNameFormatter.HoldingName = dataReader["RequestAbsentHoldingName"] as string;

										newRecord.AbsenteeName = absenteeNameFormatter;

										//newRecord.AbsenteeName = Core.Utils.FormatStaffName(dataReader["RequestAbsentForename"] as string, dataReader["RequestAbsentSurname"] as string, dataReader["RequestAbsentHoldingName"] as string, false, false, false);
										newRecord.Period = (int)dataReader["RequestPeriod"];
										newRecord.CommitmentName = dataReader["RequestCommitmentName"] as string;

										record = newRecord;
									}

									xmlElement.InnerHtml = record.InnerHtml;
								}
								else {
									Records.Selection record = new Records.Selection();
									int entitlement = (int)dataReader["SelectionEntitlement"];

									xmlElement.Id = "Selection" + (int)dataReader["Id"];
									record.IsInternal = isInternalStaff;

									if (isInternalStaff) {
										UserManagement.StaffNameFormatter nameFormatter = new UserManagement.StaffNameFormatter();
										nameFormatter.Forename = dataReader["SelectionForename"] as string;
										nameFormatter.Surname = dataReader["SelectionSurname"] as string;
										nameFormatter.HoldingName = dataReader["SelectionHoldingName"] as string;

										record.StaffName = nameFormatter;
									}
									else {
										UserManagement.StaffNameFormatter nameFormatter = new UserManagement.StaffNameFormatter();
										nameFormatter.Forename = dataReader["SelectionForename"] as string;
										nameFormatter.Surname = dataReader["SelectionSurname"] as string;

										record.StaffName = nameFormatter;
									}

									if ((bool)dataReader["SelectionIsSelected"])
										xmlElement.ClassName = "Green";
									else
										if (entitlement.Equals(0))
											xmlElement.ClassName = "Red";
										else
											xmlElement.ClassName = "";


									record.Entitlement = entitlement;
									xmlElement.InnerHtml = record.InnerHtml;
								}

								xmlCollection.Add(xmlElement);
							}
						}
					}
				}
			}

			this.Page.Contents = xmlCollection.ToString();
		}

		private bool IsInternalStaff(out bool result) {
			if (this.Path.HasNext())
				switch (this.Path.Next()) {
					case "internal":
						result = true;
						return true;
					case "outside":
						result = false;
						return true;
					default:
						result = false;
						return false;
				}

			result = false;
			return false;
		}

		private bool GetValue(out int result) {
			if (this.Path.HasNext() && int.TryParse(this.Path.Next(), out result))
				return true;

			result = 0;
			return false;
		}

		private bool CheckInputValidity() {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

			this.Path.Reset();

			for (int i = 0; i <= 5; i++)
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

					if (Modify.IsDateInSessionRange(year, month, day)) {
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
