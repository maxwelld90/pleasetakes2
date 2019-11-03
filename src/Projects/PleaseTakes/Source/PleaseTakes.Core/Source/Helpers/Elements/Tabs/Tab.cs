using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Tabs {

	internal sealed class Tab {
		private string _id;
		private string _title;
		private Constructor _content;
		private Alerts.Collection _alerts;
		private Breadcrumbs.Trail _breadcrumbTrail;
		private Menu90.Collection _menu90;

		public Tab(string id, string title, string templatePath) {
			this._id = id;
			this._title = title;
			this._content = new Constructor(templatePath);
			this._alerts = new Alerts.Collection();
			this._breadcrumbTrail = new Breadcrumbs.Trail();
			this._menu90 = new Menu90.Collection();
		}

		public string Id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}

		public string Title {
			get {
				return this._title;
			}
			set {
				this._title = value;
			}
		}

		public Constructor Content {
			get {
				return this._content;
			}
		}

		public Alerts.Collection Alerts {
			get {
				return this._alerts;
			}
		}

		public Breadcrumbs.Trail BreadcrumbTrail {
			get {
				return this._breadcrumbTrail;
			}
		}

		public Menu90.Collection Menu90 {
			get {
				return this._menu90;
			}
		}

		public override string ToString() {
			return this._content.ToString();
		}
	}

}