using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools {

	public class SchoolNotFoundException : Exception {
		public SchoolNotFoundException(string message) : base(message) { }
	}

	public class InvalidSchoolCountException : Exception {
		public InvalidSchoolCountException(string message) : base(message) { }
	}

	public class MissingSchoolConfigurationSectionException : Exception {
		public MissingSchoolConfigurationSectionException(string message) : base(message) { }
	}

}