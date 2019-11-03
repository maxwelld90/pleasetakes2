using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Persistence.SettingsCollections.Application {

	internal sealed class DefaultsCollection : PersistenceSettingsBase {
		private int _timeout;
		private string _schoolID;

		public DefaultsCollection(Helpers.Xml.Parser parser)
			: base(parser, "/PleaseTakes.Application/Default") {
			int timeout = int.Parse(this.Parser.SelectNode(this.XPath).Attributes["timeout"].Value);
			Validation.Specific.Settings.Timeout(timeout);
			this._timeout = timeout;

			if (this.Parser.AttributeExists(this.XPath, "schoolID")) {
				string schoolID = this.Parser.SelectNode(this.XPath).Attributes["schoolID"].Value;
				Validation.Specific.Settings.SchoolID(schoolID);
				this._schoolID = schoolID;
			}

		}

		public int Timeout {
			get {
				return this._timeout;
			}
		}

		public string SchoolID {
			get {
				return this._schoolID;
			}
		}
	}

}