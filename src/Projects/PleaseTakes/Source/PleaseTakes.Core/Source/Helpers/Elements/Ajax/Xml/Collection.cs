using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Ajax.Xml {

	internal sealed class Collection : CollectionBase<Element> {

		public void Add(string id, string className, string innerHtml) {
			Element newElement = new Element();
			newElement.Id = id;
			newElement.ClassName = className;
			newElement.InnerHtml = innerHtml;

			this.Add(newElement);
		}

		public override string ToString() {
			Constructor collection = new Constructor("/Templates/Elements/Ajax/Xml/collection.xml");
			string collectionStr = "";

			foreach (Element element in this.Collection)
				collectionStr += element;

			collection.SetVariable("Collection", collectionStr);
			return collection.ToString();
		}

	}

}