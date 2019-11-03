using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Slips.Printouts {

	internal sealed class Day : Core.BasicResponses.Printable {
		private int _timetableWeek;
		private int _periods;
		private DateTime _selectedDate;

		public Day(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void SetPrintContent() {
			if (this.CheckInputValidity()) {
				DateTime creationDate = DateTime.Now;

				Core.Helpers.Constructor pageContent = new Core.Helpers.Constructor("/Templates/Specific/Cover/Slips/Printouts/valid.html");
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				pageContent.SetVariable("CoverDate", this._selectedDate.ToString("dddd, dd MMMM yyyy"));

				paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@Date", this._selectedDate);
				paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
				paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)(this._selectedDate.DayOfWeek + 1));

				int internalCount = 0;
				int outsideCount = 0;

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/Slips/Printouts/daylist.sql", paramBuilder.Parameters)) {
					if (dataReader.HasRows) {
						Core.Helpers.Constructor internalConstructor = new Core.Helpers.Constructor("/Templates/Specific/Cover/Slips/Printouts/internalcover.html");
						Core.Helpers.Elements.CoverSlips.Collection internalCollection = new Core.Helpers.Elements.CoverSlips.Collection();
						int pageCount = 0;
						int pageSlipCount = 0;

						while (dataReader.Read()) {
							Core.Helpers.Elements.CoverSlips.Slip slip = new Core.Helpers.Elements.CoverSlips.Slip();

							this.PopulateSlip(ref slip, dataReader, true, creationDate);

							if (pageCount.Equals(0) && pageSlipCount.Equals(3)) {
								slip.PageBreakBefore = true;
								pageCount++;
								pageSlipCount = 0;
							}
							else if (pageSlipCount.Equals(3)) {
								slip.PageBreakBefore = true;
								pageCount++;
								pageSlipCount = 0;
							}
							else
								pageSlipCount++;

							internalCollection.Add(slip);
							internalCount++;
						}

						internalConstructor.SetVariable("Slips", internalCollection.ToString());
						pageContent.SetVariable("InternalCoverCount", internalCount.ToString());
						pageContent.SetVariable("InternalCoverSlips", internalConstructor);
					}
					else {
						pageContent.SetVariable("InternalCoverCount", "0");
						pageContent.DeleteVariable("InternalCoverSlips");
					}

					dataReader.NextResult();

					if (dataReader.HasRows) {
						Core.Helpers.Constructor outsideConstructor = new Core.Helpers.Constructor("/Templates/Specific/Cover/Slips/Printouts/outsidecover.html");
						Core.Helpers.Elements.CoverSlips.Collection outsideCollection = new Core.Helpers.Elements.CoverSlips.Collection();

						if (internalCount > 0)
							outsideConstructor.SetVariable("BreakBefore", " class=\"BreakBefore\"");
						else
							outsideConstructor.DeleteVariable("BreakBefore");

						int pageCount = 0;
						int pageSlipCount = 0;

						while (dataReader.Read()) {
							Core.Helpers.Elements.CoverSlips.Slip slip = new Core.Helpers.Elements.CoverSlips.Slip();

							this.PopulateSlip(ref slip, dataReader, false, creationDate);

							if (!internalCount.Equals(0) && pageCount.Equals(0) && pageSlipCount.Equals(4)) {
								slip.PageBreakBefore = true;
								pageCount++;
								pageSlipCount = 0;
							}
							else if (internalCount.Equals(0) && pageCount.Equals(0) && pageSlipCount.Equals(3)) {
								slip.PageBreakBefore = true;
								pageCount++;
								pageSlipCount = 0;
							}
							else if (!pageCount.Equals(0) && pageSlipCount.Equals(3)) {
								slip.PageBreakBefore = true;
								pageCount++;
								pageSlipCount = 0;
							}
							else
								pageSlipCount++;

							outsideCollection.Add(slip);
							outsideCount++;
						}

						outsideConstructor.SetVariable("Slips", outsideCollection.ToString());
						pageContent.SetVariable("OutsideCoverCount", outsideCount.ToString());
						pageContent.SetVariable("OutsideCoverSlips", outsideConstructor);
					}
					else {
						pageContent.SetVariable("OutsideCoverCount", "0");
						pageContent.DeleteVariable("OutsideCoverSlips");
					}
				}

				this.Page.SetVariable("PrintArea", pageContent);
			}
			else
				this.Page.SetVariable("PrintArea", new Core.Helpers.Constructor("/Templates/Specific/Cover/Slips/Printouts/badrequest.html"));
		}

		private void PopulateSlip(ref Core.Helpers.Elements.CoverSlips.Slip slip, SqlDataReader dataReader, bool isInternal, DateTime creationDate) {
			slip.CoverDate = this._selectedDate;
			slip.Period = (int)dataReader["Period"];
			slip.Room = dataReader["CoverRoom"] as string;
			slip.CoverStaffDetails.Forename = dataReader["CoveringForename"] as string;
			slip.CoverStaffDetails.Surname = dataReader["CoveringSurname"] as string;

			if (isInternal) {
				slip.CoverStaffDetails.HoldingName = dataReader["CoveringHoldingName"] as string;
				slip.CoverStaffDetails.MainRoom = dataReader["CoveringMainRoom"] as string;
				slip.CoverStaffDetails.Department = dataReader["CoveringDepartment"] as string;
			}

			slip.AbsentStaffDetails.Forename = dataReader["AbsentForename"] as string;
			slip.AbsentStaffDetails.Surname = dataReader["AbsentSurname"] as string;
			slip.AbsentStaffDetails.HoldingName = dataReader["AbsentHoldingName"] as string;

			slip.ClassDetails.Name = dataReader["CommitmentName"] as string;
			slip.ClassDetails.Subject = dataReader["SubjectName"] as string;

			slip.IsInternalCover = isInternal;
			slip.CoverId = (int)dataReader["CoverId"];
		}

		private bool CheckInputValidity() {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

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

					if (Day.IsDateInSessionRange(year, month, day)) {
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