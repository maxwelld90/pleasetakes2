using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.UserManagement.Modify.Teaching {

	internal sealed partial class Timetable : Core.Helpers.BaseHandlers.StandardHandler {
		private int _staffId;
		private int _weekNo;
		private int _dayNo;
		private int _period;
		private StaffNameFormatter _name;
		private TimetableStates _state;
		private bool _isUnique;
		private int? _yeargroupId;
		private string _yeargroupName;
		private int? _roomId;
		private string _roomName;
		private string _commitmentName;
		private int? _subjectId;
		private string _subjectName;
		private string _qualificationName;
		private int? _subjectQualificationId;
		private string _className;
		private int? _classId;
		private string _alert;

		public Timetable(Core.Helpers.Path.Parser path)
			: base(path, true, true, true, false, true) {
			this.Output.Send();
		}

		protected override void InitialChecks() {
			if (this.HasStaffId() && this.HasWeekNo() && this.HasDayNo() && this.HasPeriod()) {
				this.HasAlertMessage();
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
				paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._weekNo);
				paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", this._dayNo);
				paramBuilder.AddParameter(SqlDbType.Int, "@Period", this._period);

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Teaching/period.sql", paramBuilder.Parameters)) {
					dataReader.Read();

					if (((int)dataReader["Status"]).Equals(1)) {
						this.PopulateName(dataReader);

						switch ((int)dataReader["PeriodStatus"]) {
							case 0:
								this._state = TimetableStates.Unavailable;
								break;
							case 1:
								this._state = TimetableStates.Free;
								break;
							case 2:
								if (((int)dataReader["PeriodType"]).Equals(2))
									this._state = TimetableStates.Busy;
								else {
									this._state = TimetableStates.Teaching;
									this._isUnique = (bool)dataReader["IsUnique"];
									this._yeargroupId = dataReader["YeargroupId"] as int?;
									this._yeargroupName = dataReader["YeargroupName"] as string;
									this._roomId = dataReader["RoomId"] as int?;
									this._roomName = dataReader["RoomName"] as string;
									this._subjectId = dataReader["SubjectId"] as int?;
									this._subjectName = dataReader["SubjectName"] as string;
									this._qualificationName = dataReader["QualificationName"] as string;
									this._subjectQualificationId = dataReader["SubjectQualificationId"] as int?;
									this._classId = dataReader["ClassId"] as int?;
									this._className = dataReader["ClassName"] as string;
								}

								this._commitmentName = dataReader["CommitmentName"] as string;
								break;
						}
					}
					else
						Core.WebServer.PleaseTakes.Redirect("/staff/inputteaching/#Teaching");
				}
			}
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/inputteaching/#Teaching");
		}

		protected override void SetAlerts() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				switch (tab.Id) {
					case "Unavailable":
						if (this._state.Equals(TimetableStates.Unavailable))
							tab.Alerts.Add("SelectedUnavailable", new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/current.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Green, false, true);

						tab.Content.SetVariable("Alerts", tab.Alerts.ToString());
						break;
					case "Free":
						if (this._state.Equals(TimetableStates.Free))
							tab.Alerts.Add("SelectedFree", new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/current.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Green, false, true);

						tab.Content.SetVariable("Alerts", tab.Alerts.ToString());
						break;
					case "Teaching":
						if (this._state.Equals(TimetableStates.Teaching))
							tab.Alerts.Add("SelectedTeaching", new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/current.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Green, false, true);

						if (!string.IsNullOrEmpty(this._alert) && this._alert.Equals("teachingfail"))
							tab.Alerts.Add("TeachingFail", new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/teachingfail.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Red, false, false);

						tab.Content.SetVariable("Alerts", tab.Alerts.ToString());
						break;
					case "Busy":
						if (!string.IsNullOrEmpty(this._alert) && this._alert.Equals("busyfailed"))
							tab.Alerts.Add("FailedBusy", new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/busyfailed.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Red, false, true);
						
						if (this._state.Equals(TimetableStates.Busy))
							tab.Alerts.Add("SelectedBusy", new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/current.html").ToString(), Core.Helpers.Elements.Alerts.Colours.Green, false, true);

						break;
				}
			}
		}

		protected override void SetBreadcrumbTrails() {
			this._name.DisplayForenameFirst = true;

			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				tab.BreadcrumbTrail.Add("Home", "?path=/home/", null);
				tab.BreadcrumbTrail.Add("Staff Management", "?path=/staff/", null);
				tab.BreadcrumbTrail.Add("Teaching Staff", "?path=/staff/#Teaching", null);
				tab.BreadcrumbTrail.Add("<strong>" + this._name.ToString() + "</strong>", "?path=/staff/modify/teaching/" + this._staffId + "/", null);
				tab.BreadcrumbTrail.Add("Timetable", "?path=/staff/modify/teaching/" + this._staffId + "/week/" + this._weekNo + "/#Timetable", null);
				tab.BreadcrumbTrail.Add("Modify");

				switch (tab.Id) {
					case "Unavailable":
						tab.BreadcrumbTrail.Add("<strong>Unavailable</strong>");
						break;
					case "Free":
						tab.BreadcrumbTrail.Add("<strong>Free</strong>");
						break;
					case "Teaching":
						tab.BreadcrumbTrail.Add("<strong>Teaching</strong>");
						break;
					case "Selection":
						tab.BreadcrumbTrail.Add("<strong>Selection</strong>");
						break;
					case "Busy":
						tab.BreadcrumbTrail.Add("<strong>Busy</strong>");
						break;
				}

				tab.Content.SetVariable("BreadcrumbTrail", tab.BreadcrumbTrail.ToString());
			}
		}

		protected override void SetHeaderTags() {
			Core.Helpers.Elements.HeaderTags.Script modify = new Core.Helpers.Elements.HeaderTags.Script();
			modify.Conditional = Core.Helpers.Elements.HeaderTags.Conditionals.None;
			modify.Source = "?path=/resources/javascript/specific/staff/modify/teaching/timetable/modify.js";

			this.HeaderTags.Add(modify);
		}

		protected override void SetMenu90() {

		}

		protected override void SetTabs() {
			this.Tabs.Add("Unavailable", "Unavailable", "/templates/specific/staff/modify/teaching/timetable/unavailable.html");
			this.Tabs.Add("Free", "Free", "/templates/specific/staff/modify/teaching/timetable/free.html");
			this.Tabs.Add("Teaching", "Teaching", "/templates/specific/staff/modify/teaching/timetable/teaching.html");
			this.Tabs.Add("Selection", "Selection", "/templates/specific/staff/modify/teaching/timetable/selection.html");
			this.Tabs.Add("Busy", "Busy", "/templates/specific/staff/modify/teaching/timetable/busy.html");
		}

		protected override void SetTabSpecific() {
			foreach (Core.Helpers.Elements.Tabs.Tab tab in this.Tabs) {
				Core.Helpers.Elements.MenuBar.Bar menuBar = new Core.Helpers.Elements.MenuBar.Bar();
				menuBar.Text = "Rotation week <strong>" + this._weekNo + "</strong>, <strong>" + ((DayOfWeek)(this._dayNo - 1)) + "</strong>, period <strong>" + this._period + "</strong>";
				menuBar.AddButton("back.png", "Click here to jump back to the staff member's timetable.", "?path=/staff/modify/teaching/" + this._staffId + "/week/" + this._weekNo + "/#Timetable", null);

				switch (tab.Id) {
					case "Unavailable":
						if (this._state.Equals(TimetableStates.Unavailable))
							tab.Content.SetVariable("UnavailableForm", new Core.Helpers.Constructor("/Templates/Specific/Staff/Modify/Teaching/Timetable/current.html").ToString());
						else {
							Core.Helpers.Elements.Forms.BasicForm unavailableForm = new Core.Helpers.Elements.Forms.BasicForm();
							unavailableForm.Id = "Unavailable";
							unavailableForm.HasTopSpace = true;
							unavailableForm.PostUrl = "/staff/modify/update/teaching/timetable/";
							unavailableForm.AddButton(null, "Switch", null, 1, Core.Helpers.Elements.Forms.ButtonTypes.Submit);

							unavailableForm.AddHiddenField("State", null, "Unavailable");
							unavailableForm.AddHiddenField("StaffId", null, this._staffId.ToString());
							unavailableForm.AddHiddenField("WeekNo", null, this._weekNo.ToString());
							unavailableForm.AddHiddenField("DayNo", null, this._dayNo.ToString());
							unavailableForm.AddHiddenField("Period", null, this._period.ToString());

							tab.Content.SetVariable("UnavailableForm", unavailableForm.ToString());
						}

						menuBar.AddButton("thumbsup.png", "Click here to switch to the 'Free' tab.", "#Free", "switchToTab('Free');");
						menuBar.AddButton("teach.png", "Click here to switch to the 'Teaching' tab.", "#Teaching", "switchToTab('Teaching');");
						menuBar.AddButton("job.png", "Click here to switch to the 'Busy' tab.", "#Busy", "switchToTab('Busy');");
						tab.Content.SetVariable("MenuBar", menuBar.ToString());
						break;
					case "Free":
						if (this._state.Equals(TimetableStates.Free))
							tab.Content.SetVariable("FreeForm", new Core.Helpers.Constructor("/Templates/Specific/Staff/Modify/Teaching/Timetable/current.html").ToString());
						else {
							Core.Helpers.Elements.Forms.BasicForm freeForm = new Core.Helpers.Elements.Forms.BasicForm();
							freeForm.Id = "Free";
							freeForm.HasTopSpace = true;
							freeForm.PostUrl = "/staff/modify/update/teaching/timetable/";
							freeForm.AddButton(null, "Switch", null, 1, Core.Helpers.Elements.Forms.ButtonTypes.Submit);

							freeForm.AddHiddenField("State", null, "Free");
							freeForm.AddHiddenField("StaffId", null, this._staffId.ToString());
							freeForm.AddHiddenField("WeekNo", null, this._weekNo.ToString());
							freeForm.AddHiddenField("DayNo", null, this._dayNo.ToString());
							freeForm.AddHiddenField("Period", null, this._period.ToString());

							tab.Content.SetVariable("FreeForm", freeForm.ToString());
						}

						menuBar.AddButton("noentry.png", "Click here to switch to the 'Unavailable' tab.", "#Unavailable", "switchToTab('Unavailable');");
						menuBar.AddButton("teach.png", "Click here to switch to the 'Teaching' tab.", "#Teaching", "switchToTab('Teaching');");
						menuBar.AddButton("job.png", "Click here to switch to the 'Busy' tab.", "#Busy", "switchToTab('Busy');");
						tab.Content.SetVariable("MenuBar", menuBar.ToString());
						break;
					case "Teaching":
						Core.Helpers.Elements.Forms.GroupedForm teachingForm = new Core.Helpers.Elements.Forms.GroupedForm();
						teachingForm.Id = "Teaching";
						teachingForm.HasTopSpace = true;
						teachingForm.PostUrl = "/staff/modify/update/teaching/timetable/";
						teachingForm.RadioGroupName = "SelectedGroup";

						teachingForm.AddHiddenField("State", null, "Teaching");
						teachingForm.AddHiddenField("StaffId", null, this._staffId.ToString());
						teachingForm.AddHiddenField("WeekNo", null, this._weekNo.ToString());
						teachingForm.AddHiddenField("DayNo", null, this._dayNo.ToString());
						teachingForm.AddHiddenField("Period", null, this._period.ToString());

						Core.Helpers.Elements.Forms.Group uniqueGroup = teachingForm.AddGroup("Unique");
						uniqueGroup.TitleDetails.Image.Src = "teach.png";
						uniqueGroup.TitleDetails.Image.Tooltip = "A timetable entry unique to this staff member.";
						uniqueGroup.TitleDetails.Text = "<strong>A unique teaching entry</strong>";
						uniqueGroup.RadioValue = "Unique";

						if (this.Path.HasNext() && this.Path.Peek().Equals("uniquefail")) {
							Core.Helpers.Elements.Alerts.Alert uniqueFail = new Core.Helpers.Elements.Alerts.Alert("UniqueFail");
							uniqueFail.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
							uniqueFail.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/uniquefailed.html").ToString();

							uniqueGroup.RightPane.Contents = uniqueFail.ToString();
						}
						else
							uniqueGroup.RightPane.Contents = "&nbsp;";

						Core.Helpers.Elements.Forms.Group classGroup = teachingForm.AddGroup("Class");
						classGroup.TitleDetails.Image.Src = "teach.png";
						classGroup.TitleDetails.Image.Tooltip = "A class";
						classGroup.TitleDetails.Text = "<strong>A class</strong>";
						classGroup.RadioValue = "Class";

						if (this.Path.HasNext() && this.Path.Peek().Equals("classfail")) {
							Core.Helpers.Elements.Alerts.Alert classFail = new Core.Helpers.Elements.Alerts.Alert("ClassFail");
							classFail.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
							classFail.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/classfailed.html").ToString();

							classGroup.RightPane.Contents = classFail.ToString();
						}
						else
							classGroup.RightPane.Contents = "&nbsp;";

						this.CreateUniqueYeargroup(teachingForm, uniqueGroup);
						this.CreateUniqueRoom(teachingForm, uniqueGroup);
						this.CreateUniqueName(teachingForm, uniqueGroup);

						this.CreateClassSubject(teachingForm, classGroup);
						this.CreateClassQualification(teachingForm, classGroup);
						this.CreateClassClass(teachingForm, classGroup);
						this.CreateClassRoom(teachingForm, classGroup);

						if (this._state.Equals(TimetableStates.Teaching)) {
							if (this._isUnique) {
								teachingForm.OnReset = "setFormTo('Teaching', 'Unique');";
								uniqueGroup.IsDefault = true;
							}
							else {
								teachingForm.OnReset = "setFormTo('Teaching', 'Class');";
								classGroup.IsDefault = true;
							}

							teachingForm.AddButton(null, "Reset", "resetNonFields();", 3, Core.Helpers.Elements.Forms.ButtonTypes.Reset);
							teachingForm.AddButton(null, "Update", null, 2, Core.Helpers.Elements.Forms.ButtonTypes.Submit);
						}
						else {
							teachingForm.AddButton(null, "Switch", null, 2, Core.Helpers.Elements.Forms.ButtonTypes.Submit);
						}

						menuBar.AddButton("noentry.png", "Click here to switch to the 'Unavailable' tab.", "#Unavailable", "switchToTab('Unavailable');");
						menuBar.AddButton("thumbsup.png", "Click here to switch to the 'Free' tab.", "#Free", "switchToTab('Free');");
						menuBar.AddButton("job.png", "Click here to switch to the 'Busy' tab.", "#Busy", "switchToTab('Busy');");

						tab.Content.SetVariable("TeachingForm", teachingForm.ToString());
						tab.Content.SetVariable("MenuBar", menuBar.ToString());
						break;
					case "Selection":
						Core.Helpers.Elements.Search.SearchArea selectionSearch = new Core.Helpers.Elements.Search.SearchArea("Selection");
						selectionSearch.AddButton("search.png", null, "doSelectionSearch();", "Click here to search.");
						selectionSearch.AddButton("refresh.png", null, "resetSelectionSearch();", "Click here to reset the search criteria.");
						selectionSearch.AddButton("back.png", null, "switchToTab('Teaching');", "Click here to jump back to the 'Teaching' tab.");
						selectionSearch.AjaxUrl = "/staff/modify/ajax/teaching/timetable/selection/search/";
						selectionSearch.AjaxStatusUrl = "/staff/modify/ajax/teaching/timetable/selection/status/";

						tab.Content.SetVariable("SearchArea", selectionSearch.ToString());
						break;
					case "Busy":
						Core.Helpers.Elements.Forms.BasicForm busyForm = new Core.Helpers.Elements.Forms.BasicForm();
						busyForm.Id = "Busy";
						busyForm.HasTopSpace = true;
						busyForm.PostUrl = "/staff/modify/update/teaching/timetable/";

						busyForm.AddHiddenField("State", null, "Busy");
						busyForm.AddHiddenField("StaffId", null, this._staffId.ToString());
						busyForm.AddHiddenField("WeekNo", null, this._weekNo.ToString());
						busyForm.AddHiddenField("DayNo", null, this._dayNo.ToString());
						busyForm.AddHiddenField("Period", null, this._period.ToString());

						Core.Helpers.Elements.Forms.Row busyRow = busyForm.AddRow();
						busyRow.Description = "<strong>Commitment Name</strong>";

						if (tab.Alerts.IsEmpty)
							busyForm.RightPane.Contents = "&nbsp;";
						else
							busyForm.RightPane.Contents = tab.Alerts.ToString();

						if (this._state.Equals(TimetableStates.Busy)) {
							busyForm.AddButton(null, "Reset", null, 3, Core.Helpers.Elements.Forms.ButtonTypes.Reset);
							busyForm.AddButton(null, "Update", null, 2, Core.Helpers.Elements.Forms.ButtonTypes.Submit);
							busyRow.SetToTextField("BusyCommitment", null, this._commitmentName, 1, 0, false, false);
						}
						else {
							busyForm.AddButton(null, "Switch", null, 2, Core.Helpers.Elements.Forms.ButtonTypes.Submit);
							busyRow.SetToTextField("BusyCommitment", null, null, 1, 0, false, false);
						}

						tab.Content.SetVariable("BusyForm", busyForm.ToString());

						menuBar.AddButton("noentry.png", "Click here to switch to the 'Unavailable' tab.", "#Unavailable", "switchToTab('Unavailable');");
						menuBar.AddButton("thumbsup.png", "Click here to switch to the 'Free' tab.", "#Free", "switchToTab('Free');");
						menuBar.AddButton("teach.png", "Click here to switch to the 'Teaching' tab.", "#Teaching", "switchToTab('Teaching');");
						tab.Content.SetVariable("MenuBar", menuBar.ToString());
						break;
				}
			}
		}

		protected override void SpecificCommands() {

		}

		private void CreateUniqueYeargroup(Core.Helpers.Elements.Forms.Form form, Core.Helpers.Elements.Forms.Group uniqueGroup) {
			Core.Helpers.Elements.Forms.Row yeargroupRow = uniqueGroup.AddRow();
			TimetableText yeargroupText = new TimetableText();
			yeargroupRow.Description = "<strong>Yeargroup</strong>";

			if (this._state.Equals(TimetableStates.Teaching))
				if (this._isUnique) {
					yeargroupText.Highlighted = this._yeargroupName;
					form.AddHiddenField("UniqueYeargroupId", "UniqueYeargroupId", this._yeargroupId.ToString());

					form.AddHiddenField("UniqueYeargroupName-Original", "UniqueYeargroupName-Original", this._yeargroupName);
					form.AddHiddenField("UniqueYeargroupId-Original", "UniqueYeargroupId-Original", this._yeargroupId.ToString());
				}
				else {
					yeargroupText.Highlighted = "None";
					form.AddHiddenField("UniqueYeargroupId", "UniqueYeargroupId", null);

					form.AddHiddenField("UniqueYeargroupName-Original", "UniqueYeargroupName-Original", "None");
					form.AddHiddenField("UniqueYeargroupId-Original", "UniqueYeargroupId-Original", null);
				}
			else {
				yeargroupText.Highlighted = "None";
				form.AddHiddenField("UniqueYeargroupId", "UniqueYeargroupId", null);

				form.AddHiddenField("UniqueYeargroupName-Original", "UniqueYeargroupName-Original", "None");
				form.AddHiddenField("UniqueYeargroupId-Original", "UniqueYeargroupId-Original", null);
			}

			yeargroupText.HighlightedId = "UniqueYeargroupName";
			yeargroupText.OnClick = "selectUniqueYeargroup();";

			yeargroupRow.Text = yeargroupText.ToString();
		}

		private void CreateUniqueRoom(Core.Helpers.Elements.Forms.Form form, Core.Helpers.Elements.Forms.Group uniqueGroup) {
			Core.Helpers.Elements.Forms.Row roomRow = uniqueGroup.AddRow();
			TimetableText roomText = new TimetableText();
			roomRow.Description = "<strong>Room</strong>";

			if (this._state.Equals(TimetableStates.Teaching))
				if (this._isUnique) {
					if (string.IsNullOrEmpty(this._roomName)) {
						roomText.Highlighted = "None";
						form.AddHiddenField("UniqueRoomName-Original", "UniqueRoomName-Original", "None");
					}
					else {
						roomText.Highlighted = this._roomName;
						form.AddHiddenField("UniqueRoomName-Original", "UniqueRoomName-Original", this._roomName);
					}

					form.AddHiddenField("UniqueRoomId", "UniqueRoomId", this._roomId.ToString());
					form.AddHiddenField("UniqueRoomId-Original", "UniqueRoomId-Original", this._roomId.ToString());
				}
				else {
					roomText.Highlighted = "None";
					form.AddHiddenField("UniqueRoomId", "UniqueRoomId", null);

					form.AddHiddenField("UniqueRoomName-Original", "UniqueRoomName-Original", "None");
					form.AddHiddenField("UniqueRoomId-Original", "UniqueRoomId-Original", null);
				}
			else {
				roomText.Highlighted = "None";
				form.AddHiddenField("UniqueRoomId", "UniqueRoomId", null);

				form.AddHiddenField("UniqueRoomName-Original", "UniqueRoomName-Original", "None");
				form.AddHiddenField("UniqueRoomId-Original", "UniqueRoomId-Original", null);
			}

			roomText.HighlightedId = "UniqueRoomName";
			roomText.OnClick = "selectUniqueRoom();";

			roomRow.Text = roomText.ToString();
		}

		private void CreateUniqueName(Core.Helpers.Elements.Forms.Form form, Core.Helpers.Elements.Forms.Group uniqueGroup) {
			Core.Helpers.Elements.Forms.Row nameRow = uniqueGroup.AddRow();
			nameRow.Description = "<strong>Name</strong>";

			if (this._state.Equals(TimetableStates.Teaching))
				if (this._isUnique)
					nameRow.SetToTextField("UniqueName", null, this._commitmentName, 1, 0, false, false);
				else
					nameRow.SetToTextField("UniqueName", null, null, 1, 0, false, false);
			else
				nameRow.SetToTextField("UniqueName", null, null, 1, 0, false, false);
		}

		private void CreateClassSubject(Core.Helpers.Elements.Forms.Form form, Core.Helpers.Elements.Forms.Group classGroup) {
			Core.Helpers.Elements.Forms.Row subjectRow = classGroup.AddRow();
			TimetableText subjectText = new TimetableText();
			subjectRow.Description = "<strong>Subject</strong>";

			if (this._state.Equals(TimetableStates.Teaching))
				if (this._isUnique) {
					subjectText.Highlighted = "None";
					form.AddHiddenField("ClassSubjectId", "ClassSubjectId", null);

					form.AddHiddenField("ClassSubjectName-Original", "ClassSubjectName-Original", "None");
					form.AddHiddenField("ClassSubjectId-Original", "ClassSubjectId-Original", null);
				}
				else {
					subjectText.Highlighted = this._subjectName;
					form.AddHiddenField("ClassSubjectId", "ClassSubjectId", this._subjectId.ToString());

					form.AddHiddenField("ClassSubjectName-Original", "ClassSubjectName-Original", this._subjectName);
					form.AddHiddenField("ClassSubjectId-Original", "ClassSubjectId-Original", this._subjectId.ToString());
				}
			else {
				subjectText.Highlighted = "None";
				form.AddHiddenField("ClassSubjectId", "ClassSubjectId", null);

				form.AddHiddenField("ClassSubjectName-Original", "ClassSubjectName-Original", "None");
				form.AddHiddenField("ClassSubjectId-Original", "ClassSubjectId-Original", null);
			}

			subjectText.HighlightedId = "ClassSubjectName";
			subjectText.OnClick = "selectClassSubject();";

			subjectRow.Text = subjectText.ToString();
		}

		private void CreateClassQualification(Core.Helpers.Elements.Forms.Form form, Core.Helpers.Elements.Forms.Group classGroup) {
			Core.Helpers.Elements.Forms.Row qualificationRow = classGroup.AddRow();
			TimetableText qualificationText = new TimetableText();
			qualificationRow.Description = "<strong>Qualification</strong>";

			if (this._state.Equals(TimetableStates.Teaching))
				if (this._isUnique) {
					qualificationText.Highlighted = "None";
					form.AddHiddenField("ClassSubjectQualificationId", "ClassSubjectQualificationId", null);

					form.AddHiddenField("ClassQualificationName-Original", "ClassQualificationName-Original", "None");
					form.AddHiddenField("ClassSubjectQualificationId-Original", "ClassSubjectQualificationId-Original", null);
				}
				else {
					qualificationText.Highlighted = this._qualificationName;
					form.AddHiddenField("ClassSubjectQualificationId", "ClassSubjectQualificationId", this._subjectQualificationId.ToString());

					form.AddHiddenField("ClassQualificationName-Original", "ClassQualificationName-Original", this._qualificationName);
					form.AddHiddenField("ClassSubjectQualificationId-Original", "ClassSubjectQualificationId-Original", this._subjectQualificationId.ToString());
				}
			else {
				qualificationText.Highlighted = "None";
				form.AddHiddenField("ClassSubjectQualificationId", "ClassSubjectQualificationId", null);

				form.AddHiddenField("ClassQualificationName-Original", "ClassQualificationName-Original", "None");
				form.AddHiddenField("ClassSubjectQualificationId-Original", "ClassSubjectQualificationId-Original", null);
			}

			qualificationText.HighlightedId = "ClassQualificationName";
			qualificationText.OnClick = "selectClassQualification();";

			qualificationRow.Text = qualificationText.ToString();
		}

		private void CreateClassClass(Core.Helpers.Elements.Forms.Form form, Core.Helpers.Elements.Forms.Group classGroup) {
			Core.Helpers.Elements.Forms.Row classRow = classGroup.AddRow();
			TimetableText classText = new TimetableText();
			classRow.Description = "<strong>Class</strong>";

			if (this._state.Equals(TimetableStates.Teaching))
				if (this._isUnique) {
					classText.Highlighted = "None";
					form.AddHiddenField("ClassClassId", "ClassClassId", null);

					form.AddHiddenField("ClassClassName-Original", "ClassClassName-Original", "None");
					form.AddHiddenField("ClassClassId-Original", "ClassClassId-Original", null);
				}
				else {
					classText.Highlighted = this._className;
					form.AddHiddenField("ClassClassId", "ClassClassId", this._classId.ToString());

					form.AddHiddenField("ClassClassName-Original", "ClassClassName-Original", this._className);
					form.AddHiddenField("ClassClassId-Original", "ClassClassId-Original", this._classId.ToString());
				}
			else {
				classText.Highlighted = "None";
				form.AddHiddenField("ClassClassId", "ClassClassId", null);

				form.AddHiddenField("ClassClassName-Original", "ClassClassName-Original", "None");
				form.AddHiddenField("ClassClassId-Original", "ClassClassId-Original", null);
			}

			classText.HighlightedId = "ClassClassName";
			classText.OnClick = "selectClassClass();";

			classRow.Text = classText.ToString();
		}

		private void CreateClassRoom(Core.Helpers.Elements.Forms.Form form, Core.Helpers.Elements.Forms.Group classGroup) {
			Core.Helpers.Elements.Forms.Row roomRow = classGroup.AddRow();
			TimetableText roomText = new TimetableText();
			roomRow.Description = "<strong>Room</strong>";

			if (this._state.Equals(TimetableStates.Teaching))
				if (this._isUnique) {
					roomText.Highlighted = "None";
					form.AddHiddenField("ClassRoomId", "ClassRoomId", null);

					form.AddHiddenField("ClassRoomName-Original", "ClassRoomName-Original", "None");
					form.AddHiddenField("ClassRoomId-Original", "ClassRoomId-Original", null);
				}
				else {
					if (string.IsNullOrEmpty(this._roomName)) {
						roomText.Highlighted = "None";
						form.AddHiddenField("ClassRoomName-Original", "ClassRoomName-Original", "None");
					}
					else {
						roomText.Highlighted = this._roomName;
						form.AddHiddenField("ClassRoomName-Original", "ClassRoomName-Original", this._roomName);
					}
					
					form.AddHiddenField("ClassRoomId", "ClassRoomId", this._roomId.ToString());
					form.AddHiddenField("ClassRoomId-Original", "ClassRoomId-Original", this._roomId.ToString());
				}
			else {
				roomText.Highlighted = "None";
				form.AddHiddenField("ClassRoomId", "ClassRoomId", null);

				form.AddHiddenField("ClassRoomName-Original", "ClassRoomName-Original", "None");
				form.AddHiddenField("ClassRoomId-Original", "ClassRoomId-Original", null);
			}

			roomText.HighlightedId = "ClassRoomName";
			roomText.OnClick = "selectClassRoom();";

			roomRow.Text = roomText.ToString();
		}

		private bool HasStaffId() {
			this.Path.Next();

			if (this.Path.HasNext())
				return int.TryParse(this.Path.Next(), out this._staffId);

			return false;
		}

		private bool HasWeekNo() {
			if (this.Path.HasNext())
				if (int.TryParse(this.Path.Next(), out this._weekNo)) {
					try {
						this._weekNo.RequireThat("Week Number").IsInRange(1, Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout.NoWeeks);
						return true;
					}
					catch (IndexOutOfRangeException) {
						return false;
					}
				}

			return false;
		}

		private bool HasDayNo() {
			if (this.Path.HasNext())
				if (int.TryParse(this.Path.Next(), out this._dayNo)) {
					try {
						this._dayNo.RequireThat("Day Number").IsInRange(1, 7);
						return true;
					}
					catch (IndexOutOfRangeException) {
						return false;
					}
				}

			return false;
		}

		private bool HasPeriod() {
			if (this.Path.HasNext())
				if (int.TryParse(this.Path.Next(), out this._period)) {
					try {
						this._period.RequireThat("Period").IsInRange(1, Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this._weekNo][this._dayNo]);
						return true;
					}
					catch (IndexOutOfRangeException) {
						return false;
					}
					catch (Core.Schools.SettingsCollections.Timetabling.Layout.DayNotFoundException) {
						return false;
					}
				}

			return false;
		}

		private void PopulateName(SqlDataReader dataReader) {
			dataReader.NextResult();

			if (dataReader.Read()) {
				this._name = new StaffNameFormatter();
				this._name.Forename = dataReader["Forename"] as string;
				this._name.Surname = dataReader["Surname"] as string;
				this._name.HoldingName = dataReader["HoldingName"] as string;
				this._name.DisplayForenameFirst = true;
			}

			dataReader.NextResult();
			dataReader.Read();
		}

		private void HasAlertMessage() {
			if (this.Path.HasNext())
				this._alert = this.Path.Peek();
		}
	}

}