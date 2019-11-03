using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover.Records {

	internal sealed class Selection : RecordBase {
		private int _entitlement;
		private bool _isInternal;
		private bool _isSelected;

		public Selection() {
			this._record.RightSide.Image.Source = "clock.png";
		}

		public string Id {
			get {
				return this.Record.Id;
			}
			set {
				this.Record.Id = value;
				this._record.OnClick = "setSelection(" + value + ")";
			}
		}

		public UserManagement.StaffNameFormatter StaffName {
			set {
				this._record.LeftSide.MainText = value.ToString();

				value.IsToolTip = true;
				this._record.LeftSide.Image.ToolTip = value.ToString();
			}
		}

		public int Entitlement {
			get {
				return this._entitlement;
			}
			set {
				this._entitlement = value;
				this._record.RightSide.MainText = "Remaining entitlement: <strong>" + this._entitlement.ToString() + "</strong>";
				this._record.RightSide.Image.ToolTip = "Remaining entitlement: " + this._entitlement.ToString();

				if ((this._entitlement <= 0) && (!this._isSelected))
					this._record.Colour = Core.Helpers.Elements.RecordLists.Colours.Red;
			}
		}

		public bool IsInternal {
			get {
				return this._isInternal;
			}
			set {
				this._isInternal = value;

				if (this._isInternal)
					this._record.LeftSide.Image.Source = "dude.png";
				else
					this._record.LeftSide.Image.Source = "dudettedoor.png";
			}
		}

		public bool IsSelected {
			get {
				return this._isSelected;
			}
			set {
				this._isSelected = value;

				if (value)
					this._record.Colour = Core.Helpers.Elements.RecordLists.Colours.Green;
			}
		}

	}

}