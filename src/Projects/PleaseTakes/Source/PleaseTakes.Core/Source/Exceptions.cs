using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core {

	public class PleaseTakesInitialisationException : Exception {
		public PleaseTakesInitialisationException(string message)
			: base(message) {
			WebServer.PleaseTakes.Application.ScrubInstance();
		}
	}

	public class TEMPEXCEPTION : Exception {
		public TEMPEXCEPTION(string message) : base(message) { }
	}

}