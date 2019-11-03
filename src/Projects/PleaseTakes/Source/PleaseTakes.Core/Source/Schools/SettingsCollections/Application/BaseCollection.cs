using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections.Application {

	internal sealed class BaseCollection : SchoolSettingsBase {
		private DatabaseCollection _database;
		private SessionCollection _session;

		public BaseCollection(School school)
			: base(school) {
			this._database = new DatabaseCollection(this.School);
			this._session = new SessionCollection(this.School);
		}

		public DatabaseCollection Database {
			get {
				return this._database;
			}
		}

		public SessionCollection Session {
			get {
				return this._session;
			}
		}
	}

}