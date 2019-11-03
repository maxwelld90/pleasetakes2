using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.Elements {

	internal abstract class CollectionBase<T> : IEnumerable<T> {
		private List<T> _collection;

		public CollectionBase() {
			this._collection = new List<T>();
		}

		protected List<T> Collection {
			get {
				return this._collection;
			}
		}

		public T this[int index] {
			get {
				return this.Collection[index];
			}
		}

		public int Count {
			get {
				return this._collection.Count;
			}
		}

		public bool IsEmpty {
			get {
				return (this._collection.Count.Equals(0));
			}
		}

		public void Add(T newElement) {
			this.Collection.Add(newElement);
		}

		protected void RemoveLast() {
			if (!this.IsEmpty)
				this.Collection.RemoveAt(this.Count - 1);
		}

		public IEnumerator<T> GetEnumerator() {
			foreach (T element in this._collection)
				yield return element;
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

	}

}