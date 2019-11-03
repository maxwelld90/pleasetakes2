using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Core.Schools.SettingsCollections.Timetabling.Layout {

	internal class BaseCollection : SchoolSettingsBase {
		private int _noWeeks;
		private int _noPeriods;
		private Dictionary<int, Week> _dictionary;

		public BaseCollection(School school)
			: base(school, "/PleaseTakes.Schools/School[@id='" + school.SchoolID + "']/Timetabling/Layout") {
			this._dictionary = new Dictionary<int, Week>();

			if (this.Parser.HasChildNodes(this.XPath)) {
				foreach (XmlNode dayNode in this.Parser.ChildNodes(this.XPath)) {
					int weekNo = int.Parse(dayNode.Attributes["weekNo"].Value);

					weekNo.RequireThat("week value").IsInRange(1, (int)Consts.TimetablingLimits.WeekLimit);

					if (!this.Exists(weekNo)) {
						this.Add(weekNo);
						this.SumPeriods(weekNo);
					}
				}

				this._noWeeks = this._dictionary.Keys.Max();
			}
			else
				this._noWeeks = 0;
		}

		private void SumPeriods(int weekNo) {
			foreach (XmlNode dayNode in this.Parser.SelectNodes(this.XPath + "/Day[@weekNo='" + weekNo + "']"))
				this._noPeriods += int.Parse(dayNode.Attributes["periods"].Value);
		}

		public int NoPeriods {
			get {
				return this._noPeriods;
			}
		}

		public Week this[int weekNo] {
			get {
				try {
					return this._dictionary[weekNo];
				}
				catch (KeyNotFoundException) {
					throw new WeekNotFoundException("The specified week has not been defined for this school.");
				}
			}
		}

		public int NoWeeks {
			get {
				return this._noWeeks;
			}
		}

		public bool Exists(int weekNo) {
			return this._dictionary.ContainsKey(weekNo);
		}

		public void Add(int weekNo) {
			weekNo.RequireThat("week value").IsInRange(1, (int)Consts.TimetablingLimits.WeekLimit);

			this._dictionary.Add(weekNo, new Week(School, weekNo));
			this._noWeeks = this._dictionary.Keys.Max();
		}

		public void Remove(int weekNo) {
			weekNo.RequireThat("week value").IsInRange(1, (int)Consts.TimetablingLimits.WeekLimit);

			if (!this.Exists(weekNo))
				throw new WeekNotFoundException("The specified week has not been defined for this school.");

			this.Parser.RemoveChildren(this.XPath + "/Day[@weekNo='" + weekNo + "']");
			this.Parser.Save();
			this._dictionary.Remove(weekNo);
		}
	}

	public class WeekNotFoundException : Exception {
		public WeekNotFoundException(string message) : base(message) { }
	}

}