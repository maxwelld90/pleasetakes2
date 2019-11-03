using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Output {

	internal abstract class OutputBase {
		private Helpers.Resource _resource;

		public OutputBase(Helpers.Resource resource) {
			this._resource = resource;
		}

		public OutputBase() {
			this._resource = null;
		}

		public Helpers.Resource Resource {
			get {
				if (this._resource == null)
					throw new NullReferenceException();

				return this._resource;
			}
		}

		public bool IsFromResource {
			get {
				if (this._resource == null)
					return false;
				return true;
			}
		}

		public void SetContentType() {
			if (!IsFromResource)
				throw new InvalidResponseRequestException("Cannot set the content type for a response when not utilising an embedded response.");

			WebServer.Response.ContentType = this._resource.ContentType;
		}

		public void SetContentType(string contentType) {
			WebServer.Response.ContentType = contentType;
		}

		public abstract void PrepareOutput();

		public abstract void Send();

	}

	public class InvalidResponseRequestException : Exception {
		public InvalidResponseRequestException(string message) : base(message) { }
	}

}