using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal abstract class FormElement {
		private string _name;
		private string _id;
		private int _tabIndex;

		public string Name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public int TabIndex {
			get {
				return this._tabIndex;
			}
			set {
				this._tabIndex = value;
			}
		}

		public abstract override string ToString();
	}

}