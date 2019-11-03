using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Validation {

	internal class ArgumentWrapper<T> {
		private T _value;
		private string _name;

		public ArgumentWrapper(T value, string name) {
			this._value = value;
			this._name = name;
		}

		public ArgumentWrapper(T value) {
			this._value = value;
		}

		public T Value {
			get {
				return this._value;
			}
			set {
				this._value = value;
			}
		}

		public string Name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		public static implicit operator T(ArgumentWrapper<T> argument) {
			return argument.Value;
		}
	}

}