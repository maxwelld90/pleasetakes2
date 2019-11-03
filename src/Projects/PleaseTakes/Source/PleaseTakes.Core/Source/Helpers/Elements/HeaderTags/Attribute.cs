using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.HeaderTags {

	internal abstract partial class Tag {

		private sealed class Attribute {
			private string _name;
			private string _value;

			public string Name {
				get {
					return this._name;
				}
				set {
					this._name = value;
				}
			}

			public string Value {
				get {
					return this._value;
				}
				set {
					this._value = value;
				}
			}

			public override string ToString() {
				return this._name + "=\"" + this._value + "\"";
			}
		}

	}

}