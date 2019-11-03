using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.HeaderTags {

	internal sealed class Script : Tag {

		public Script()
			: base("script") {
			this.CreateAttribute("type", "text/javascript");
			this.CreateAttribute("src");

			this.ClosesSelf = false;
		}

		public Script(string source)
			: base("script") {
			this.CreateAttribute("type", "text/javascript");
			this.CreateAttribute("src", source);

			this.ClosesSelf = false;
		}

		public string Source {
			get {
				return this["src"];
			}
			set {
				this["src"] = value;
			}
		}

	}

}