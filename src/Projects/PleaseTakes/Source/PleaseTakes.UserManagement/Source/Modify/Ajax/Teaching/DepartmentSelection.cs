using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.UserManagement.Modify.Ajax.Teaching {

	internal sealed class DepartmentSelection : Core.Helpers.BaseHandlers.AjaxHandler {
		private int _staffId;
		private string _searchTerm;

		public DepartmentSelection(Core.Helpers.Path.Parser path)
			: base(path) {
			this.Output.Send();
		}

		protected override void GenerateOutput() {
			if (this.HasStaffId()) {
				this.SetSearchTerm();

				Core.Helpers.Database.ParameterBuilder paramBuilder = new Core.Helpers.Database.ParameterBuilder();
				Core.Helpers.Elements.RecordLists.Collection recordCollection = new Core.Helpers.Elements.RecordLists.Collection();
				recordCollection.Id = "Department";
				
				paramBuilder.AddParameter(SqlDbType.Int, "@StaffId", this._staffId);
				paramBuilder.AddParameter(SqlDbType.VarChar, "@SearchTerm", this._searchTerm);

				using (SqlDataReader dataReader = Core.Helpers.Database.Provider.ExecuteReader("/Sql/Specific/Staff/Modify/Teaching/department.sql", paramBuilder.Parameters)) {
					dataReader.Read();

					if (((bool)dataReader["Status"])) {
						dataReader.NextResult();

						if (dataReader.HasRows) {
							int recordCount = 0;

							while (dataReader.Read()) {
								Core.Helpers.Elements.RecordLists.Record departmentRecord = new Core.Helpers.Elements.RecordLists.Record();

								int departmentId = (int)dataReader["Id"];
								
								departmentRecord.LeftSide.Image.Source = "books.png";
								departmentRecord.LeftSide.Image.ToolTip = "Department";
								departmentRecord.LeftSide.MainText = dataReader["Name"] as string;
								departmentRecord.Id = departmentId.ToString();
								departmentRecord.OnClick = "getResponse('Department" + departmentId + "', '?path=/staff/modify/update/teaching/department/" + this._staffId + "/" + departmentId + "/', false, false, true);";

								if ((bool)dataReader["IsSelected"])
									departmentRecord.Colour = Core.Helpers.Elements.RecordLists.Colours.Green;
								else
									departmentRecord.Colour = Core.Helpers.Elements.RecordLists.Colours.Blue;

								recordCollection.Add(departmentRecord);
								recordCount++;
							}

							this.Page.Contents = recordCollection.ToString();

							if (Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.ContainsKey("StaffTeachingDepartmentSelection"))
								Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage["StaffTeachingDepartmentSelection"] = recordCount;
							else
								Core.WebServer.PleaseTakes.Session.CurrentInstance.TemporaryStorage.Add("StaffTeachingDepartmentSelection", recordCount);
						}
						else {
							Core.Helpers.Elements.Alerts.Alert noResultsAlert = new Core.Helpers.Elements.Alerts.Alert("NoResults");
							noResultsAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Yellow;
							noResultsAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Department/noresults.html").ToString();

							this.Page.Contents = noResultsAlert.ToString();
						}
					}
					else {
						Core.Helpers.Elements.Alerts.Alert invalidAlert = new Core.Helpers.Elements.Alerts.Alert("InvalidDetails");
						invalidAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
						invalidAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Department/invalid.html").ToString();

						this.Page.Contents = invalidAlert.ToString();
					}
				}
			}
			else {
				Core.Helpers.Elements.Alerts.Alert missingAlert = new Core.Helpers.Elements.Alerts.Alert("MissingDetails");
				missingAlert.Colour = Core.Helpers.Elements.Alerts.Colours.Red;
				missingAlert.Message = new Core.Helpers.Constructor("/Alerts/Specific/Staff/Modify/Teaching/Department/missing.html").ToString();

				this.Page.Contents = missingAlert.ToString();
			}
		}

		private bool HasStaffId() {
			if (this.Path.HasNext())
				return int.TryParse(this.Path.Next(), out this._staffId);

			return false;
		}

		private void SetSearchTerm() {
			if (this.Path.HasNext()) {
				this.Path.Next();

				if (this.Path.HasNext())
					this._searchTerm = this.Path.Next();
				else
					this._searchTerm = "";
			}
			else
				this._searchTerm = "";
		}
	}

}