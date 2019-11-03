using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Output {

	internal sealed class StringResource : StringBase {

		public StringResource(Helpers.Resource resource)
			: base(resource) {
			if ((this.Resource.FileExtension.Equals("html")) || (this.Resource.FileExtension.Equals("xsd")) || (this.Resource.FileExtension.Equals("sql")))
				throw new InvalidDirectResourceRequestException("The requested resource '" + this.Resource.ToString() + "' cannot be directly accessed.");
		}

		public override void PrepareOutput() {
			WebServer.Response.Clear();
			this.SetContentType();
			this.Constructor.SetServedTimestamp();
		}

	}

	public class InvalidDirectResourceRequestException : Exception {
		public InvalidDirectResourceRequestException(string message) : base(message) { }
	}

}
