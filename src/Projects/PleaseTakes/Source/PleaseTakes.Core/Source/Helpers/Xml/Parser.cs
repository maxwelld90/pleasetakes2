using System;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace PleaseTakes.Core.Helpers.Xml {

	internal sealed partial class Parser {
		private string _xmlFilename;
		private XmlDocument _document;

		public Parser(string xmlFilename) {
			this._xmlFilename = xmlFilename;
			this._document = new XmlDocument();
			this._document.Load(Utils.MappedApplicationPath + this._xmlFilename);

			new XmlValidator(this._document, this._xmlFilename);
		}

		public void Reload() {
			this._document.Load(Utils.MappedApplicationPath + this._xmlFilename);
			new XmlValidator(this._document, this._xmlFilename);
		}

		public XmlDocument Document {
			get {
				return this._document;
			}
		}

		public string Filename {
			get {
				return this._xmlFilename;
			}
		}

		public XmlNode SelectNode(string xPath) {
			return this._document.SelectSingleNode(xPath);
		}

		public XmlNodeList SelectNodes(string xPath) {
			return this._document.SelectNodes(xPath);
		}

		public bool NodeExists(string xPath) {
			if (this._document.SelectSingleNode(xPath) == null)
				return false;
			return true;
		}

		public bool AttributeExists(string xPath, string attributeName) {
			if ((this._document.SelectSingleNode(xPath).Attributes[attributeName] == null) || (this._document.SelectSingleNode(xPath).Attributes[attributeName].Value.Equals("")))
				return false;
			return true;
		}

		public XmlNodeList ChildNodes(string xPath) {
			return this._document.SelectSingleNode(xPath).ChildNodes;
		}

		public bool HasChildNodes(string xPath) {
			return (this._document.SelectSingleNode(xPath).ChildNodes.Count > 0);
		}

		public void RemoveNode(string xPath) {
			this._document.SelectSingleNode(xPath).ParentNode.RemoveChild(
				this._document.SelectSingleNode(xPath));
		}

		public void RemoveChildren(string xPath) {
			XmlNodeList childList = this._document.SelectNodes(xPath);

			for (int i = (childList.Count - 1); i >= 0; i--)
				childList[i].ParentNode.RemoveChild(childList[i]);
		}

		public void RemoveAttribute(string xPath, string attribute) {
			XmlElement node = (XmlElement)this._document.SelectSingleNode(xPath);
			node.RemoveAttribute(attribute);
		}

		public void CreateAttribute(string xPath, string attribute, string attributeValue) {
			XmlElement node = (XmlElement)this._document.SelectSingleNode(xPath);
			node.SetAttribute(attribute, attributeValue);
		}

		public void CreateNode(string xPathParent, string nodeRepresentation) {
			XmlReader xmlReader = XmlReader.Create(new StringReader(nodeRepresentation));
			this._document.SelectSingleNode(xPathParent).AppendChild(this._document.ReadNode(xmlReader));
		}

		public void Save() {
			new XmlValidator(this._document, this._xmlFilename);

			XmlTextWriter xmlWriter = new XmlTextWriter(Utils.MappedApplicationPath + this._xmlFilename, System.Text.UTF8Encoding.UTF8);
			xmlWriter.Formatting = System.Xml.Formatting.Indented;

			this._document.Save(xmlWriter);
			xmlWriter.Close();
		}
	}

}
