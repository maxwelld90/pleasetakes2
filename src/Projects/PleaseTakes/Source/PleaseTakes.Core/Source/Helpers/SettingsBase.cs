using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers {

	internal abstract class SettingsBase {
		private Xml.Parser _parser;
		private string _xPath;

		public SettingsBase(Xml.Parser parser, string xPath) {
			this._parser = parser;
			this._xPath = xPath;
		}

		public SettingsBase(Xml.Parser parser) {
			this._parser = parser;
		}

		protected Xml.Parser Parser {
			get {
				return this._parser;
			}
		}

		protected string XPath {
			get {
				return this._xPath;
			}
		}
	}

}