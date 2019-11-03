using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Attendance.Ajax.Periods {

	internal sealed class Modify : Core.Helpers.BaseHandlers.AjaxHandler {
		private DateTime _selectedDate;
		private int _timetableWeek;
		private int _periods;
		private int _period;
		private int _staffId;

		public Modify(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.SetContentType("text/xml");
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			Core.Helpers.Elements.Ajax.Xml.Collection xmlCollection = new Core.Helpers.Elements.Ajax.Xml.Collection();

			if (this.CheckInputValidity()) {
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
				paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@Date", this._selectedDate);
				paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
				paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)(this._selectedDate.DayOfWeek + 1));

				if (this._period.Equals(0)) {
					paramBuilder.AddParameter(SqlDbType.Int, "@PeriodMin", 1);
					paramBuilder.AddParameter(SqlDbType.Int, "@PeriodMax", this._periods);
				}
				else {
					paramBuilder.AddParameter(SqlDbType.Int, "@PeriodMin", this._period);
					paramBuilder.AddParameter(SqlDbType.Int, "@PeriodMax", this._period);
				}

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/Arrange/Attendance/Periods/modify.sql", paramBuilder.Parameters)) {
					if (dataReader.HasRows) {
						while (dataReader.Read()) {
							int period = (int)dataReader["Period"];
							string id = "Periods" + this._staffId + "-" + period;
							string classes = "Large ";
							Core.Helpers.Constructor innerHtml = new Core.Helpers.Constructor("/Templates/Specific/Cover/Arrange/Attendance/periodsinner.html");

							switch ((dataReader["Status"] as string).ToLower()) {
								case "absent":
									classes += "Red";
									innerHtml.SetVariable("Symbol", "A");
									break;
								case "present":
									classes += "Green";
									innerHtml.SetVariable("Symbol", "P");
									break;
								case "presentyeargroupaway":
									classes += "Blue";
									innerHtml.SetVariable("Symbol", "P");
									break;
								case "presentisbusy":
									classes += "Indigo";
									innerHtml.SetVariable("Symbol", "P");
									break;
							}

							innerHtml.SetVariable("StaffId", this._staffId.ToString());
							innerHtml.SetVariable("Year", this._selectedDate.Year.ToString());
							innerHtml.SetVariable("Month", this._selectedDate.Month.ToString());
							innerHtml.SetVariable("Day", this._selectedDate.Day.ToString());
							innerHtml.SetVariable("Period", period.ToString());

							xmlCollection.Add(id, classes, innerHtml.ToString());
						}	
					}
				}
			}
			
			this.Page.Contents = xmlCollection.ToString();
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

								if (this.Path.HasNext()) {
									bool successfulStaffIdParse = int.TryParse(this.Path.Next(), out this._staffId);

									if (successfulStaffIdParse) {
										if (this.Path.HasNext()) {
											bool successfulPeriodParse = int.TryParse(this.Path.Next(), out this._period);

											if (successfulPeriodParse) {
												try {
													this._period.RequireThat("period").IsInRange(1, this._periods);
													return true;
												}
												catch (IndexOutOfRangeException) {
													return false;
												}
											}
											else
												return false;
										}

										return true;
									}
									else
										return false;
								}
								else
									return false;
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
