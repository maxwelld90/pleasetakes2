using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Login {

	internal sealed class UpdateRecord {
		private Helpers.Elements.RecordLists.Record _record;
		private List<string> _assemblies;

		public UpdateRecord() {
			this._record = new Helpers.Elements.RecordLists.Record();
			this._assemblies = new List<string>();

			this._record.LeftSide.Image.Source = "news.png";
			this._record.LeftSide.Image.ToolTip = "Recent update entry";
			this._record.LeftSide.SmallText = "{$Assemblies}{$Description}";
		}

		public string Build {
			set {
				this._record.LeftSide.MainText = "<strong>Build " + value + "</strong>";
			}
		}

		public void AddAssembly(string assemblyName) {
			this._assemblies.Add(assemblyName);
		}

		public string Description {
			set {
				this._record.LeftSide.SmallText = this._record.LeftSide.SmallText.Replace("{$Description}", value);
			}
		}

		public string Href {
			set {
				this._record.Href = "?path=/resources/updates/" + value;
				this._record.RightSide.Image.Source = "report.png";
				this._record.RightSide.Image.ToolTip = "A PDF file containing more information on this update is available; click to download it.";
				this._record.RightSide.MainText = "Further information available";
				this._record.RightSide.SmallText = "Click to view PDF file";
			}
		}

		public bool IsImportant {
			set {
				if (value)
					this._record.Colour = Helpers.Elements.RecordLists.Colours.Yellow;
				else
					this._record.Colour = Helpers.Elements.RecordLists.Colours.Blue;
			}
		}

		public Helpers.Elements.RecordLists.Record Record {
			get {
				this._record.LeftSide.SmallText = this._record.LeftSide.SmallText.Replace("{$Description}", "");

				if (this._assemblies.Count == 0)
					this._record.LeftSide.SmallText = this._record.LeftSide.SmallText.Replace("{$Assemblies}", "");
				else {
					string assemblyStr = "";
					char[] trimArray = { ',', ' ' };

					foreach (string assembly in this._assemblies)
						assemblyStr += assembly + ", ";

					assemblyStr = assemblyStr.TrimEnd(trimArray);
					this._record.LeftSide.SmallText = this._record.LeftSide.SmallText.Replace("{$Assemblies}", "<strong>Assemblies: </strong>" + assemblyStr + "<br />");
				}

				return this._record;
			}
		}
	}

}