using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.RightMenu {

	internal sealed class Collection : CollectionBase<Link> {

		public void Add(string href, string image, string rollover, string hiddenText) {
			Link newLink = new Link();
			newLink.Href = href;
			newLink.Image = image;
			newLink.Rollover = rollover;
			newLink.HiddenText = hiddenText;

			this.Add(newLink);
		}

		public override string ToString() {
			string returnStr = new string('\t', 8) + "<ul>\n";

			foreach (Link link in this.Collection)
				returnStr += link;

			return returnStr + new string('\t', 8) + "</ul>";
		}

	}

}
