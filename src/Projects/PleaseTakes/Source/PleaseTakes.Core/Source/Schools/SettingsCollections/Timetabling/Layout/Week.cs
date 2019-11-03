using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Core.Schools.SettingsCollections.Timetabling.Layout {

	internal class Week : SchoolSettingsBase {
		private Dictionary<int, int> _dictionary;
		private int _weekNo;

		public Week(School school, int weekNo)
			: base(school, "/PleaseTakes.Schools/School[@id='" + school.SchoolID + "']/Timetabling/Layout") {
			this._dictionary = new Dictionary<int, int>(7);
			this._weekNo = weekNo;

			foreach (XmlNode dayNode in this.Parser.SelectNodes(this.XPath + "/Day[@weekNo='" + weekNo + "']")) {
				int dayNo = int.Parse(dayNode.Attributes["dayNo"].Value);
				int periods = int.Parse(dayNode.Attributes["periods"].Value);

				dayNo.RequireThat("day number").IsInRange(1, 7);
				periods.RequireThat("number of periods").IsInRange(1, (int)Consts.TimetablingLimits.PeriodLimit);

				if (this.Exists(dayNo))
					throw new DuplicateDayException("A duplicate day value for the same week has been found.");

				this._dictionary.Add(dayNo, periods);
			}
		}

		public int this[int dayNo] {
			get {
				try {
					return this._dictionary[dayNo];
				}
				catch (KeyNotFoundException) {
					throw new DayNotFoundException("The specified day has not been defined.");
				}
			}
			set {
				value.RequireThat("updated number of periods").IsInRange(1, (int)Consts.TimetablingLimits.PeriodLimit);

				if (!int.Parse(this.Parser.SelectNode(this.XPath + "/Day[@weekNo='" + this._weekNo + "' and @dayNo='" + dayNo + "']").Attributes["periods"].Value).Equals(value)) {
					this.Parser.SelectNode(this.XPath + "/Day[@weekNo='" + this._weekNo + "' and @dayNo='" + dayNo + "']").Attributes["periods"].Value = value.ToString();
					this.Parser.Save();
					this._dictionary[dayNo] = value;
				}
			}
		}

		public int HighestPeriodValue {
			get {
				int highestValue = 0;

				foreach (int value in this._dictionary.Values) {
					if (value > highestValue)
						highestValue = value;
				}

				return highestValue;
			}
		}

		public bool Exists(int dayNo) {
			return this._dictionary.ContainsKey(dayNo);
		}

		public void Add(int dayNo, int periods) {
			dayNo.RequireThat("day number").IsInRange(1, 7);
			periods.RequireThat("number of periods").IsInRange(1, (int)Consts.TimetablingLimits.PeriodLimit);

			if (this.Exists(dayNo))
				throw new DuplicateDayException("A duplicate day value for the same week has been found.");

			this.Parser.CreateNode(this.XPath, "<Day weekNo=\"" + this._weekNo + "\" dayNo=\"" + dayNo + "\" periods=\"" + periods + "\" />");
			this.Parser.Save();
			this._dictionary.Add(dayNo, periods);
		}

		public void Remove(int dayNo) {
			dayNo.RequireThat("day number").IsInRange(1, 7);

			if (!this.Exists(dayNo))
				throw new DayNotFoundException("The specified day has not been defined.");

			this.Parser.RemoveNode(this.XPath + "/Day[@weekNo='" + this._weekNo + "' and @dayNo='" + dayNo + "']");
			this.Parser.Save();
			this._dictionary.Remove(dayNo);
		}
	}

	public class DuplicateDayException : Exception {
		public DuplicateDayException(string message) : base(message) { }
	}

	public class DayNotFoundException : Exception {
		public DayNotFoundException(string message) : base(message) { }
	}

}