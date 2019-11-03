using System;
using System.Collections.Generic;
using PleaseTakes.Core.Validation;

namespace PleaseTakes.Core.Helpers.Path {

	internal sealed class Parser {
		private List<string> _list;
		private int _index = 0;

		public Parser(string path) {
			if ((path == null) || (path.Length < 1))
				this._list = null;
			else {
				path = Parser.FormatPath(path);

				if (path == null)
					this._list = null;
				else
					this._list = new List<string>(path.Split('/'));
			}
		}

		private static string FormatPath(string path) {
			path = path.ToLower();
			path = path.Replace('\\', '/');
			path = path.Trim();
			path = path.TrimStart('/');
			path = path.TrimEnd('/');

			if (path.Length == 0)
				return null;
			else {
				char prev = path[0];
				string returnStr = "";

				foreach (char c in path) {
					if (c != '/')
						returnStr += c;
					else if (prev != c)
						returnStr += c;

					prev = c;
				}

				return returnStr;
			}
		}

		public bool IsEmpty {
			get {
				if ((this._list == null) || (this._list.Count < 1))
					return true;
				return false;
			}
		}

		public int Count {
			get {
				this._list.RequireThat("path").IsNotNull();
				return this._list.Count;
			}
		}

		public bool IsAjax {
			get {
				try {
					return this.ToString().Contains("ajax");
				}
				catch (NullReferenceException) {
					return false;
				}
			}
		}

		public bool HasNext() {
			if (this._list == null)
				return false;
			return (this._index < this._list.Count);
		}

		public string Next() {
			if (!this.HasNext())
				throw new IndexOutOfRangeException(this.ToString());
			return this._list[this._index++];
		}

		public string Peek() {
			return this._list[this._index];
		}

		public void Reset() {
			this._index = 0;
		}

		public void RemoveAt(int index) {
			if (this.IsEmpty)
				throw new IndexOutOfRangeException();
			else {
				index.RequireThat("path index").IsInRange(0, (this.Count - 1));
				this._list.RemoveAt(index);
			}
		}

		public string this[int index] {
			get {
				return this._list[index];
			}
		}

		public bool Contains(string searchFor) {
			return (this.ToString().Contains(searchFor));
		}

		public override string ToString() {
			if (this.IsEmpty)
				throw new NullReferenceException();
			else {
				string returnStr = "/";

				foreach (string s in this._list)
					returnStr += s + "/";

				returnStr = returnStr.TrimEnd('/');
				return returnStr;
			}
		}
	}

}