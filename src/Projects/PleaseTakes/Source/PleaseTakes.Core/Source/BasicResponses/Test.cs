using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses {

	internal sealed class Test {

		public Test(Helpers.Path.Parser path) {

			Helpers.Elements.CoverSlips.Slip s = new PleaseTakes.Core.Helpers.Elements.CoverSlips.Slip();
			s.CoverDate = new DateTime(2010, 08, 16);
			s.PageBreakBefore = true;
			s.CoverStaffDetails.Forename = "Graeme";
			s.CoverStaffDetails.Surname = "Leitch";

			s.Period = 9;


			WebServer.Response.Write(s);
		}

	}
}