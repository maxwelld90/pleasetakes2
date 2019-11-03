using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Cover.Handlers.Arrange.Selection.Ajax.Selection {

	internal sealed class Standard : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _timetableWeek;
		private int _periods;
		private DateTime _selectedDate;

		public Standard(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			bool isInternal = false;
			bool showZeroEntitlement = false;
			string statusMessage = "";

			if (this.Path.HasNext()) {
				int coverId;
				string searchTerm;

				if (this.GetBoolInput(out isInternal) && this.GetBoolInput(out showZeroEntitlement) && this.CheckInputValidity() && this.GetCoverId(out coverId)) {
					DateTime startDate = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(this._selectedDate).RotationStartDate;
					DateTime endDate = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(this._selectedDate).RotationEndDate;
					searchTerm = this.GetSearchTerm();

					Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
					paramBuilder.AddParameter(SqlDbType.Int, "@CoverId", coverId);
					paramBuilder.AddParameter(SqlDbType.Bit, "@GetInternalStaff", isInternal);
					paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@CurrentDate", this._selectedDate);
					paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@StartDate", startDate);
					paramBuilder.AddParameter(SqlDbType.SmallDateTime, "@EndDate", endDate);
					paramBuilder.AddParameter(SqlDbType.Int, "@WeekNo", this._timetableWeek);
					paramBuilder.AddParameter(SqlDbType.Int, "@DayNo", (int)(this._selectedDate.DayOfWeek + 1));
					paramBuilder.AddParameter(SqlDbType.Bit, "@ShowZeroEntitlement", showZeroEntitlement);

					if (string.IsNullOrEmpty(searchTerm))
						paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", "");
					else
						paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", searchTerm);

					using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Cover/Arrange/Selection/Selection/list.sql", paramBuilder.Parameters)) {
						dataReader.Read();

						if ((bool)dataReader["DoesExist"]) {
							dataReader.NextResult();
							int staffCount = 0;

							if (dataReader.HasRows) {
								Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();
								recordCollection.Id = "Selection";
								
								while (dataReader.Read()) {
									Records.Selection selectionRecord = new Records.Selection();
									selectionRecord.Id = ((int)dataReader["Id"]).ToString();
									selectionRecord.IsInternal = isInternal;
									selectionRecord.Entitlement = (int)dataReader["RemainingEntitlement"];
									selectionRecord.IsSelected = (bool)dataReader["IsSelected"];

									if (isInternal) {
										UserManagement.StaffNameFormatter nameFormatter = new UserManagement.StaffNameFormatter();
										nameFormatter.Forename = dataReader["Forename"] as string;
										nameFormatter.Surname = dataReader["Surname"] as string;
										nameFormatter.HoldingName = dataReader["HoldingName"] as string;

										selectionRecord.StaffName = nameFormatter;
									}
									else {
										UserManagement.StaffNameFormatter nameFormatter = new UserManagement.StaffNameFormatter();
										nameFormatter.Forename = dataReader["Forename"] as string;
										nameFormatter.Surname = dataReader["Surname"] as string;

										selectionRecord.StaffName = nameFormatter;
									}

									recordCollection.Add(selectionRecord.Record);
									staffCount++;
								}

								this.Page.Contents = recordCollection.ToString();
							}
							else {
								Core.Helpers.Elements.Alerts.Alert noResultsAlert = new Core.Helpers.Elements.Alerts.Alert("NoReults");
								noResultsAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
								noResultsAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Selection/Selection/noresults.html").ToString();
								noResultsAlert.NoScript = false;
								noResultsAlert.ShowCloseBox = false;
								noResultsAlert.StartHidden = false;

								this.Page.Contents = noResultsAlert.ToString();
							}

							if (staffCount.Equals(0))
								statusMessage = "<strong>Couldnae find anyone!</strong>";
							else
								if (staffCount.Equals(1))
									if (isInternal)
										statusMessage = "Found <strong>1</strong> member of teaching staff";
									else
										statusMessage = "Found <strong>1</strong> member of outside cover staff";
								else
									if (isInternal)
										statusMessage = "Found <strong>" + staffCount + "</strong> members of teaching staff";
									else
										statusMessage = "Found <strong>" + staffCount + "</strong> members of outside cover staff";
						}
						else {
							Core.Helpers.Elements.Alerts.Alert requestDoesNotExistAlert = new Core.Helpers.Elements.Alerts.Alert("RequestDoesNotExist");
							requestDoesNotExistAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
							requestDoesNotExistAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Selection/Selection/requestdoesnotexist.html").ToString();
							requestDoesNotExistAlert.NoScript = false;
							requestDoesNotExistAlert.ShowCloseBox = false;
							requestDoesNotExistAlert.StartHidden = false;

							this.Page.Contents = requestDoesNotExistAlert.ToString();
						}
					}
				}
				else {
					Core.Helpers.Elements.Alerts.Alert badInputAlert = new Core.Helpers.Elements.Alerts.Alert("BadInput");
					badInputAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
					badInputAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Selection/Selection/badinput.html").ToString();
					badInputAlert.NoScript = false;
					badInputAlert.ShowCloseBox = false;
					badInputAlert.StartHidden = false;

					this.Page.Contents = badInputAlert.ToString();
				}
			}
			else {
				Core.Helpers.Elements.Alerts.Alert noRequestSelectedAlert = new Core.Helpers.Elements.Alerts.Alert("NoRequestSelected");
				noRequestSelectedAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
				noRequestSelectedAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Cover/Arrange/Selection/Selection/norequestselected.html").ToString();
				noRequestSelectedAlert.NoScript = false;
				noRequestSelectedAlert.ShowCloseBox = false;
				noRequestSelectedAlert.StartHidden = false;

				this.Page.Contents = noRequestSelectedAlert.ToString();
			}

			if (!string.IsNullOrEmpty(statusMessage)) {
				string hoverMessage = "";
				if (isInternal && showZeroEntitlement)
					hoverMessage = "Showing all available teaching staff";
				else if (isInternal && !showZeroEntitlement)
					hoverMessage = "Showing available teaching staff with a remaining entitlement of 1 or greater";
				else if (!isInternal && showZeroEntitlement)
					hoverMessage = "Showing all available outside cover staff";
				else
					hoverMessage = "Showing available outside cover staff with a remaining entitlement of 1 or greater";

				statusMessage = "<p class=\"Plain\" title=\"" + hoverMessage + "\">" + statusMessage + "</p>";
			}

			if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("ArrangeSelectionInfo"))
				Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["ArrangeSelectionInfo"] = statusMessage;
			else
				Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("ArrangeSelectionInfo", statusMessage);
		}

		private bool GetBoolInput(out bool result) {
			if (this.Path.HasNext())
				switch (this.Path.Next()) {
					case "internal":
					case "show":
						result = true;
						return true;
					case "outside":
					case "hide":
						result = false;
						return true;
					default:
						result = false;
						return false;
				}

			result = false;
			return false;
		}

		private bool GetCoverId(out int result) {
			if (this.Path.HasNext()) {
				return int.TryParse(this.Path.Next(), out result);
			}

			result = 0;
			return false;
		}

		private string GetSearchTerm() {
			if (this.Path.HasNext() && this.Path.Next().Equals("search"))
				if (this.Path.HasNext())
					return this.Path.Next();

			return null;
		}

		private bool CheckInputValidity() {
			bool validYear = false;
			bool validMonth = false;
			bool validDay = false;
			int year = 0;
			int month = 0;
			int day = 0;

			for (int i = 0; i <= 2; i++) {
				if (this.Path.HasNext())
					switch (i) {
						case 0:
							validYear = int.TryParse(this.Path.Next(), out year);
							break;
						case 1:
							validMonth = int.TryParse(this.Path.Next(), out month);
							break;
						case 2:
							validDay = int.TryParse(this.Path.Next(), out day);
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

					if (Standard.IsDateInSessionRange(year, month, day)) {
						DateTime selectedDate = new DateTime(year, month, day);
						this._timetableWeek = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.SessionDetails.CurrentSession.GetDateSessionInformation(selectedDate).TimetableWeek;

						try {
							this._periods = Core.WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Timetabling.Layout[this._timetableWeek][(int)selectedDate.DayOfWeek + 1];

							if (this._periods.Equals(0))
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