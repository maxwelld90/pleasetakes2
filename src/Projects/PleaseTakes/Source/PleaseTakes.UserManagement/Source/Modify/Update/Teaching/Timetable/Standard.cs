using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.UserManagement.Modify.Update.Teaching.Timetable {

	internal sealed class Standard {
		private Core.Helpers.Path.Parser _path;
		private TimetableStates _state;
		private int _staffId;
		private int _weekNo;
		private int _dayNo;
		private int _period;
		private int _yeargroupId;
		private int _subjectId;
		private int _subjectQualificationId;
		private int _classId;
		private string _commitmentName;
		private int? _roomId;

		public Standard(Core.Helpers.Path.Parser path) {
			this._path = path;

			if (this.HasState() && this.HasStaffId() && this.HasWeekNo() && this.HasDayNo() && this.HasPeriod()) {
				this.DoUpdate();
			}
			else
				Core.WebServer.PleaseTakes.Redirect("/staff/inputteaching#Teaching");
		}

		private void DoUpdate() {
			Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
			paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
			paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._weekNo);
			paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", this._dayNo);
			paramBuilder.AddParameter(SqlDbType.Int, "@Period", this._period);

			string sqlPath = "";

			switch (this._state) {
				case TimetableStates.Unavailable:
					sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/unavailable.sql";
					break;
				case TimetableStates.Free:
					sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/free.sql";
					break;
				case TimetableStates.Teaching:
					sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/teaching.sql";

					if (!string.IsNullOrEmpty(Core.WebServer.Request["SelectedGroup"]) && (Core.WebServer.Request["SelectedGroup"].Equals("Unique") || Core.WebServer.Request["SelectedGroup"].Equals("Class"))) {
						bool isUnique = Core.WebServer.Request["SelectedGroup"].Equals("Unique");

						if (isUnique) {
							paramBuilder.AddParameter(SqlDbType.Bit, "@IsUnique", true);

							if (this.HasYeargroupId(Core.WebServer.Request["UniqueYeargroupId"]) && this.HasCommitmentName(Core.WebServer.Request["UniqueName"])) {
								this.CheckRoomId(Core.WebServer.Request["UniqueRoomId"]);

								paramBuilder.AddParameter(SqlDbType.Int, "@YeargroupId", this._yeargroupId);
								paramBuilder.AddParameter(SqlDbType.Int, "@RoomId", this._roomId);
								paramBuilder.AddParameter(SqlDbType.VarChar, "@CommitmentName", this._commitmentName);

								paramBuilder.AddParameter(SqlDbType.Int, "@SubjectId", null);
								paramBuilder.AddParameter(SqlDbType.Int, "@SubjectQualificationId", null);
								paramBuilder.AddParameter(SqlDbType.Int, "@ClassId", null);
							}
							else
								Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/timetable/" + this._staffId + "/" + this._weekNo + "/" + this._dayNo + "/" + this._period + "/uniquefail/#Teaching");
						}
						else {
							paramBuilder.AddParameter(SqlDbType.Bit, "@IsUnique", false);

							if (this.HasSubjectId() && this.HasSubjectQualificationId() && this.HasClassId()) {
								this.CheckRoomId(Core.WebServer.Request["ClassRoomId"]);

								paramBuilder.AddParameter(SqlDbType.Int, "@SubjectId", this._subjectId);
								paramBuilder.AddParameter(SqlDbType.Int, "@SubjectQualificationId", this._subjectQualificationId);
								paramBuilder.AddParameter(SqlDbType.Int, "@ClassId", this._classId);
								paramBuilder.AddParameter(SqlDbType.Int, "@RoomId", this._roomId);

								paramBuilder.AddParameter(SqlDbType.Int, "@YeargroupId", null);
								paramBuilder.AddParameter(SqlDbType.VarChar, "@CommitmentName", null);
							}
							else
								Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/timetable/" + this._staffId + "/" + this._weekNo + "/" + this._dayNo + "/" + this._period + "/classfail/#Teaching");
						}
					}
					else
						Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/timetable/" + this._staffId + "/" + this._weekNo + "/" + this._dayNo + "/" + this._period + "/teachingfail/#Teaching");

					break;
				case TimetableStates.Busy:
					sqlPath = "/Sql/Specific/Staff/Modify/Teaching/Timetable/busy.sql";
					string busyCommitment = Core.WebServer.Request["BusyCommitment"];

					if (string.IsNullOrEmpty(busyCommitment))
						Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/timetable/" + this._staffId + "/" + this._weekNo + "/" + this._dayNo + "/" + this._period + "/busyfailed/#Busy");
					else
						paramBuilder.AddParameter(SqlDbType.VarChar, "@CommitmentName", busyCommitment);
					
					break;
			}
			
			using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader(sqlPath, paramBuilder.Parameters)) {
				dataReader.Read();
				int status = (int)dataReader["Status"];

				if (status.Equals(2))
					Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/week/" + this._weekNo + "/success/#Timetable");
				else
					Core.WebServer.PleaseTakes.Redirect("/staff/modify/teaching/" + this._staffId + "/week/" + this._weekNo + "/failed/#Timetable");
			}
		}

		private bool HasSubjectId() {
			return int.TryParse(Core.WebServer.Request["ClassSubjectId"], out this._subjectId);
		}

		private bool HasSubjectQualificationId() {
			return int.TryParse(Core.WebServer.Request["ClassSubjectQualificationId"], out this._subjectQualificationId);
		}

		private bool HasClassId() {
			return int.TryParse(Core.WebServer.Request["ClassClassId"], out this._classId);
		}

		private bool HasYeargroupId(string fromForm) {
			return int.TryParse(fromForm, out this._yeargroupId);
		}

		private bool HasCommitmentName(string fromForm) {
			if (string.IsNullOrEmpty(fromForm))
				return false;
			else
				this._commitmentName = fromForm;

			return true;
		}

		private void CheckRoomId(string fromForm) {
			if (!string.IsNullOrEmpty(fromForm)) {
				int intFromForm;

				if (int.TryParse(fromForm, out intFromForm))
					this._roomId = intFromForm;
			}
		}

		private bool HasState() {
			string fromForm = Core.WebServer.Request["State"];

			if (string.IsNullOrEmpty(fromForm))
				return false;

			switch (fromForm) {
				case "Unavailable":
					this._state = TimetableStates.Unavailable;
					break;
				case "Free":
					this._state = TimetableStates.Free;
					break;
				case "Teaching":
					this._state = TimetableStates.Teaching;
					break;
				case "Busy":
					this._state = TimetableStates.Busy;
					break;
			}

			return true;
		}

		private bool HasStaffId() {
			string fromForm = Core.WebServer.Request["StaffId"];

			if (string.IsNullOrEmpty(fromForm))
				return false;

			return int.TryParse(fromForm, out this._staffId);
		}

		private bool HasWeekNo() {
			string fromForm = Core.WebServer.Request["WeekNo"];

			if (string.IsNullOrEmpty(fromForm))
				return false;

			if (int.TryParse(fromForm, out this._weekNo)) {
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
			string fromForm = Core.WebServer.Request["DayNo"];

			if (string.IsNullOrEmpty(fromForm))
				return false;

			if (int.TryParse(fromForm, out this._dayNo)) {
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
			string fromForm = Core.WebServer.Request["Period"];

			if (string.IsNullOrEmpty(fromForm))
				return false;

			if (int.TryParse(fromForm, out this._period)) {
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

	}

}