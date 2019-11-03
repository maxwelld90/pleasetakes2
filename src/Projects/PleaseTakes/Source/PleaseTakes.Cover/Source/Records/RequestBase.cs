using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Records {

	internal class RequestBase : RecordBase {
		private int? _period;
		private string _commitmentName;

		public RequestBase() {
			this._record.LeftSide.Image.Source = "dudeabsent.png";
		}

		public UserManagement.StaffNameFormatter AbsenteeName {
			set {
				this._record.LeftSide.MainText = "<strong>" + value.ToString() + "</strong>";
			}
		}

		public UserManagement.StaffNameFormatter AbsenteeNameToolTip {
			set {
				value.IsToolTip = true;
				this._record.LeftSide.Image.ToolTip = value.ToString();
			}
		}

		public int? Period {
			get {
				return this._period;
			}
			set {
				this._period = value;
				SetAbsenteeDetails(true);
			}
		}

		public string CommitmentName {
			get {
				return this._commitmentName;
			}
			set {
				this._commitmentName = value;
				SetAbsenteeDetails(false);
			}
		}

		private void SetAbsenteeDetails(bool setPeriod) {
			if (setPeriod) {
				if (string.IsNullOrEmpty(this._commitmentName))
					this._record.LeftSide.SmallText = "Period " + this._period;
				else
					this._record.LeftSide.SmallText = "Period " + this._period + "<br />" + this._commitmentName;
			}
			else
				if (this._period == null)
					this._record.LeftSide.SmallText = this._commitmentName;
				else
					this._record.LeftSide.SmallText = "Period " + this._period + "<br />" + this._commitmentName;
		}

		public override string ToString() {
			return this._record.ToString();
		}
	}

}