using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Slips.Ajax.Landing {

	internal sealed class IsValid : Core.Helpers.BaseHandlers.AjaxHandler {
		private DateTime _selectedDate;

		public IsValid(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.SetContentType("text/xml");
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			Core.Helpers.Path.Parser sourcePath = new Core.Helpers.Path.Parser(Core.WebServer.Request["sourcepath"]);
			Core.Helpers.Elements.Ajax.Xml.Collection xmlCollection = new Core.Helpers.Elements.Ajax.Xml.Collection();
			Core.Helpers.Elements.Ajax.Xml.Element xmlElement = new Core.Helpers.Elements.Ajax.Xml.Element();

			if (!sourcePath.IsEmpty)
				if (this.CheckInputValidity(sourcePath))
					xmlElement.Value = this._selectedDate.Year + "/" + this._selectedDate.Month + "/" + this._selectedDate.Day + "/";

			xmlCollection.Add(xmlElement);
			this.Page.Contents = xmlCollection.ToString();
		}

		private bool CheckInputValidity(Core.Helpers.Path.Parser path) {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

			for (int i = 0; i <= 1; i++) {
				if (path.HasNext())
					path.Next();
				else
					return false;
			}

			for (int i = 0; i <= 2; i++) {
				if (path.HasNext())
					switch (i) {
						case 0:
							validYear = int.TryParse(path.Next(), out year);
							break;
						case 1:
							validMonth = int.TryParse(path.Next(), out month);
							break;
						case 2:
							validDay = int.TryParse(path.Next(), out day);
							break;
					}
				else
					return false;
			}

			if ((!validYear) || (!validMonth) || (!validDay))
				return false;
			else {
				try {
					year.RequireThat("year").IsInRange(2000, 3000);
					month.RequireThat("month").IsInRange(1, 12);
					day.RequireThat("day").IsInRange(1, DateTime.DaysInMonth(year, month));

					if (IsValid.IsDateInSessionRange(year, month, day)) {
						DateTime selectedDate = new DateTime(year, month, day);
						int timetableWeek = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(selectedDate).TimetableWeek;

						try {
							int periods = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[timetableWeek][(int)selectedDate.DayOfWeek + 1];

							if (periods.Equals(0))
								return false;
							else {
								this._selectedDate = selectedDate;
								return true;
							}
						}
						catch (Core.Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) {
							return false;
						}
					}
					else
						return false;
				}
				catch (IndexOutOfRangeException) {
					return false;
				}
			}
		}

		private static bool IsDateInSessionRange(int year, int month, int day) {
			DateTime date = new DateTime(year, month, day);
			DateTime low = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.StartDate;
			DateTime high = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.EndDate;

			if (Core.Utils.IsDateInRange(low, high, date))
				return true;

			return false;
		}
	}

}