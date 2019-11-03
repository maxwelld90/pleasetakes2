using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Ajax.Xml {

	internal sealed class Element {
		private string _id;
		private string _value;
		private string _className;
		private string _innerHtml;

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string Value {
			get {
				return this._value;
			}
			set {
				this._value = value;
			}
		}

		public string ClassName {
			get {
				return this._className;
			}
			set {
				this._className = value;
			}
		}

		public string InnerHtml {
			get {
				return this._innerHtml;
			}
			set {
				this._innerHtml = value;
			}
		}

		public override string ToString() {
			Constructor element = new Constructor("/Templates/Elements/Ajax/Xml/element.xml");

			if (string.IsNullOrEmpty(this._id))
				element.DeleteVariable("Id");
			else
				element.SetVariable("Id", " id=\"" + this._id + "\"");

			if (string.IsNullOrEmpty(this._value))
				element.DeleteVariable("Value");
			else
				element.SetVariable("Value", " value=\"" + this._value + "\"");

			if (string.IsNullOrEmpty(this._className))
				element.DeleteVariable("ClassName");
			else
				element.SetVariable("ClassName", " className=\"" + this._className + "\"");

			if (string.IsNullOrEmpty(this._innerHtml))
				element.SetVariable("InnerHtml", " />");
			else
				element.SetVariable("InnerHtml", "><![CDATA[" + this._innerHtml + "]]></Element>");

			return element.ToString();
		}
	}

}
