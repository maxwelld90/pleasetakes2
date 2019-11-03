using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Ajax {

	internal static class Alerts {

		public static Elements.Alerts.Alert NoSchoolExpiration() {
			Elements.Alerts.Alert noSchoolExpiration = new Elements.Alerts.Alert("AjaxNoSchoolExpiration");
			noSchoolExpiration.Colour = Elements.Alerts.Colours.Red;
			noSchoolExpiration.Message = new Constructor("/Alerts/Ajax/noschoolexpiration.html").ToString();
			noSchoolExpiration.NoScript = false;
			noSchoolExpiration.ShowCloseBox = false;
			noSchoolExpiration.StartHidden = false;

			return noSchoolExpiration;
		}

		public static Elements.Alerts.Alert Expiration() {
			Elements.Alerts.Alert expiration = new Elements.Alerts.Alert("AjaxExpiration");
			expiration.Colour = Elements.Alerts.Colours.Red;
			expiration.Message = new Constructor("/Alerts/Ajax/expiration.html").ToString();
			expiration.NoScript = false;
			expiration.ShowCloseBox = false;
			expiration.StartHidden = false;

			return expiration;
		}

	}

}