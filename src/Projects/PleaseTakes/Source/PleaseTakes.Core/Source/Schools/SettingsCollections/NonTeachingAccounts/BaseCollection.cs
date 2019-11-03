using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace PleaseTakes.Core.Schools.SettingsCollections.NonTeachingAccounts {

	internal sealed class BaseCollection : SchoolSettingsBase, IEnumerable {
		private List<UserManagement.Accounts.NonTeachingAccount> _accountsList;

		public BaseCollection(School school)
			: base(school, "/PleaseTakes.Schools/School[@id='" + school.SchoolID + "']/NonTeachingAccounts") {
			this._accountsList = new List<UserManagement.Accounts.NonTeachingAccount>();

			foreach(XmlNode accountNode in this.Parser.ChildNodes(this.XPath)) {
				if (this.Exists(accountNode.Attributes["username"].Value))
					throw new NonTeachingAccountDuplicateException("Account with username '" + accountNode.Attributes["username"].Value + "' already exists - duplicates are not permitted.");

				this._accountsList.Add(new UserManagement.Accounts.NonTeachingAccount(
					this,
					this.Parser,
					accountNode.Attributes["username"].Value,
					accountNode.Attributes["password"].Value,
					accountNode.Attributes["forename"].Value,
					accountNode.Attributes["surname"].Value,
					bool.Parse(accountNode.Attributes["active"].Value)));
			}

			this.Sort();
		}

		public UserManagement.Accounts.Account this[string username] {
			get {
				if (!this.Exists(username))
					throw new NonTeachingAccountNotFoundException("The non-teaching account with username '" + username + "' was not found.");

				return this._accountsList.Find(delegate(UserManagement.Accounts.NonTeachingAccount a) { return a.Username.Equals(username); });
			}
		}

		public int Count {
			get {
				return this._accountsList.Count;
			}
		}

		public bool IsEmpty {
			get {
				return (this.Count == 0);
			}
		}

		public bool Exists(string username) {
			return (this._accountsList.Exists(delegate(UserManagement.Accounts.NonTeachingAccount a) { return a.Username.Equals(username); }));
		}

		public string XPathToAccount(string username) {
			if (!this.Parser.NodeExists(this.XPath + "/Account[@username='" + username + "']"))
				throw new NonTeachingAccountNotFoundException("The non-teaching account with username '" + username + "' was not found.");

			return "/PleaseTakes.Schools/School[@id='" + this.School.SchoolID + "']/NonTeachingAccounts/Account[@username='" + username + "']";
		}

		public void Create(string username, string password, string forename, string surname, bool isActive) {
			if (this.Exists(username))
				throw new NonTeachingAccountDuplicateException("Cannot create non-teaching account with username '" + username + "'; an account with that username already exists.");

			this.Parser.CreateNode(this.XPath, "<Account username=\"" + WebServer.Server.HtmlEncode(username) + "\" password=\"" + WebServer.Server.HtmlEncode(password) + "\" forename=\"" + WebServer.Server.HtmlEncode(forename) + "\" surname=\"" + WebServer.Server.HtmlEncode(surname) + "\" active=\"" + isActive.ToString().ToLower() + "\" />");
			this.Parser.Save();
			this._accountsList.Add(new UserManagement.Accounts.NonTeachingAccount(
				this,
				this.Parser,
				username,
				password,
				forename,
				surname,
				isActive));

			this.Sort();
		}

		public void Remove(string username) {
			if (!this.Exists(username))
				throw new NonTeachingAccountNotFoundException("The non-teaching account with username '" + username + "' was not found.");

			this._accountsList.RemoveAll(delegate(UserManagement.Accounts.NonTeachingAccount a) { return a.Username.Equals(username); });
			this.Parser.RemoveNode(this.XPathToAccount(username));
			this.Parser.Save();
		}

		private void Sort() {
			this._accountsList.Sort(delegate(UserManagement.Accounts.NonTeachingAccount account1, UserManagement.Accounts.NonTeachingAccount account2) { return account1.Surname.CompareTo(account2.Surname); });
		}

		public List<UserManagement.Accounts.NonTeachingAccount> Search(string searchTerm) {
			var searchedList = (from UserManagement.Accounts.NonTeachingAccount account in this._accountsList
								where account.Forename.ToLower().Contains(searchTerm) || account.Surname.ToLower().Contains(searchTerm)
								select account);

			return searchedList.ToList();
		}

		public IEnumerator GetEnumerator() {
			foreach (UserManagement.Accounts.NonTeachingAccount account in this._accountsList)
				yield return account;
		}
	}

	public class NonTeachingAccountDuplicateException : Exception {
		public NonTeachingAccountDuplicateException(string message) : base(message) { }
	}

	public class NonTeachingAccountNotFoundException : Exception {
		public NonTeachingAccountNotFoundException(string message) : base(message) { }
	}

}