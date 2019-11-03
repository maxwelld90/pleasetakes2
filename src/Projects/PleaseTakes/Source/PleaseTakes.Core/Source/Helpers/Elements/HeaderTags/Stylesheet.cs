using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.HeaderTags {

	internal sealed class Stylesheet : Tag {

		public Stylesheet()
			: base("link") {
			this.CreateAttribute("rel", "stylesheet");
			this.CreateAttribute("type", "text/css");
			this.CreateAttribute("href");
			this.CreateAttribute("media");

			this.ClosesSelf = true;
		}

		public Stylesheet(string href, string media)
			: base("link") {
			this.CreateAttribute("rel", "stylesheet");
			this.CreateAttribute("type", "text/css");
			this.CreateAttribute("href", href);
			this.CreateAttribute("media", media);

			this.ClosesSelf = true;
		}

		public string Source {
			get {
				return this["href"];
			}
			set {
				this["href"] = value;
			}
		}

		public string Media {
			get {
				return this["media"];
			}
			set {
				this["media"] = value;
			}
		}

	}

}