using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections {

	internal sealed class BaseCollection : SchoolSettingsBase {
		private Application.BaseCollection _application;
		private NonTeachingAccounts.BaseCollection _nonTeachingAccounts;
		private Timetabling.BaseCollection _timetabling;

		public BaseCollection(School school)
			: base(school) {
			this._application = new Application.BaseCollection(this.School);
			this._nonTeachingAccounts = new NonTeachingAccounts.BaseCollection(this.School);
			this._timetabling = new Timetabling.BaseCollection(this.School);
		}

		public Application.BaseCollection Application {
			get {
				return this._application;
			}
		}

		public NonTeachingAccounts.BaseCollection NonTeachingAccounts {
			get {
				return this._nonTeachingAccounts;
			}
		}

		public Timetabling.BaseCollection Timetabling {
			get {
				return this._timetabling;
			}
		}
	}


}
