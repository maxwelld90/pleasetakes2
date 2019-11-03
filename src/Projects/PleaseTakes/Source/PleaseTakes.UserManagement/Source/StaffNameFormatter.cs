using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.UserManagement {

	internal sealed class StaffNameFormatter {
		private bool _isTooltip;
		private bool _abbreviateForename;
		private bool _displayForenameFirst;
		private bool _displaySmallHoldingName;
		private string _title;
		private string _forename;
		private string _surname;
		private string _holdingName;

		public bool IsToolTip {
			get {
				return this._isTooltip;
			}
			set {
				this._isTooltip = value;
			}
		}

		public bool AbbreviateForename {
			get {
				return this._abbreviateForename;
			}
			set {
				this._abbreviateForename = value;
			}
		}

		public bool DisplayForenameFirst {
			get {
				return this._displayForenameFirst;
			}
			set {
				this._displayForenameFirst = value;
			}
		}

		public bool DisplaySmallHoldingName {
			get {
				return this._displaySmallHoldingName;
			}
			set {
				this._displaySmallHoldingName = value;
			}
		}

		public string Title {
			get {
				return this._title;
			}
			set {
				this._title = value;
			}
		}

		public string Forename {
			get {
				return this._forename;
			}
			set {
				this._forename = value;
			}
		}

		public string Surname {
			get {
				return this._surname;
			}
			set {
				this._surname = value;
			}
		}

		public string HoldingName {
			get {
				return this._holdingName;
			}
			set {
				this._holdingName = value;
			}
		}

		public override string ToString() {
			if ((string.IsNullOrEmpty(this._forename)) && (string.IsNullOrEmpty(this._surname)) && (string.IsNullOrEmpty(this._holdingName)))
				if (this._isTooltip)
					return "Uknown staff name";
				else
					return "<em>Unknown staff name</em>";
			else
				if ((string.IsNullOrEmpty(this._forename)) && (string.IsNullOrEmpty(this._surname)))
					if (this._isTooltip)
						return this._holdingName;
					else
						return "<em>" + this._holdingName + "</em>";
				else
					if (string.IsNullOrEmpty(this._holdingName))
						if (this._abbreviateForename)
							if (this._displayForenameFirst)
								return char.ToUpper(this._forename[0]) + " " + this._surname;
							else
								return this._surname + ", " + char.ToUpper(this._forename[0]) + ".";
						else
							if (this._displayForenameFirst)
								return this._forename + " " + this._surname;
							else
								return this._surname + ", " + this._forename;
					else
						if (this._abbreviateForename)
							if (this._displaySmallHoldingName)
								if (this._displayForenameFirst)
									if (this._isTooltip)
										return char.ToUpper(this._forename[0]) + " " + this._surname + " (" + this._holdingName + ")";
									else
										return char.ToUpper(this._forename[0]) + " " + this._surname + " <em style=\"font-size: 8pt;\">(" + this._holdingName + ")</em>";
								else
									if (this._isTooltip)
										return this._surname + ", " + char.ToUpper(this._forename[0]) + ". (" + this._holdingName + ")";
									else
										return this._surname + ", " + char.ToUpper(this._forename[0]) + ". <em style=\"font-size: 8pt;\">(" + this._holdingName + ")</em>";
							else
								if (this._displayForenameFirst)
									if (this._isTooltip)
										return char.ToUpper(this._forename[0]) + " " + this._surname + " (" + this._holdingName + ")";
									else
										return char.ToUpper(this._forename[0]) + " " + this._surname + " <em>(" + this._holdingName + ")</em>";
								else
									if (this._isTooltip)
										return this._surname + ", " + char.ToUpper(this._forename[0]) + ". (" + this._holdingName + ")";
									else
										return this._surname + ", " + char.ToUpper(this._forename[0]) + ". <em>(" + this._holdingName + ")</em>";
						else
							if (this._displaySmallHoldingName)
								if (this._displayForenameFirst)
									if (this._isTooltip)
										return this._forename + " " + this._surname + " (" + this._holdingName + ")";
									else
										return this._forename + " " + this._surname + " <em style=\"font-size: 8pt;\">(" + this._holdingName + ")</em>";
								else
									if (this._isTooltip)
										return this._surname + ", " + this._forename + " (" + this._holdingName + ")";
									else
										return this._surname + ", " + this._forename + " <em style=\"font-size: 8pt;\">(" + this._holdingName + ")</em>";
							else
								if (this._displayForenameFirst)
									if (this._isTooltip)
										return  this._forename + " " + this._surname + " (" + this._holdingName + ")";
									else
										return this._forename + " " + this._surname + " <em>(" + this._holdingName + ")</em>";
								else
									if (this._isTooltip)
										return this._surname + ", " + this._forename + " (" + this._holdingName + ")";
									else
										return this._surname + ", " + this._forename + " <em>(" + this._holdingName + ")</em>";
		}
	}

}