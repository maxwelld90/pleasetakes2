using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.HeaderTags {

	internal abstract partial class Tag {
		private string _tagType;
		private string _innerText;
		private bool _closesSelf;
		private Conditionals _conditional;
		private List<Attribute> _attributes;

		public Tag(string tagType) {
			this._tagType = tagType;
			this._attributes = new List<Attribute>();
		}

		protected void CreateAttribute(string name) {
			this.CreateAttribute(name, null);
		}

		protected void CreateAttribute(string name, string value) {
			Attribute newAttribute = new Attribute();
			newAttribute.Name = name;

			if (value != null)
				newAttribute.Value = value;

			this._attributes.Add(newAttribute);
		}

		protected string this[string attributeName] {
			get {
				return (this._attributes.Single(a => a.Name.Equals(attributeName)).Value);
			}
			set {
				this._attributes.Single(a => a.Name.Equals(attributeName)).Value = value;
			}
		}

		protected bool ClosesSelf {
			get {
				return this._closesSelf;
			}
			set {
				this._closesSelf = value;
			}
		}

		public Conditionals Conditional {
			get {
				return this._conditional;
			}
			set {
				this._conditional = value;
			}
		}

		protected string InnerText {
			get {
				return this._innerText;
			}
			set {
				this._innerText = value;
			}
		}

		public override string ToString() {
			string returnStr = "";

			switch (this._conditional) {
				case Conditionals.Ie:
					returnStr = "<!--[if IE]>";
					break;
				case Conditionals.LteIe6:
					returnStr = "<!--[if lte IE 6]>";
					break;
				case Conditionals.GteIe7:
					returnStr = "<!--[if gte IE 7]>";
					break;
			}

			returnStr += "<" + this._tagType + " ";

			if (this._attributes.Count > 0)
				foreach (Attribute a in this._attributes)
					returnStr += a + " ";

			returnStr = returnStr.TrimEnd(' ');

			if (this._closesSelf)
				returnStr += " />";
			else
				if (string.IsNullOrEmpty(this._innerText))
					returnStr += "></" + this._tagType + ">";
				else
					returnStr += ">" + this._innerText + "</" + this._tagType + ">";

			if (this._conditional != Conditionals.None)
				returnStr += "<![endif]-->";

			return returnStr;
		}
	}

}