using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.BasicResponses.Login {

	internal static partial class Redirector {

		public static void Go(Helpers.Path.Parser path) {
			if (path.IsEmpty)
				new BasicResponses.Login.Handler(path);
			else
				if (path.Count.Equals(1))
					new BasicResponses.Login.Handler(path);
				else
					if (path.Peek().Equals("dologin"))
						BasicResponses.Login.Action.AttemptLogin();
					else
						new BasicResponses.Login.Handler(path);
		}

	}

}