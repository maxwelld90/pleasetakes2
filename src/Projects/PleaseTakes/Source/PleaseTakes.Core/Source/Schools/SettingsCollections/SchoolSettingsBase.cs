using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections {

	internal abstract class SchoolSettingsBase : Helpers.SettingsBase {
		private School _school;

		public SchoolSettingsBase(School school, string xPath)
			: base(school.Parser, xPath) {
			this._school = school;
		}

		public SchoolSettingsBase(School school)
			: base(school.Parser) {
			this._school = school;
		}

		protected School School {
			get {
				return this._school;
			}
		}
	}

}