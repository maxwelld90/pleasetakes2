using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Persistence {

	internal sealed class Application : PersistenceBase {
		private Helpers.Xml.Parser _parser;
		private Schools.Collection _schools;
		private SettingsCollections.Application.DefaultsCollection _defaults;

		public Application() {
			this._parser = new Helpers.Xml.Parser("pleasetakes.application.config");
			this._schools = new Schools.Collection();
			this._defaults = new SettingsCollections.Application.DefaultsCollection(this._parser);
		}

		public Schools.Collection Schools {
			get {
				return this._schools;
			}
		}

		public SettingsCollections.Application.DefaultsCollection Defaults {
			get {
				return this._defaults;
			}
		}
	}

}