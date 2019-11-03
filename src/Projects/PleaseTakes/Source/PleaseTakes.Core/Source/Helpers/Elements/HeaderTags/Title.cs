using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.HeaderTags {

	internal sealed class Title : Tag {

		public Title(string title)
			: base("title") {
			this.ClosesSelf = false;
			this.InnerText = title;
		}

		public string Value {
			get {
				return this.InnerText;
			}
			set {
				this.InnerText = value;
			}
		}

	}

}