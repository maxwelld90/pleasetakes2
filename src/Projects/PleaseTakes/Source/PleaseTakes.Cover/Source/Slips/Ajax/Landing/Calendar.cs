using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Slips.Ajax.Landing {

	internal sealed class Calendar : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _year;
		private int _month;
		private bool _validAjaxDate;
		
		public Calendar(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			Core.Helpers.Elements.DataGrids.Calendar.Calendar calendar = new Core.Helpers.Elements.DataGrids.Calendar.Calendar();
			calendar.Month = 8;
			calendar.Year = 2010;

			calendar.TopControls.Left.Value = "MonthName, Year";
			calendar.TopControls.Centre.Value = "MonthName, Year";
			calendar.TopControls.Right.Value = "MonthName, Year";

			calendar.TopControls.Left.OnClick = "getUpdatedResponse('Calendar', '?path=/cover/slips/ajax/calendar/Year/Month/');";
			calendar.TopControls.Right.OnClick = "getUpdatedResponse('Calendar', '?path=/cover/slips/ajax/calendar/Year/Month/');";

			calendar.Events.Past.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.Past.Colour = Core.Helpers.Elements.DataGrids.CellColours.Red;
			calendar.Events.Past.Value = "Day";

			calendar.Events.NoDay.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.NoDay.Colour = Core.Helpers.Elements.DataGrids.CellColours.Red;
			calendar.Events.NoDay.Value = "Day";

			calendar.Events.TodayInRange.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.TodayInRange.Colour = Core.Helpers.Elements.DataGrids.CellColours.Yellow;
			calendar.Events.TodayInRange.Value = "Day";
			calendar.Events.TodayInRange.OnClick = "setRequestsToAjaxDate(Year, Month, Day)";

			calendar.Events.InRange.Colour = Core.Helpers.Elements.DataGrids.CellColours.Green;
			calendar.Events.InRange.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.InRange.Value = "Day";
			calendar.Events.InRange.OnClick = "setRequestsToAjaxDate(Year, Month, Day);";

			calendar.Events.OutOfRange.Type = Core.Helpers.Elements.DataGrids.CellTypes.Standard;
			calendar.Events.OutOfRange.Colour = Core.Helpers.Elements.DataGrids.CellColours.Red;
			calendar.Events.OutOfRange.Value = "Day";
			
			this.Page.Contents = Core.WebServer.Request["sourcePath"] + calendar.ToString();
		}



	}

}
