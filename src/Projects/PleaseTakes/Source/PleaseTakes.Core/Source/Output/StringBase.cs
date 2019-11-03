using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Output {

	internal abstract class StringBase : OutputBase {
		private Helpers.Constructor _constructor;

		public StringBase(Helpers.Resource resource)
			: base(resource) {
			this._constructor = new Helpers.Constructor(resource);
		}

		public StringBase() {
			this._constructor = new Helpers.Constructor();
		}

		protected Helpers.Constructor Constructor {
			get {
				return this._constructor;
			}
			set {
				this._constructor = value;
			}
		}

		public string Output {
			get {
				return this._constructor.Contents;
			}
			set {
				this._constructor.Contents = value;
			}
		}

		public override void Send() {
			WebServer.Response.Write(this._constructor.Contents);
			WebServer.Response.End();
		}

		public override string ToString() {
			return this._constructor.Contents;
		}
	}

}