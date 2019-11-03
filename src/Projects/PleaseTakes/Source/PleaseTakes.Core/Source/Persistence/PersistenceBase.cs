using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Persistence {

	internal abstract class PersistenceBase {
		private DateTime _creationDate;

		public PersistenceBase() {
			this._creationDate =DateTime.Now;
		}

		public DateTime CreationDate {
			get {
				return this._creationDate;
			}
		}
	}

}