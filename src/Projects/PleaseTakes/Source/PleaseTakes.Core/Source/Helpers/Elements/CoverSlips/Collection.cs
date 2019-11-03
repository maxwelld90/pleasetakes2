using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.CoverSlips {

	internal sealed class Collection : CollectionBase<Slip> {

		public override string ToString() {
			string returnStr = "";

			foreach (Slip slip in this.Collection)
				returnStr += slip;

			return returnStr;
		}

	}

}
