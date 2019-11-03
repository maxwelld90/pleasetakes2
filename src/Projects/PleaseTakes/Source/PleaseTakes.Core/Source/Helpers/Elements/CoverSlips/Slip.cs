using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.CoverSlips {

	internal sealed class Slip {
		private DateTime _coverDate;
		private StaffDetails _coverStaffDetails;
		private StaffDetails _absentStaffDetails;
		private ClassDetails _classDetails;
		private int _period;
		private int _coverId;
		private string _room;
		private bool _pageBreakBefore;
		private bool _isInternalCover;

		public Slip() {
			this._coverStaffDetails = new StaffDetails();
			this._absentStaffDetails = new StaffDetails();
			this._classDetails = new ClassDetails();
		}

		public DateTime CoverDate {
			get {
				return this._coverDate;
			}
			set {
				this._coverDate = value;
			}
		}

		public StaffDetails CoverStaffDetails {
			get {
				return this._coverStaffDetails;
			}
		}

		public StaffDetails AbsentStaffDetails {
			get {
				return this._absentStaffDetails;
			}
		}

		public ClassDetails ClassDetails {
			get {
				return this._classDetails;
			}
		}

		public int Period {
			get {
				return this._period;
			}
			set {
				this._period = value;
			}
		}

		public int CoverId {
			get {
				return this._coverId;
			}
			set {
				this._coverId = value;
			}
		}

		public string Room {
			get {
				return this._room;
			}
			set {
				this._room = value;
			}
		}

		public bool PageBreakBefore {
			get {
				return this._pageBreakBefore;
			}
			set {
				this._pageBreakBefore = value;
			}
		}

		public bool IsInternalCover {
			get {
				return this._isInternalCover;
			}
			set {
				this._isInternalCover = value;
			}
		}

		public override string ToString() {
			Constructor slip = new Constructor("/Templates/Elements/Coverslips/slip.html");

			if (this._pageBreakBefore)
				slip.SetVariable("PageBreakBefore", " BreakBefore");
			else
				slip.DeleteVariable("PageBreakBefore");

			slip.SetVariable("Date", this._coverDate.ToString("dddd, dd MMMM yyyy"));
			slip.SetVariable("SchoolName", WebServer.PleaseTakes.Session.CurrentInstance.School.FullName);
			slip.SetVariable("Period", this._period.ToString());
			slip.SetVariable("CoverId", this._coverId.ToString());

			if (this._isInternalCover)
				slip.SetVariable("CoverType", "INT");
			else
				slip.SetVariable("CoverType", "OUT");

			if (string.IsNullOrEmpty(this._room))
				slip.SetVariable("Room", "N/A");
			else
				slip.SetVariable("Room", this._room);

			slip.SetVariable("CoveringStaffName", this._coverStaffDetails.NameFormatter.ToString());

			if ((string.IsNullOrEmpty(this._coverStaffDetails.MainRoom)) && (string.IsNullOrEmpty(this._coverStaffDetails.Department)))
				slip.DeleteVariable("CoveringStaffDetails");
			else
				if (string.IsNullOrEmpty(this._coverStaffDetails.MainRoom))
					slip.SetVariable("CoveringStaffDetails", " <em>(" + this._coverStaffDetails.Department + ")</em>");
				else if (string.IsNullOrEmpty(this._coverStaffDetails.Department))
					slip.SetVariable("CoveringStaffDetails", " <em>(" + this._coverStaffDetails.MainRoom + ")</em>");
				else
					slip.SetVariable("CoveringStaffDetails", " <em>(" + this._coverStaffDetails.Department + " - " + this._coverStaffDetails.MainRoom + ")</em>");

			slip.SetVariable("AbsentStaffName", this._absentStaffDetails.NameFormatter.ToString());

			if (string.IsNullOrEmpty(this._absentStaffDetails.MainRoom))
				slip.DeleteVariable("AbsentStaffMainRoom");
			else
				slip.SetVariable("AbsentStaffMainRoom", " <em>(" + this._absentStaffDetails.MainRoom + ")</em>");

			slip.SetVariable("CommitmentName", this.ClassDetails.Name);

			if (string.IsNullOrEmpty(this.ClassDetails.Subject))
				slip.DeleteVariable("CommitmentSubject");
			else
				slip.SetVariable("CommitmentSubject", " <em>(" + this.ClassDetails.Subject + ")</em>");

			return slip.ToString();
		}
	}

}
