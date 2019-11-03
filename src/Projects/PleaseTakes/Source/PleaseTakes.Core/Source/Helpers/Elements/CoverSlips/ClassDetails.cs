using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.CoverSlips {

	internal sealed class ClassDetails {
		private string _name;
		private string _subject;

		public string Name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		public string Subject {
			get {
				return this._subject;
			}
			set {
				this._subject = value;
			}
		}
	}

}
