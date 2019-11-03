using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Output {

	internal sealed class BinaryResource : OutputBase {

		public BinaryResource(Helpers.Resource resource)
			: base(resource) {
		}

		public override void PrepareOutput() {
			WebServer.Response.Clear();
			this.SetContentType();
		}

		public override void Send() {
			WebServer.Response.BinaryWrite(this.Resource.ByteRepresentation);
			WebServer.Response.End();
		}

	}
}
