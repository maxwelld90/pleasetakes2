using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Calendar.Ajax {

	internal sealed class Standard : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _year;
		private int _month;
		private bool _validAjaxDate;

		public Standard(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			this.SetYearAndMonth();

			Core.Helpers.Elements.DataGrids.Calendar.Calendar calendar = new Core.Helpers.Elements.DataGrids.Calendar.Calendar();
			calendar.Year = this._year;
			calendar.Month = this._month;

			calendar.Events.Past.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.Past.Colour = Core.Helpers.Elements.DataGrids.CellColours.Red;
			calendar.Events.Past.Value = "Day";

			calendar.Events.NoDay.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.NoDay.Colour = Core.Helpers.Elements.DataGrids.CellColours.Red;
			calendar.Events.NoDay.Value = "Day";

			calendar.Events.TodayInRange.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.TodayInRange.Colour = Core.Helpers.Elements.DataGrids.CellColours.Yellow;
			calendar.Events.TodayInRange.Value = "Day";
			calendar.Events.TodayInRange.Href = "/cover/arrange/attendance/Year/Month/Day/";

			calendar.Events.InRange.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.InRange.Colour = Core.Helpers.Elements.DataGrids.CellColours.Green;
			calendar.Events.InRange.Value = "Day";
			calendar.Events.InRange.Href = "/cover/arrange/attendance/Year/Month/Day/";

			calendar.Events.OutOfRange.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.OutOfRange.Colour = Core.Helpers.Elements.DataGrids.CellColours.Red;
			calendar.Events.OutOfRange.Value = "Day";

			if (this.Path.HasNext()) {
				Core.Helpers.Elements.Alerts.Alert alert = new Core.Helpers.Elements.Alerts.Alert("");
				switch (this.Path.Next()) {
					case "invalid":
						alert.Id = "InvalidDetails";
						alert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
						alert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Calendar/invalid.html").ToString();
						alert.NoScript = false;
						alert.StartHidden = false;
						alert.ShowCloseBox = true;

						this.Page.Contents = alert.ToString();
						break;
					case "outofrange":
						alert.Id = "OutOfRange";
						alert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
						alert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Calendar/outofrange.html").ToString();
						alert.NoScript = false;
						alert.StartHidden = false;
						alert.ShowCloseBox = true;

						this.Page.Contents = alert.ToString();
						break;
					case "noperiods":
						alert.Id = "NoPeriods";
						alert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
						alert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Calendar/noperiods.html").ToString();
						alert.NoScript = false;
						alert.StartHidden = false;
						alert.ShowCloseBox = true;

						this.Page.Contents = alert.ToString();
						break;
				}
			}

			calendar.TopControls.Left.Value = "MonthName, Year";
			calendar.TopControls.Centre.Value = "MonthName, Year";
			calendar.TopControls.Right.Value = "MonthName, Year";

			calendar.TopControls.Left.OnClick = "getUpdatedResponse('Calendar', '?path=/cover/arrange/calendar/ajax/Year/Month/ignore/');";
			calendar.TopControls.Right.OnClick = "getUpdatedResponse('Calendar', '?path=/cover/arrange/calendar/ajax/Year/Month/ignore/');";

			this.Page.Contents += calendar.ToString();
		}

		private void SetYearAndMonth() {
			this._year = DateTime.Now.Year;
			this._month = DateTime.Now.Month;

			if (this.CanAdvance(true))
				this.SetYearAndMonth(true);

			if (!this.Path.HasNext())
				if (this.CanAdvance(false))
					this.SetYearAndMonth(false);
		}

		private bool CanAdvance(bool useAjaxPath) {
			if (useAjaxPath) {
				return this.Path.HasNext();
			}
			else {
				if (this.SourcePath.IsEmpty)
					return false;
				else {
					for (int i = 0; i <= 2; i++)
						if (this.SourcePath.HasNext())
							this.SourcePath.Next();
						else
							return false;

					if (this.SourcePath.HasNext())
						return true;
					else
						return false;
				}
			}
		}

		private void SetTo(int year, int month) {
			this._year = year;
			this._month = month;
		}

		private static bool IsDateInSessionRange(int year, int month) {
			DateTime date = new DateTime(year, month, 1);
			DateTime low = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.StartDateMonth.AddMonths(-1);
			DateTime high = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.EndDateMonth.AddMonths(1);

			if (Core.Utils.IsDateInRange(low, high, date))
				return true;

			return false;
		}

		private void SetYearAndMonth(bool useAjaxPath) {
			bool validYearInt = false;
			bool validMonthInt = false;
			int year = 0;
			int month = 0;

			if (useAjaxPath)
				this.TryRead(this.Path, ref validYearInt, ref validMonthInt, ref year, ref month);
			else
				this.TryRead(this.SourcePath, ref validYearInt, ref validMonthInt, ref year, ref month);

			if ((!validYearInt) || (!validMonthInt)) {
				if ((!useAjaxPath) && (!this._validAjaxDate))
					Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/ajax/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/invalid/");
			}
			else {
				try {
					year.RequireThat("year").IsInRange(2000, 3000);
					month.RequireThat("month").IsInRange(1, 12);

					if (Standard.IsDateInSessionRange(year, month)) {
						if (useAjaxPath) {
							this._validAjaxDate = true;
							this.SetTo(year, month);
						}
						else {
							if (!this._validAjaxDate)
								this.SetTo(year, month);
						}
					}
					else
						Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/ajax/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/outofrange/");
				}
				catch (IndexOutOfRangeException) {
					Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/ajax/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/invalid/");
				}
			}
		}

		private void TryRead(Core.Helpers.Path.Parser path, ref bool validYearInt, ref bool validMonthInt, ref int year, ref int month) {
			if (path.Peek().Equals("outofrange"))
				Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/ajax/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/outofrange/");
			else if (path.Peek().Equals("noperiods"))
				Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/ajax/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/noperiods/");
			else
				validYearInt = int.TryParse(path.Next(), out year);

			if (path.HasNext())
				validMonthInt = int.TryParse(path.Next(), out month);
		}
	}

}