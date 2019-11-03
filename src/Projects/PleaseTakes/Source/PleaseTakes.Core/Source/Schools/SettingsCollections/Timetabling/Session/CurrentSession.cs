using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections.Timetabling.Session {

	internal sealed class CurrentSession : SchoolSettingsBase {
		private DateTime _startDate;
		private DateTime _endDate;
		private string _sessionName;

		public CurrentSession(School school)
			: base(school, "/PleaseTakes.Schools/School[@id='" + school.SchoolID + "']/Timetabling/SessionDetails") {
			DateTime startDate = DateTime.Parse(this.Parser.SelectNode(this.XPath).Attributes["startDate"].Value);
			DateTime endDate = DateTime.Parse(this.Parser.SelectNode(this.XPath).Attributes["endDate"].Value);

			Validation.Specific.Settings.StartDate(startDate, endDate);
			Validation.Specific.Settings.EndDate(endDate, startDate);

			this._startDate = startDate;
			this._endDate = endDate;

			this.SetSessionName();
		}

		public DateTime StartDate {
			get {
				return this._startDate;
			}
			set {
				Validation.Specific.Settings.StartDate(value, this._endDate);
				this.Parser.SelectNode(this.XPath).Attributes["startDate"].Value = value.ToString(Consts.DateTimeISO);
				this.Parser.Save();
				this._endDate = value;

				this.SetSessionName();
			}
		}

		public DateTime StartDateMonth {
			get {
				return new DateTime(this._startDate.Year, this._startDate.Month, 1);
			}
		}

		public DateTime EndDate {
			get {
				return this._endDate;
			}
			set {
				Validation.Specific.Settings.EndDate(value, this._startDate);
				this.Parser.SelectNode(this.XPath).Attributes["endDate"].Value = value.ToString(Consts.DateTimeISO);
				this.Parser.Save();
				this._endDate = value;

				this.SetSessionName();
			}
		}

		public DateTime EndDateMonth {
			get {
				return new DateTime(this._endDate.Year, this._endDate.Month, 1);
			}
		}

		public int TotalWeeks {
			get {
				DateTime startDate = Utils.GetWeekDates(true, this._startDate);
				DateTime endDate = Utils.GetWeekDates(false, this._endDate);

				return (((endDate - startDate).Days + 1) / 7);
			}
		}

		public string Name {
			get {
				return this._sessionName;
			}
		}

		private void SetSessionName() {
			if (this._startDate.Year.Equals(this._endDate.Year))
				this._sessionName = this._startDate.Year.ToString();
			else
				this._sessionName = this._startDate.Year.ToString() + "/" + this._endDate.Year.ToString();
		}

		public bool IsDateInSession(DateTime date) {
			return Utils.IsDateInRange(this._startDate, this._endDate, date);
		}

		public SessionDate GetDateSessionInformation(DateTime date) {
			if (Utils.IsDateInRange(this._startDate, this._endDate, date)) {
				int timetableWeek = 1;
				DateTime weekStart = Utils.GetWeekDates(true, this._startDate);
				DateTime weekEnd = Utils.GetWeekDates(false, this._startDate);

				for (int weekCounter = 1; (weekCounter <= this.TotalWeeks); weekCounter++) {
					if (Utils.IsDateInRange(weekStart, weekEnd, date))
						return new SessionDate(weekCounter, timetableWeek, weekStart, weekEnd);

					if (timetableWeek.Equals(WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoWeeks))
						timetableWeek = 1;
					else
						timetableWeek++;

					weekStart = weekStart.AddDays(7);
					weekEnd = weekEnd.AddDays(7);
				}

				throw new ArgumentOutOfRangeException("date", "The supplied date is outwith the range of the current session.");
			}
			else
				throw new ArgumentOutOfRangeException("date", "The supplied date is outwith the range of the current session.");
		}

		
	}

}