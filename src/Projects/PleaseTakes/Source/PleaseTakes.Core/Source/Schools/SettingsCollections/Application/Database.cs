using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections.Application {

	internal sealed class DatabaseCollection : SchoolSettingsBase {
		private string _connectionString;

		public DatabaseCollection(School school)
			: base(school, "/PleaseTakes.Schools/School[@id='" + school.SchoolID + "']/PleaseTakes.Application/Database") {
			string connectionString = this.Parser.SelectNode(this.XPath).Attributes["connectionString"].Value;
			Validation.Specific.Settings.DatabaseConnectionString(connectionString);

			this._connectionString = connectionString;
		}

		public string ConnectionString {
			get {
				return this._connectionString;
			}
			set {
				Validation.Specific.Settings.DatabaseConnectionString(value);

				this.Parser.SelectNode(this.XPath).Attributes["connectionString"].Value = Helpers.Xml.Formatting.EncodeString(value);
				this.Parser.Save();
				this._connectionString = value;
			}
		}
	}

}