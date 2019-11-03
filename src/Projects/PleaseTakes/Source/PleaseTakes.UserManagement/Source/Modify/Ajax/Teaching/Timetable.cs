using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.UserManagement.Modify.Ajax.Teaching {

	internal sealed class Timetable : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _staffId;
		private int _weekNo;

		public Timetable(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			if (this.GetStaffId()) {
				try {
					if (!this.GetWeekNo())
						this._weekNo = 1;

					Core.Helpers.Elements.DataGrids.Timetables.StaffTimetable staffTimetable = new Core.Helpers.Elements.DataGrids.Timetables.StaffTimetable(this._staffId);
					staffTimetable.IsCurrentSession = true;
					staffTimetable.WeekNo = this._weekNo;

					staffTimetable.TopControls.Left.OnClick = "getUpdatedResponse('Timetable', '?path=/staff/modify/ajax/teaching/timetable/" + this._staffId + "/WeekNo/');";
					staffTimetable.TopControls.Right.OnClick = "getUpdatedResponse('Timetable', '?path=/staff/modify/ajax/teaching/timetable/" + this._staffId + "/WeekNo/');";

					staffTimetable.Events.Unavailable.Href = "/staff/modify/teaching/timetable/StaffId/WeekNo/DayNo/Period/#Unavailable";
					staffTimetable.Events.Free.Href = "/staff/modify/teaching/timetable/StaffId/WeekNo/DayNo/Period/#Free";
					staffTimetable.Events.BusyOther.Href = "/staff/modify/teaching/timetable/StaffId/WeekNo/DayNo/Period/#Busy";
					staffTimetable.Events.BusyTeaching.Href = "/staff/modify/teaching/timetable/StaffId/WeekNo/DayNo/Period/#Teaching";

					this.Page.Contents = staffTimetable.ToString();
				}
				catch (Core.Helpers.Elements.DataGrids.Timetables.InvalidStaffIdException) {
					Core.Helpers.Elements.Alerts.Alert unknownStaffMemberAlert = new Core.Helpers.Elements.Alerts.Alert("UnknownStaffMember");
					unknownStaffMemberAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
					unknownStaffMemberAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Timetable/unknown.html").ToString();
					unknownStaffMemberAlert.NoScript = false;
					unknownStaffMemberAlert.ShowCloseBox = false;
					unknownStaffMemberAlert.StartHidden = false;

					this.Page.Contents = unknownStaffMemberAlert.ToString();
				}
				catch (Core.Helpers.Elements.DataGrids.Timetables.NoTimetableException) {
					Core.Helpers.Constructor noTimetableConstructor = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Timetable/notimetable.html");
					noTimetableConstructor.SetVariable("StaffId", this._staffId.ToString());
					
					Core.Helpers.Elements.Alerts.Alert noTimetableAlert = new Core.Helpers.Elements.Alerts.Alert("NoTimetable");
					noTimetableAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
					noTimetableAlert.Message = noTimetableConstructor.ToString();
					noTimetableAlert.NoScript = false;
					noTimetableAlert.ShowCloseBox = false;
					noTimetableAlert.StartHidden = false;

					this.Page.Contents = noTimetableAlert.ToString();
				}
			}
			else {
				Core.Helpers.Elements.Alerts.Alert invalidInput = new Core.Helpers.Elements.Alerts.Alert("InvalidInput");
				invalidInput.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
				invalidInput.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Timetable/invalidinput.html").ToString();
				invalidInput.NoScript = false;
				invalidInput.ShowCloseBox = false;
				invalidInput.StartHidden = false;

				this.Page.Contents = invalidInput.ToString();
			}
		}

		private bool GetStaffId() {
			if (this.Path.HasNext())
				return int.TryParse(this.Path.Next(), out this._staffId);

			return false;
		}

		private bool GetWeekNo() {
			if (this.Path.HasNext()) {
				if (int.TryParse(this.Path.Next(), out this._weekNo)) {
					try {
						this._weekNo.RequireThat("Week Number").IsInRange(1, Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoWeeks);
						return true;
					}
					catch (IndexOutOfRangeException) {
						return this.GetSourceWeekNo();
					}
				}

				return this.GetSourceWeekNo();
			}

			return this.GetSourceWeekNo();
		}

		private bool GetSourceWeekNo() {
			if (this.SourcePath.IsEmpty)
				return false;
			else {
				for (int i = 0; i <= 3; i++) {
					if (this.SourcePath.HasNext())
						this.SourcePath.Next();
					else
						return false;
				}

				if (this.SourcePath.HasNext() && this.SourcePath.Next().Equals("week") && this.SourcePath.HasNext() && int.TryParse(this.SourcePath.Next(), out this._weekNo)) {
					try {
						this._weekNo.RequireThat("Week Number").IsInRange(1, Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoWeeks);
						return true;
					}
					catch (IndexOutOfRangeException) {
						return false;
					}
				}
				else

				return false;
			}
		}
	}

}