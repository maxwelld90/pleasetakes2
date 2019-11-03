using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Ajax {

	internal sealed class Element {
		private string _id;
		private string _url;
		private bool _showLoadingMessage;
		private bool _getPath;

		public Element(string id) {
			this._id = id;
		}

		public string Id {
			get {
				return this._id;
			}
		}

		public string Url {
			get {
				return this._url;
			}
			set {
				this._url = value;
			}
		}

		public bool ShowLoadingMessage {
			get {
				return this._showLoadingMessage;
			}
			set {
				this._showLoadingMessage = value;
			}
		}

		public bool GetPath {
			get {
				return this._getPath;
			}
			set {
				this._getPath = value;
			}
		}

		public override string ToString() {
			Constructor ajaxElement = new Constructor("/Templates/Elements/Ajax/element.html");
			ajaxElement.SetVariable("Id", this._id);
			ajaxElement.SetVariable("Url", "?path=" + this._url);
			ajaxElement.SetVariable("ShowLoading", this._showLoadingMessage.ToString().ToLower());
			ajaxElement.SetVariable("GetPath", this._getPath.ToString().ToLower());

			return ajaxElement.ToString();
		}
	}

}