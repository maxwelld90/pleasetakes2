using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Schools {

	internal sealed class Collection {
		private Dictionary<string, School> _dictionary;

		public Collection() {
			this._dictionary = new Dictionary<string, School>();
			this.Populate();
		}

		public School this[string schoolID] {
			get {
				try {
					return this._dictionary[schoolID];
				}
				catch (KeyNotFoundException) {
					throw new SchoolNotFoundException("No school with ID " + schoolID + " was found in the current set of assemblies.");
				}
			}
		}

		public int Count {
			get {
				return this._dictionary.Count;
			}
		}

		public bool IsEmpty {
			get {
				return (this.Count == 0);
			}
		}

		public bool Exists(string schoolID) {
			return (this._dictionary.ContainsKey(schoolID.ToUpper()));
		}

		public School OnlySchool {
			get {
				if (this._dictionary.Count != 1)
					throw new InvalidSchoolCountException("The method called can only be used when one school is in the permitted list. There are currently " + this.Count + " schools in the list.");
				return this._dictionary.Values.First();
			}
		}

		private void Populate() {
			//Mearns Castle High School
			this._dictionary.Add("E2C0MWS7R", new School(
				"E2C0MWS7R",
				"MCHS",
				"Mearns Castle High School",
				"Scotland",
				"East Renfrewshire Council",
				new DateTime(2010, 06, 20)));
		}
	}

}