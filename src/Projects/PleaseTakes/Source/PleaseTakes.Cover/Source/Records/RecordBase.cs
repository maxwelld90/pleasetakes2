using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Records {

	internal abstract class RecordBase {
		protected Core.Helpers.Elements.RecordLists.Record _record;

		public RecordBase() {
			this._record = new Core.Helpers.Elements.RecordLists.Record();
		}

		public string Href {
			get {
				return this._record.Href;
			}
			set {
				this._record.Href = value;
			}
		}

		public string OnClick {
			get {
				return this._record.OnClick;
			}
			set {
				this._record.OnClick = value;
			}
		}

		public string InnerHtml {
			get {
				return this._record.InnerHtml;
			}
		}

		public Core.Helpers.Elements.RecordLists.Record Record {
			get {
				return this._record;
			}
		}

		public override string ToString() {
			return this._record.ToString();
		}
	}

}