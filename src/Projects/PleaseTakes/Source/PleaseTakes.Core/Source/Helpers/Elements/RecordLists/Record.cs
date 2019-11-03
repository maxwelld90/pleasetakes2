using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PleaseTakes.Core.Helpers.Elements.RecordLists {

	internal sealed class Record {
		private string _id;
		private string _href;
		private string _onClick;
		private Colours _colour;
		private Side _leftSide;
		private Side _rightSide;

		public Record() {
			this._leftSide = new Side(true);
			this._rightSide = new Side(false);
		}

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string Href {
			get {
				return this._href;
			}
			set {
				this._href = value;
			}
		}

		public string OnClick {
			get {
				return this._onClick;
			}
			set {
				this._onClick = value;
			}
		}

		public Colours Colour {
			get {
				return this._colour;
			}
			set {
				this._colour = value;
			}
		}

		public Side LeftSide {
			get {
				return this._leftSide;
			}
		}

		public Side RightSide {
			get {
				return this._rightSide;
			}
		}

		public string InnerHtml {
			get {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(this.ToString());

				XmlNode node = xmlDocument.SelectSingleNode("/li/a");

				return node.InnerXml;
			}
		}

		public override string ToString() {
			Constructor record = new Constructor("/Templates/Elements/Recordlists/record.html");
			string classes = "";

			if (string.IsNullOrEmpty(this._id))
				record.DeleteVariable("Id");
			else
				record.SetVariable("Id", " id=\"{$CollectionId}" + this._id + "\"");

			if ((string.IsNullOrEmpty(this._href)) && (string.IsNullOrEmpty(this._onClick))) {
				record.SetVariable("Href", " href=\"javascript:void(0);\"");
				record.DeleteVariable("OnClick");
				classes = "Nowhere ";
			}
			else {
				if (string.IsNullOrEmpty(this._href))
					record.SetVariable("Href", " href=\"javascript:void(0);\"");
				else
					record.SetVariable("Href", " href=\"" + this._href + "\"");

				if (string.IsNullOrEmpty(this._onClick))
					record.DeleteVariable("OnClick");
				else
					record.SetVariable("OnClick", " onclick=\"" + this._onClick + "\"");
			}

			switch (this._colour) {
				case Colours.Red:
					classes += "Red";
					break;
				case Colours.Yellow:
					classes += "Yellow";
					break;
				case Colours.Green:
					classes += "Green";
					break;
			}

			if (string.IsNullOrEmpty(classes))
				record.DeleteVariable("Classes");
			else {
				classes = classes.TrimEnd(' ');
				record.SetVariable("Classes", " class=\"" + classes + "\"");
			}

			if (this._leftSide.IsEmpty)
				record.DeleteVariable("LeftSide");
			else
				record.SetVariable("LeftSide", this._leftSide.ToString());

			if (this._rightSide.IsEmpty)
				record.DeleteVariable("RightSide");
			else
				record.SetVariable("RightSide", this._rightSide.ToString());

			return record.ToString();
		}

	}

}
