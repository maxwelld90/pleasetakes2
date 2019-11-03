using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.BaseHandlers {

	internal abstract class BaseHandler {
		private Constructor _page;
		private Path.Parser _path;
		private Output.Page _output;

		public BaseHandler(TemplateTypes templateType, Path.Parser path) {
			this._path = path;

			switch (templateType) {
				case TemplateTypes.Base:
					this._page = new Constructor("/Templates/base.html");
					break;
				case TemplateTypes.Quote:
					this._page = new Constructor("/Templates/quote.html");
					break;
				default:
					this._page = new Constructor();
					break;
			}

			this.Page.SetServedTimestamp();
			this.Output.PrepareOutput();
		}

		public Constructor Page {
			get {
				return this._page;
			}
		}

		protected Path.Parser Path {
			get {
				return this._path;
			}
		}

		public Output.Page Output {
			get {
				if (this._output != null)
					return this._output;
				else {
					this._output = new Output.Page(this);
					return this._output;
				}
			}
		}
	}

}