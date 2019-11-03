using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers {

	internal sealed class Constructor {
		private string _contents;

		public Constructor(string templatePath) {
			this._contents = new Resource(templatePath).StringRepresentation;
		}

		public Constructor(Resource resource) {
			this._contents = resource.StringRepresentation;
		}

		public Constructor() {
			this._contents = "";
		}

		public string Contents {
			get {
				return this._contents;
			}
			set {
				this._contents = value;
			}
		}

		public void SetVariable(string variableName, string replacement) {
			this._contents = this._contents.Replace(
				Consts.StringVariableMarkerStart + variableName + Consts.StringVariableMarkerEnd,
				replacement);
		}

		public void SetVariable(string variableName, Constructor replacement) {
			this.SetVariable(variableName, replacement.ToString());
		}

		public void DeleteVariable(string variableName) {
			this.SetVariable(variableName, "");
		}

		public void SetServedTimestamp() {
			this.SetVariable(
				"ServedTimestamp",
				DateTime.Now.ToString(Consts.DateTimeFull));
		}

		public bool IsEmpty {
			get {
				return (string.IsNullOrEmpty(this._contents));
			}
		}

		public override string ToString() {
			return this._contents;
		}
	}

}