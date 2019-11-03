using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.RecordLists {

	internal sealed class Collection : CollectionBase<Record> {
		private string _id;
		private bool _topSpace;

		public Collection() { }

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public bool TopSpace {
			get {
				return this._topSpace;
			}
			set {
				this._topSpace = value;
			}
		}

		public override string ToString() {
			Constructor collection = new Constructor("/Templates/Elements/Recordlists/collection.html");
			string collectionStr = "";

			foreach (Record record in this.Collection)
				collectionStr += record;

			if (string.IsNullOrEmpty(this._id)) {
				collection.DeleteVariable("Id");
				collection.DeleteVariable("CollectionId");
			}
			else
				collection.SetVariable("Id", " id=\"RecordCollection" + this._id + "\"");

			collection.SetVariable("Collection", collectionStr);
			collection.SetVariable("CollectionId", this._id);

			if (this._topSpace)
				collection.SetVariable("TopSpace", " TopSpace");
			else
				collection.DeleteVariable("TopSpace");

			return collection.ToString();
		}

	}

}
