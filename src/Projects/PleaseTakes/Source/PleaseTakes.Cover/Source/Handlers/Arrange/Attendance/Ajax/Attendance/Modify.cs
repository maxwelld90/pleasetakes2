using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Attendance.Ajax.Attendance {

	internal sealed class Modify : Core.Helpers.BaseHandlers.AjaxHandler {
		private DateTime _selectedDate;
		private int _timetableWeek;
		private int _periods;

		public Modify(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.SetContentType("text/xml");
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			Core.Helpers.Elements.Ajax.Xml.Collection xmlCollection = new Core.Helpers.Elements.Ajax.Xml.Collection();

			if (this.CheckInputValidity()) {
				if (this.Path.HasNext()) {
					int staffId;
					bool successfulParse = int.TryParse(this.Path.Next(), out staffId);

					if (successfulParse) {
						Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
						paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", staffId);
						paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@Date", this._selectedDate);

						using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/Arrange/Attendance/Attendance/modify.sql", paramBuilder.Parameters)) {
							if (dataReader.Read())
								xmlCollection.Add(null, dataReader["Colour"] as string, null);
							else
								xmlCollection.Add(null, "Yellow", null);
						}
					}
					else
						xmlCollection.Add(null, "Yellow", null);
				}
				else
					xmlCollection.Add(null, "Yellow", null);
			}
			else
				xmlCollection.Add(null, "Yellow", null);

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
