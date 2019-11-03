using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Tabs {

	internal sealed class Collection : CollectionBase<Tab> {

		public void Add(string id, string title, string templatePath) {
			Tab newTab = new Tab(id, title, templatePath);

			this.Add(newTab);
		}

		public Tab this[string tabId] {
			get {
				return (this.Collection.Single(t => t.Id.Equals(tabId)));
			}
		}

		public string TabList {
			get {
				string returnStr = "<div id=\"Tabs\">\n" + new string('\t', 6) + "<ul id=\"TabsList\">\n";

				foreach (Tab tab in this.Collection)
					returnStr += new string('\t', 7) + "<li><a href=\"#" + tab.Id + "\">" + tab.Title + "</a></li>\n";

				return returnStr += new string('\t', 6) + "</ul>\n" + new string('\t', 5) + "</div>\n";
			}
		}

		public string TabContents {
			get {
				string returnStr = "\n";

				foreach (Tab tab in this.Collection)
					returnStr += new string('\t', 8) + "<div class=\"TabContent\" id=\"" + tab.Id + "\">\n" + tab.ToString() + "\n" + new string('\t', 8) + "</div>\n";

				return returnStr;
			}
		}

	}

}