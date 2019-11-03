using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal interface IRowAdders {

		void SetToTextField(string name, string id, string defaultValue, int tabIndex, int maxLength, bool isPassword, bool isItalic);

		void SetToRadioGroup();

	}

}