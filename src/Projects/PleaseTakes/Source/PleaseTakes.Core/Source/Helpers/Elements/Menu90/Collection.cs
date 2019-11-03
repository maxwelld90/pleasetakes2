using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Menu90 {

	internal sealed class Collection : CollectionBase<Link> {

		public void Add(string href, string image, string title, string description, string rollover) {
			Link newLink = new Link();
			newLink.Href = href;
			newLink.Image = image;
			newLink.Title = title;
			newLink.Description = description;
			newLink.Rollover = rollover;

			this.Add(newLink);
		}

		public override string ToString() {
			string returnStr = "<ul class=\"Menu90\">\n";

			foreach (Link link in this.Collection) {
				returnStr += link;
			}

			return returnStr + new string('\t', 10) + "</ul>";
		}

	}

}