using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.BaseHandlers {

	internal abstract class AjaxHandler : BaseHandler {
		private Path.Parser _source;

		public AjaxHandler(Path.Parser path)
			: base(TemplateTypes.None, path) {
			this._source = new Path.Parser(Core.WebServer.Request["sourcePath"]);
			this.GenerateOutput();
		}

		protected abstract void GenerateOutput();

		protected Path.Parser SourcePath {
			get {
				return this._source;
			}
		}

	}

}