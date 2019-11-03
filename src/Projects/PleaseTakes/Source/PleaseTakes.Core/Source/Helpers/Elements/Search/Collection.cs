using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Search {

	internal sealed class Collection : CollectionBase<SearchArea> {

		public void Add(string id, string ajaxUrl) {
			SearchArea newSearchArea = new SearchArea(id);
			newSearchArea.AjaxUrl = ajaxUrl;

			this.Add(newSearchArea);
		}

		public SearchArea this[string searchAreaId] {
			get {
				return (this.Collection.Single(sA => sA.Id.Equals(searchAreaId)));
			}
		}

	}

}