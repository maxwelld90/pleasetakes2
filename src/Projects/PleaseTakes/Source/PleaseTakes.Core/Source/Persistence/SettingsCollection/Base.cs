using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Persistence.SettingsCollections {

	internal abstract class PersistenceSettingsBase : Helpers.SettingsBase {

		public PersistenceSettingsBase(Helpers.Xml.Parser parser, string xPath)
			: base(parser, xPath) {
		}

		public PersistenceSettingsBase(Helpers.Xml.Parser parser)
			: base(parser) {
		}

	}

}