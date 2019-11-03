using System;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace PleaseTakes.Core.Helpers.Xml {

	internal sealed partial class Parser {

		private sealed class XmlValidator {
			private StringReader _document;
			private StringReader _schema;

			public XmlValidator(XmlDocument xmlDocument, string xmlFilename) {
				this._document = new StringReader(xmlDocument.OuterXml);
				this._schema = new StringReader(new Constructor("/Schemas/" + xmlFilename + ".xsd").ToString());

				this.Validate();
			}

			private void Validate() {
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ValidationType = ValidationType.Schema;
				settings.ProhibitDtd = false;

				XmlSchemaSet schemas = new XmlSchemaSet();
				settings.Schemas = schemas;
				schemas.Add(null, XmlReader.Create(this._schema));

				settings.ValidationEventHandler += ValidateDocumentEventHandler;
				XmlReader validator = XmlReader.Create(this._document, settings);

				try {
					while (validator.Read()) { }
				}
				catch (XmlException e) {
					throw new InvalidDataException(e.Message);
				}
				finally {
					validator.Close();
				}
			}

			private static void ValidateDocumentEventHandler(object sender, ValidationEventArgs args) {
				throw new InvalidDataException(args.Message);
			}
		}

	}
}
