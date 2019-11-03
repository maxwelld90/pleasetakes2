using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.BaseHandlers {

	internal sealed class QuoteHandler : BaseHandler {

		public QuoteHandler()
			: base(TemplateTypes.Quote, null) {
			this.SetQuote();
			this.Output.Send();
		}

		private void SetQuote() {
			List<Quote> list = new List<Quote>();

			list.Add(new Quote("Title", "Body", "Author", "Extra"));

			Quote random = list[Utils.RandomNumber(0, (list.Count - 1))];

			this.Page.SetVariable("Title", random.Title);
			this.Page.SetVariable("Body", random.Body);
			this.Page.SetVariable("Author", random.Author);

			if (string.IsNullOrEmpty(random.Extra))
				this.Page.DeleteVariable("Extra");
			else
				this.Page.SetVariable("Extra", "<span>" + random.Extra + "</span>");
		}

		private sealed class Quote {
			private string _title;
			private string _body;
			private string _author;
			private string _extra;

			public Quote(string title, string body, string author, string extra) {
				this._title = title;
				this._body = body;
				this._author = author;
				this._extra = extra;
			}

			public string Title {
				get {
					return this._title;
				}
				set {
					this._title = value;
				}
			}

			public string Body {
				get {
					return this._body;
				}
				set {
					this._body = value;
				}
			}

			public string Author {
				get {
					return this._author;
				}
				set {
					this._author = value;
				}
			}

			public string Extra {
				get {
					return this._extra;
				}
				set {
					this._extra = value;
				}
			}

		}
	}

}