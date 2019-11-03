using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools.SettingsCollections.Application {

	internal sealed class SessionCollection : SchoolSettingsBase {
		private int _timeout;

		public SessionCollection(School school)
			: base(school, "/PleaseTakes.Schools/School[@id='" + school.SchoolID + "']/PleaseTakes.Application/Session") {
			if (this.Parser.AttributeExists(this.XPath, "timeout")) {
				int timeout = int.Parse(this.Parser.SelectNode(this.XPath).Attributes["timeout"].Value);
				Validation.Specific.Settings.Timeout(timeout);

				this._timeout = timeout;
			}
			else
				this._timeout = WebServer.PleaseTakes.Application.CurrentInstance.Defaults.Timeout;

			WebServer.Session.Timeout = this._timeout;
		}

		public int Timeout {
			get {
				return this._timeout;
			}
			set {
				if (value.Equals(0)) {
					if (this.Parser.AttributeExists(this.XPath, "timeout")) {
						this.Parser.RemoveAttribute(this.XPath, "timeout");
						this.Parser.Save();
					}

					this._timeout = WebServer.PleaseTakes.Application.CurrentInstance.Defaults.Timeout;
				}
				else {
					Validation.Specific.Settings.Timeout(value);

					if (!this.Parser.AttributeExists(this.XPath, "timeout")) {
						this.Parser.CreateAttribute(this.XPath, "timeout", value.ToString());
						this.Parser.Save();
					}
					else
						if (!int.Parse(this.Parser.SelectNode(this.XPath).Attributes["timeout"].Value).Equals(value)) {
							this.Parser.SelectNode(this.XPath).Attributes["timeout"].Value = value.ToString();
							this.Parser.Save();
						}

					this._timeout = value;
				}

				WebServer.Session.Timeout = this._timeout;
			}
		}
	}

}
