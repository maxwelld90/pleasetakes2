using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections.Timetabling.Session {

	internal class BaseCollection : SchoolSettingsBase {
		private bool _useCurrent;
		private CurrentSession _currentSession;

		public BaseCollection(School school)
			: base(school, "/PleaseTakes.Schools/School[@id='" + school.SchoolID + "']/Timetabling/SessionDetails") {
			this._useCurrent = bool.Parse(this.Parser.SelectNode(this.XPath).Attributes["useCurrent"].Value);

			this._currentSession = new CurrentSession(school);
		}

		public bool UseCurrent {
			get {
				return this._useCurrent;
			}
			set {
				this.Parser.SelectNode(this.XPath).Attributes["useCurrent"].Value = Helpers.Xml.Formatting.FormatBool(value);
				this.Parser.Save();
				this._useCurrent = value;
			}
		}

		public CurrentSession CurrentSession {
			get {
				return this._currentSession;
			}
		}
	}

}