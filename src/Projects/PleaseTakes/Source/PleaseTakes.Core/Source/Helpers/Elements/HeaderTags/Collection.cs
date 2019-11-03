using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.HeaderTags {

	internal sealed class Collection : CollectionBase<Tag> {

		public override string ToString() {
			string returnStr = "";

			foreach (Tag tag in this.Collection)
				returnStr += new string('\t', 2) + tag + "\n";

			returnStr += "End";

			return returnStr.Replace("\nEnd", "");
		}

	}

}