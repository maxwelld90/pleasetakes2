using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections.Timetabling {

	internal sealed class BaseCollection : SchoolSettingsBase {
		private Layout.BaseCollection _layout;
		private Session.BaseCollection _session;

		public BaseCollection(School school)
			: base(school) {
			this._layout = new Layout.BaseCollection(school);
			this._session = new Session.BaseCollection(school);
		}

		public Layout.BaseCollection Layout {
			get {
				return this._layout;
			}
		}

		public Session.BaseCollection SessionDetails {
			get {
				return this._session;
			}
		}
	}

}