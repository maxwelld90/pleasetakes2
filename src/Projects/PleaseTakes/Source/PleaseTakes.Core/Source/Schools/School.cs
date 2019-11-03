using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools {

	internal sealed class School {
		private string _schoolID;
		private string _abbreviatedName;
		private string _fullName;
		private string _country;
		private string _authority;
		private DateTime _registrationDate;
		private Helpers.Xml.Parser _parser;
		private SettingsCollections.BaseCollection _settings;

		public School(string schoolID, string abbreviatedName, string fullName, string country, string authority, DateTime registrationDate) {
			this._schoolID = schoolID;
			this._abbreviatedName = abbreviatedName;
			this._fullName = fullName;
			this._country = country;
			this._authority = authority;
			this._registrationDate = registrationDate;
			this._parser = new Helpers.Xml.Parser("pleasetakes.schools.config");
		}

		public string SchoolID {
			get {
				return this._schoolID;
			}
		}

		public string AbbreviatedName {
			get {
				return this._abbreviatedName;
			}
		}

		public string FullName {
			get {
				return this._fullName;
			}
		}

		public string Country {
			get {
				return this._country;
			}
		}

		public string Authority {
			get {
				return this._authority;
			}
		}

		public DateTime RegistrationDate {
			get {
				return this._registrationDate;
			}
		}

		public Helpers.Xml.Parser Parser {
			get {
				return this._parser;
			}
		}

		public SettingsCollections.BaseCollection Settings {
			get {
				if (this._settings == null) {
					if (!this._parser.NodeExists("/PleaseTakes.Schools/School[@id='" + this._schoolID + "']"))
						throw new MissingSchoolConfigurationSectionException(this._fullName + " (" + this._schoolID + ") does not have a section in the schools configuration file.");

					this._settings = new SettingsCollections.BaseCollection(this);
				}

				return this._settings;
			}
		}
	}

}