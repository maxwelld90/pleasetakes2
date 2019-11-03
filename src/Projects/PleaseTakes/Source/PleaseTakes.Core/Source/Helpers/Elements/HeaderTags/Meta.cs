using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.HeaderTags {

	internal sealed class Meta : Tag {

		public Meta()
			: base("meta") {
			this.CreateAttribute("http-equiv");
			this.CreateAttribute("content");

			this.ClosesSelf = true;
		}

		public Meta(string httpEquiv, string content)
			: base("meta") {
			this.CreateAttribute("http-equiv", httpEquiv);
			this.CreateAttribute("content", content);

			this.ClosesSelf = true;
		}

		public string HttpEquiv {
			get {
				return this["http-equiv"];
			}
			set {
				this["http-equiv"] = value;
			}
		}

		public string Content {
			get {
				return this["content"];
			}
			set {
				this["content"] = value;
			}
		}

	}


}