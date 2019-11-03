using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Core.Helpers.BaseHandlers {

	internal abstract class StandardHandler : BaseHandler {
		private Elements.HeaderTags.Collection _headerTags;
		private Elements.Tabs.Collection _tabs;
		private bool _containsAjax;
		private bool _containsSearch;
		private bool _containsPrintContent;
		private bool _containsForm;

		public StandardHandler(Path.Parser path, bool useRightMenu, bool containsAjax, bool containsSearch, bool containsPrintContent, bool containsForm)
			: base(TemplateTypes.Base, path) {
			this._headerTags = new Elements.HeaderTags.Collection();
			this._tabs = new Elements.Tabs.Collection();
			this._containsAjax = containsAjax;
			this._containsSearch = containsSearch;
			this._containsPrintContent = containsPrintContent;
			this._containsForm = containsForm;

			this.InitialChecks();

			this.SetPageLayout(useRightMenu);

			this.SetCommonHeaderTags();
			this.SetHeaderTags();
			this.SetPageTitle();
			this.ApplyHeaderTags();
			
			this.SetTabs();

			this.SetAlerts();
			this.SetBreadcrumbTrails();
			this.SetMenu90();
			this.SetTabSpecific();

			this.ApplyTabs();
			this.ApplySchoolName();
			this.SetTopRight();

			this.SpecificCommands();

			if (this._containsPrintContent)
				this.Page.SetVariable("Printable", new Constructor("/Templates/printable.html"));
			else
				this.Page.DeleteVariable("Printable");
		}

		protected abstract void InitialChecks();

		private void SetPageLayout(bool useRightMenu) {
			if (useRightMenu) {
				this.Page.SetVariable("PageContent", new Constructor("/Templates/menu.html"));
				this.ApplyRightMenu();
			}
			else
				this.Page.SetVariable("PageContent", new Constructor("/Templates/plain.html"));
		}

		private void ApplyRightMenu() {
			Elements.RightMenu.Collection collection = new Elements.RightMenu.Collection();
			collection.Add("?path=/home/", "home.png", "Click here to jump to PleaseTakes Home.", "PleaseTakes Home");
			
			if (WebServer.PleaseTakes.Session.CurrentInstance.Account.IsAdmin) {
				collection.Add("?path=/cover/arrange/", "arrange.png", "Click here to arrange cover for absent members of teaching staff.", "Arrange Staff Cover");
				collection.Add("?path=/cover/slips/", "printpaper.png", "Click here to print cover slips.", "Print Cover Slips");
				collection.Add("?path=/staff/", "staff.png", "Click here to add, delete or modify staff records, as well as their individual timetables.", "Staff Management");
			}
			else {
				// Special base-only links
			}

			collection.Add("?path=/logout/", "logout.png", "Click here to logout from PleaseTakes.", "Logout");
			this.Page.SetVariable("RightMenu", collection.ToString());
		}

		protected abstract void SetHeaderTags();

		protected abstract void SetTabs();

		protected abstract void SetAlerts();

		protected abstract void SetBreadcrumbTrails();

		protected abstract void SetTabSpecific();

		protected abstract void SetMenu90();

		protected abstract void SpecificCommands();

		protected Elements.HeaderTags.Collection HeaderTags {
			get {
				return this._headerTags;
			}
		}

		protected Elements.Tabs.Collection Tabs {
			get {
				return this._tabs;
			}
		}

		private void ApplyHeaderTags() {
			this.Page.SetVariable("HeaderTags", this._headerTags.ToString());
		}

		private void ApplyTabs() {
			if (this._tabs.IsEmpty) {
				this.Page.DeleteVariable("Tabs");
				this.Page.DeleteVariable("TabContents");
			}
			else {
				this.Page.SetVariable("Tabs", this._tabs.TabList);
				this.Page.SetVariable("TabContents", this._tabs.TabContents);
			}
		}

		private void SetCommonHeaderTags() {
			Elements.HeaderTags.Meta emulateIe = new Elements.HeaderTags.Meta();
			emulateIe.HttpEquiv = "X-UA-Compatible";
			emulateIe.Content = "IE=EmulateIE7";

			Elements.HeaderTags.Meta content = new Elements.HeaderTags.Meta();
			content.HttpEquiv = "Content-Type";
			content.Content = "text/html; charset=UTF-8";

			Elements.HeaderTags.Stylesheet baseCss = new Elements.HeaderTags.Stylesheet();
			baseCss.Source = "?path=/resources/stylesheets/base.css";
			baseCss.Media = "screen";

			Elements.HeaderTags.Stylesheet ieCss = new Elements.HeaderTags.Stylesheet();
			ieCss.Conditional = Elements.HeaderTags.Conditionals.Ie;
			ieCss.Source = "?path=/resources/stylesheets/ie/base.css";
			ieCss.Media = "screen";

			Elements.HeaderTags.Stylesheet lteIe6Css = new Elements.HeaderTags.Stylesheet();
			lteIe6Css.Conditional = Elements.HeaderTags.Conditionals.LteIe6;
			lteIe6Css.Source = "?path=/resources/stylesheets/ie/lteie6.css";
			lteIe6Css.Media = "screen";

			Elements.HeaderTags.Stylesheet gteIe7Css = new Elements.HeaderTags.Stylesheet();
			gteIe7Css.Conditional = Elements.HeaderTags.Conditionals.GteIe7;
			gteIe7Css.Source = "?path=/resources/stylesheets/ie/lteie7.css";
			gteIe7Css.Media = "screen";

			Elements.HeaderTags.Stylesheet printStylesheet = new Elements.HeaderTags.Stylesheet();
			printStylesheet.Conditional = Elements.HeaderTags.Conditionals.None;
			printStylesheet.Media = "screen,print";
			printStylesheet.Source = "?path=/resources/stylesheets/printable.css";

			Elements.HeaderTags.Script baseScript = new Elements.HeaderTags.Script();
			baseScript.Source = "?path=/resources/javascript/base.js";

			Elements.HeaderTags.Script tagsScript = new Elements.HeaderTags.Script();
			tagsScript.Source = "?path=/resources/javascript/tabs.js";

			Elements.HeaderTags.Script ajaxScript = new Elements.HeaderTags.Script();
			ajaxScript.Source = "?path=/resources/javascript/ajax.js";

			Elements.HeaderTags.Script searchScript = new Elements.HeaderTags.Script();
			searchScript.Source = "?path=/resources/javascript/search.js";

			Elements.HeaderTags.Script printScript = new Elements.HeaderTags.Script();
			printScript.Source = "?path=/resources/javascript/printable.js";

			Elements.HeaderTags.Script formsScript = new Elements.HeaderTags.Script();
			formsScript.Source = "?path=/resources/javascript/forms.js";

			this.HeaderTags.Add(emulateIe);
			this.HeaderTags.Add(content);
			this.HeaderTags.Add(baseCss);
			this.HeaderTags.Add(ieCss);
			this.HeaderTags.Add(lteIe6Css);
			this.HeaderTags.Add(gteIe7Css);
			this.HeaderTags.Add(baseScript);
			this.HeaderTags.Add(tagsScript);

			if (this._containsAjax || this._containsSearch)
				this.HeaderTags.Add(ajaxScript);

			if (this._containsSearch)
				this.HeaderTags.Add(searchScript);

			if (this._containsPrintContent) {
				this.HeaderTags.Add(printStylesheet);
				this.HeaderTags.Add(printScript);
			}

			if (this._containsForm)
				this.HeaderTags.Add(formsScript);
		}

		private void SetPageTitle() {
			Elements.HeaderTags.Title titleTag;

			if (WebServer.PleaseTakes.Session.CurrentInstance == null)
				titleTag = new Elements.HeaderTags.Title(Consts.WebPageTitlePrefix);
			else
				if (WebServer.PleaseTakes.Session.CurrentInstance.SessionSchoolExists)
					titleTag = new Elements.HeaderTags.Title(Consts.WebPageTitlePrefix + " :: " + WebServer.PleaseTakes.Session.CurrentInstance.School.FullName);
				else
					titleTag = new Elements.HeaderTags.Title(Consts.WebPageTitlePrefix);

			this.HeaderTags.Add(titleTag);
		}

		protected static string GetAlertMessage(string path) {
			return new Constructor(path).ToString();
		}

		protected void ApplySchoolName() {
			if (WebServer.PleaseTakes.Session.CurrentInstance == null)
				this.Page.DeleteVariable("SchoolName");
			else
				if (WebServer.PleaseTakes.Session.CurrentInstance.SessionSchoolExists) {
					Constructor template = new Constructor("/Templates/Elements/schoolname.html");
					template.SetVariable("SchoolName", WebServer.PleaseTakes.Session.CurrentInstance.School.FullName);
					this.Page.SetVariable("SchoolName", template.ToString());
				}
				else
					this.Page.DeleteVariable("SchoolName");
		}

		protected void SetTopRight() {
			Helpers.Elements.TopRight.Area topRightArea;

			if (WebServer.PleaseTakes.Session.CurrentInstance == null)
				topRightArea = new Helpers.Elements.TopRight.Area(Helpers.Elements.TopRight.Types.None);
			else {
				if (WebServer.PleaseTakes.Session.CurrentInstance.SessionSchoolExists)
					if (WebServer.PleaseTakes.Session.CurrentInstance.IsLoggedIn)
						topRightArea = new Helpers.Elements.TopRight.Area(Helpers.Elements.TopRight.Types.Standard);
					else
						topRightArea = new Helpers.Elements.TopRight.Area(Helpers.Elements.TopRight.Types.Login);
				else
					topRightArea = new Helpers.Elements.TopRight.Area(Helpers.Elements.TopRight.Types.None);
			}

			this.Page.SetVariable("TopRight", topRightArea.ToString());
		}
	}

}
