using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections.Timetabling.Session {

	internal sealed class SessionDate {
		private int _sessionWeek;
		private int _timetableWeek;
		private DateTime _timetableWeekStart;
		private DateTime _timetableWeekEnd;

		public SessionDate(int sessionWeek, int timetableWeek, DateTime timetableWeekStart, DateTime timetableWeekEnd) {
			this._sessionWeek = sessionWeek;
			this._timetableWeek = timetableWeek;
			this._timetableWeekStart = timetableWeekStart;
			this._timetableWeekEnd = timetableWeekEnd;
		}

		public int SessionWeek {
			get {
				return this._sessionWeek;
			}
		}

		public int TimetableWeek {
			get {
				return this._timetableWeek;
			}
		}

		public DateTime RotationStartDate {
			get {
				if (!this._timetableWeek.Equals(1))
					return this._timetableWeekStart.AddDays(-((this._timetableWeek * 7) - 7));
				else
					return this._timetableWeekStart;
			}
		}

		public DateTime RotationEndDate {
			get {
				return this._timetableWeekEnd.AddDays((WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoWeeks - this._timetableWeek) * 7);
			}
		}
	}

}
