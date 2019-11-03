using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement.Accounts {

	internal sealed class NonTeachingAccount : Account {
		private Core.Schools.SettingsCollections.NonTeachingAccounts.BaseCollection _collection;
		private Core.Helpers.Xml.Parser _parser;
		private string _xPath;

		public NonTeachingAccount(Core.Schools.SettingsCollections.NonTeachingAccounts.BaseCollection collection, Core.Helpers.Xml.Parser parser, string username, string password, string forename, string surname, bool isActive)
			: base(false, username, password, forename, surname, isActive, true) {
			this._username = Core.Helpers.Xml.Formatting.DecodeString(username);
			this._password = Core.Helpers.Xml.Formatting.DecodeString(password);
			this._forename = Core.Helpers.Xml.Formatting.DecodeString(forename);
			this._surname = Core.Helpers.Xml.Formatting.DecodeString(surname);
			this._isActive = isActive;

			this._collection = collection;
			this._parser = parser;
			this._xPath = this._collection.XPathToAccount(this._username);
		}

		public override string Username {
			get {
				return this._username;
			}
			set {
				Core.Validation.Specific.Account.Username(value);

				this._parser.SelectNode(this._xPath).Attributes["username"].Value = Core.Helpers.Xml.Formatting.EncodeString(value);
				this._parser.Save();
				this._username = value;
			}
		}

		public override string Password {
			get {
				return this._password;
			}
			set {
				Core.Validation.Specific.Account.Password(value);

				this._parser.SelectNode(this._xPath).Attributes["password"].Value = Core.Helpers.Xml.Formatting.EncodeString(value);
				this._parser.Save();
				this._password = value;
			}
		}

		public override string Forename {
			get {
				return this._forename;
			}
			set {
				Core.Validation.Specific.Account.Forename(value);

				this._parser.SelectNode(this._xPath).Attributes["forename"].Value = Core.Helpers.Xml.Formatting.EncodeString(value);
				this._parser.Save();
				this._forename = value;
			}
		}

		public override string Surname {
			get {
				return this._surname;
			}
			set {
				Core.Validation.Specific.Account.Surname(value);

				this._parser.SelectNode(this._xPath).Attributes["surname"].Value = Core.Helpers.Xml.Formatting.EncodeString(value);
				this._parser.Save();
				this._surname = value;
			}
		}

		public override bool IsActive {
			get {
				return this._isActive;
			}
			set {
				this._parser.SelectNode(this._xPath).Attributes["active"].Value = Core.Helpers.Xml.Formatting.FormatBool(value);
				this._parser.Save();
				this._isActive = value;
			}
		}
	}

}