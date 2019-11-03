using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Ajax.Teaching {

	internal sealed class TimetableSelection : Core.Helpers.BaseHandlers.AjaxHandler {
		private TimetableSelectionStates _state;
		private string _searchTerm = "";
		private bool _success;

		public TimetableSelection(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			if (this.HasState()) {
				Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();
				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				string sqlPath = "";
				int recordCount = 0;
				int subjectId;

				switch (this._state) {
					case TimetableSelectionStates.UniqueYeargroup:
						this.SetSearchTerm();
						sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/Selection/yeargroups.sql";
						break;
					case TimetableSelectionStates.UniqueRoom:
						this.SetSearchTerm();
						sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/Selection/rooms.sql";
						break;
					case TimetableSelectionStates.ClassSubject:
						this.SetSearchTerm();
						sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/Selection/subjects.sql";
						break;
					case TimetableSelectionStates.ClassQualification:
						this.GetId(out subjectId);
						paramBuilder.AddParameter(SqlDbType.Int, "@SubjectId", subjectId);

						this.SetSearchTerm();
						sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/Selection/qualifications.sql";
						break;
					case TimetableSelectionStates.ClassClass:
						this.GetId(out subjectId);

						this.SetSearchTerm();
						paramBuilder.AddParameter(SqlDbType.Int, "@SubjectQualificationId", subjectId);
						sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/Selection/classes.sql";
						break;
					case TimetableSelectionStates.ClassRoom:
						this.SetSearchTerm();
						sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/Selection/rooms.sql";
						break;
				}

				paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", this._searchTerm);

				switch (this._state) {
					case TimetableSelectionStates.UniqueRoom:
					case TimetableSelectionStates.ClassRoom:
						Core.Helpers.Elements.RecordLists.Record noRoomRecord = new Core.Helpers.Elements.RecordLists.Record();

						noRoomRecord.Colour = Core.Helpers.Elements.RecordLists.Colours.Yellow;
						noRoomRecord.LeftSide.Image.Source = "nodoor.png";
						noRoomRecord.LeftSide.Image.ToolTip = "Teaching does not take place in a room";
						noRoomRecord.LeftSide.MainText = "<em>Teaching does not take place in a room</em>";

						if (this._state.Equals(TimetableSelectionStates.UniqueRoom))
							noRoomRecord.OnClick = "setUniqueRoom();";
						else
							noRoomRecord.OnClick = "setClassRoom();";

						recordCollection.Add(noRoomRecord);
						break;
				}

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader(sqlPath, paramBuilder.Parameters)) {
					if (dataReader.HasRows) {
						while (dataReader.Read()) {
							switch (this._state) {
								case TimetableSelectionStates.UniqueYeargroup:
									Core.Helpers.Elements.RecordLists.Record yeargroupRecord = new Core.Helpers.Elements.RecordLists.Record();
									
									yeargroupRecord.LeftSide.Image.Source = "yeargroup.png";
									yeargroupRecord.LeftSide.Image.ToolTip = "Yeargroup";
									yeargroupRecord.LeftSide.MainText = dataReader["Name"] as string;
									yeargroupRecord.OnClick = "setUniqueYeargroup('" + dataReader["Name"] as string + "', " + (int)dataReader["Id"] + ");";

									recordCount++;
									recordCollection.Add(yeargroupRecord);
									break;
								case TimetableSelectionStates.UniqueRoom:
								case TimetableSelectionStates.ClassRoom:
									Core.Helpers.Elements.RecordLists.Record roomRecord = new Core.Helpers.Elements.RecordLists.Record();

									roomRecord.LeftSide.Image.Source = "door.png";
									roomRecord.LeftSide.Image.ToolTip = "Room";
									roomRecord.LeftSide.MainText = dataReader["Name"] as string;
									
									if (this._state.Equals(TimetableSelectionStates.UniqueRoom))
										roomRecord.OnClick = "setUniqueRoom('" + dataReader["Name"] as string + "', " + (int)dataReader["Id"] + ");";
									else
										roomRecord.OnClick = "setClassRoom('" + dataReader["Name"] as string + "', " + (int)dataReader["Id"] + ");";

									recordCount++;
									recordCollection.Add(roomRecord);
									break;
								case TimetableSelectionStates.ClassSubject:
									Core.Helpers.Elements.RecordLists.Record subjectRecord = new Core.Helpers.Elements.RecordLists.Record();

									subjectRecord.LeftSide.Image.Source = "subject.png";
									subjectRecord.LeftSide.Image.ToolTip = "Subject";
									subjectRecord.LeftSide.MainText = dataReader["Name"] as string;

									string subjectName;

									if (string.IsNullOrEmpty(dataReader["Abbreviation"] as string))
										subjectName = dataReader["Name"] as string;
									else
										subjectName = dataReader["Abbreviation"] as string;

									subjectRecord.OnClick = "setClassSubject('" + subjectName + "', " + (int)dataReader["Id"] + ");";

									recordCount++;
									recordCollection.Add(subjectRecord);
									break;
								case TimetableSelectionStates.ClassQualification:
									Core.Helpers.Elements.RecordLists.Record qualificationRecord = new Core.Helpers.Elements.RecordLists.Record();

									qualificationRecord.LeftSide.Image.Source = "lightbulb.png";
									qualificationRecord.LeftSide.Image.ToolTip = "Qualification";
									qualificationRecord.LeftSide.MainText = dataReader["Name"] as string;
									qualificationRecord.OnClick = "setClassQualification('" + dataReader["Name"] as string + "', " + (int)dataReader["Id"] + ");";

									recordCount++;
									recordCollection.Add(qualificationRecord);
									break;
								case TimetableSelectionStates.ClassClass:
									Core.Helpers.Elements.RecordLists.Record classRecord = new Core.Helpers.Elements.RecordLists.Record();

									classRecord.LeftSide.Image.Source = "class.png";
									classRecord.LeftSide.Image.ToolTip = "Class";
									classRecord.LeftSide.MainText = dataReader["Name"] as string;
									classRecord.OnClick = "setClassClass('" + dataReader["Name"] as string + "', " + (int)dataReader["Id"] + ");";

									recordCount++;
									recordCollection.Add(classRecord);
									break;
							}
						}

						this.Page.Contents = recordCollection.ToString();
					}
					else {
						switch (this._state) {
							case TimetableSelectionStates.UniqueYeargroup:
								Core.Helpers.Elements.Alerts.Alert noYeargroupsAlert = new Core.Helpers.Elements.Alerts.Alert("NoYeargroups");
								noYeargroupsAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
								noYeargroupsAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/Selection/noyeargroups.html").ToString();

								this.Page.Contents = noYeargroupsAlert.ToString();
								break;
							case TimetableSelectionStates.UniqueRoom:
							case TimetableSelectionStates.ClassRoom:
								this.Page.Contents = recordCollection.ToString();
								break;
							case TimetableSelectionStates.ClassSubject:
								Core.Helpers.Elements.Alerts.Alert noSubjectsAlert = new Core.Helpers.Elements.Alerts.Alert("NoSubjects");
								noSubjectsAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
								noSubjectsAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/Selection/nosubjects.html").ToString();

								this.Page.Contents = noSubjectsAlert.ToString();
								break;
							case TimetableSelectionStates.ClassQualification:
								Core.Helpers.Elements.Alerts.Alert noQualificationsAlert = new Core.Helpers.Elements.Alerts.Alert("NoQualifications");
								noQualificationsAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
								noQualificationsAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/Selection/noqualifications.html").ToString();

								this.Page.Contents = noQualificationsAlert.ToString();
								break;
							case TimetableSelectionStates.ClassClass:
								Core.Helpers.Elements.Alerts.Alert noClassesAlert = new Core.Helpers.Elements.Alerts.Alert("NoClasses");
								noClassesAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
								noClassesAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/Selection/noclasses.html").ToString();

								this.Page.Contents = noClassesAlert.ToString();
								break;
						}
					}
				}

				string statusMessage = "";

				switch (this._state) {
					case TimetableSelectionStates.UniqueYeargroup:
						if (recordCount.Equals(0))
							statusMessage = "Found no yeargroups!";
						else if (recordCount.Equals(1))
							statusMessage = "Found <strong>1</strong> yeargroup";
						else
							statusMessage = "Found <strong>" + recordCount + "</strong> yeargroups";
						break;
					case TimetableSelectionStates.UniqueRoom:
					case TimetableSelectionStates.ClassRoom:
						if (recordCount.Equals(0))
							statusMessage = "Found no rooms!";
						else if (recordCount.Equals(1))
							statusMessage = "Found <strong>1</strong> room";
						else
							statusMessage = "Found <strong>" + recordCount + "</strong> rooms";
						break;
					case TimetableSelectionStates.ClassSubject:
						if (recordCount.Equals(0))
							statusMessage = "Found no subjects!";
						else if (recordCount.Equals(1))
							statusMessage = "Found <strong>1</strong> subject";
						else
							statusMessage = "Found <strong>" + recordCount + "</strong> subjects";
						break;
					case TimetableSelectionStates.ClassQualification:
						if (recordCount.Equals(0))
							statusMessage = "Found no qualifications!";
						else if (recordCount.Equals(1))
							statusMessage = "Found <strong>1</strong> qualification";
						else
							statusMessage = "Found <strong>" + recordCount + "</strong> qualifications";
						break;
					case TimetableSelectionStates.ClassClass:
						if (recordCount.Equals(0))
							statusMessage = "Found no classes!";
						else if (recordCount.Equals(1))
							statusMessage = "Found <strong>1</strong> class";
						else
							statusMessage = "Found <strong>" + recordCount + "</strong> classes";
						break;
				}

				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffTeachingTimetableSelection"))
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingTimetableSelection"] = statusMessage;
				else
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("StaffTeachingTimetableSelection", statusMessage);
			}
			else {
				if (this._success) {
					Core.Helpers.Elements.Alerts.Alert success = new Core.Helpers.Elements.Alerts.Alert("Success");
					success.Colour = Core.Helpers.Elements.Alerts.Colours.Green;
					success.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/Selection/success.html").ToString();

					this.Page.Contents = success.ToString();
				}
				else {
					Core.Helpers.Elements.Alerts.Alert noSelection = new Core.Helpers.Elements.Alerts.Alert("NoSelection");
					noSelection.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
					noSelection.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Period/Selection/noselectionstate.html").ToString();

					this.Page.Contents = noSelection.ToString();
				}

				if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffTeachingTimetableSelection"))
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingTimetableSelection"] = "";
				else
					Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("StaffTeachingTimetableSelection", "");
			}
		}

		private void GetId(out int result) {
			if (this.Path.HasNext())
				int.TryParse(this.Path.Next(), out result);
			else
				result = -1;
		}

		private void SetSearchTerm() {
			if (this.Path.HasNext()) {
				this.Path.Next();

				if (this.Path.HasNext())
					this._searchTerm = this.Path.Next();
			}
		}

		private bool HasState() {
			if (this.Path.HasNext())
				switch (this.Path.Next()) {
					case "uniqueyeargroup":
						this._state = TimetableSelectionStates.UniqueYeargroup;
						return true;
					case "uniqueroom":
						this._state = TimetableSelectionStates.UniqueRoom;
						return true;
					case "classsubject":
						this._state = TimetableSelectionStates.ClassSubject;
						return true;
					case "classqualification":
						this._state = TimetableSelectionStates.ClassQualification;
						return true;
					case "classclass":
						this._state = TimetableSelectionStates.ClassClass;
						return true;
					case "classroom":
						this._state = TimetableSelectionStates.ClassRoom;
						return true;
					case "success":
						this._success = true;
						break;
				}

			this._state = TimetableSelectionStates.None;
			return false;
		}

	}

}