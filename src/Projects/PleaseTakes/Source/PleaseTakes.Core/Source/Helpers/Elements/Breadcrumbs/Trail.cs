using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Breadcrumbs {

	internal sealed class Trail : CollectionBase<Breadcrumb> {

		public void Add(string displayText, string href, string onClick) {
			Breadcrumb newBreadcrumb = new Breadcrumb();
			newBreadcrumb.DisplayText = displayText;

			if (!string.IsNullOrEmpty(href))
				newBreadcrumb.Href = href;

			if (!string.IsNullOrEmpty(onClick))
				newBreadcrumb.OnClick = onClick;

			this.Add(newBreadcrumb);
		}

		public void Add(string displayText) {
			Breadcrumb newBreadcrumb = new Breadcrumb();
			newBreadcrumb.DisplayText = displayText;

			this.Add(newBreadcrumb);
		}

		public override string ToString() {
			Constructor trailConstructor = new Constructor("/Templates/Elements/Breadcrumbs/trail.html");
			string trailStr = "";

			for (int i = 0; (i <= (this.Collection.Count - 1)); i++) {
				if (i.Equals(this.Collection.Count - 1))
					trailStr += this.Collection[i].ToString().Replace(Consts.BreadcrumbSeparator, "");
				else
					trailStr += this.Collection[i] + "\n" + new string('\t', 11);
			}

			trailConstructor.SetVariable("Trail", trailStr);
			return trailConstructor.ToString();
		}

	}

}