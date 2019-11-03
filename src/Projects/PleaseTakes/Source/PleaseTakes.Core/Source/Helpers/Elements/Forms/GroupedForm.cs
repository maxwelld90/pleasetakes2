using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements.Forms {

	internal sealed class GroupedForm : Form {
		private Dictionary<string, Group> _groups;
		private string _radioGroupName;

		public GroupedForm() {
			this._groups = new Dictionary<string, Group>();
		}

		public string RadioGroupName {
			get {
				return this._radioGroupName;
			}
			set {
				this._radioGroupName = value;
			}
		}

		public Group AddGroup(string groupName) {
			if (this._groups.ContainsKey(groupName))
				throw new GroupExistsException("The specified group '" + groupName + "' already exists; duplicate groups cannot be added.");
			else {
				Group newGroup = new Group(this, groupName);
				this._groups.Add(groupName, newGroup);

				return newGroup;
			}
		}

		public Group this[string groupName] {
			get {
				try {
					return this._groups[groupName];
				}
				catch (KeyNotFoundException) {
					throw new GroupDoesNotExistException("The specified group '" + groupName + "' does not currently exist.");
				}
			}
		}

		public override string ToString() {
			Constructor baseConstructor = new Constructor();
			baseConstructor.Contents = base.ToString();
			string groupsStr = "";

			if (!this._groups.Count.Equals(0))
				foreach (KeyValuePair<string, Group> groupPair in this._groups)
					groupsStr += groupPair.Value.ToString();

			baseConstructor.SetVariable("Contents", groupsStr);
			return baseConstructor.ToString();
		}

	}

	public class GroupExistsException : Exception {
		public GroupExistsException(string message) : base(message) { }
	}

	public class GroupDoesNotExistException : Exception {
		public GroupDoesNotExistException(string message) : base(message) { }
	}

}