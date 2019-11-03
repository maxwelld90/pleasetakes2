using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Output {

	internal sealed class Page : StringBase {

		public Page(Helpers.BaseHandlers.BaseHandler handler)
			: base() {
			this.Constructor = handler.Page;
		}

		public override void PrepareOutput() {
			WebServer.Response.Clear();
			this.SetContentType("text/html");
		}

	}

}
