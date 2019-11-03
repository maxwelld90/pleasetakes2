var _tabLinks = new Array();
var _tabContentDivs = new Array();

function initialiseTabs() {
	
	if (document.getElementById("TabsList")) {
		var i = 0;
		var tabListItems = document.getElementById("TabsList").childNodes;
		var currHash = window.location.hash;
		var initWith = 0;
		var item;
		
		for (i = 0; i < tabListItems.length; i++)

			if (tabListItems[i].nodeName == "LI") {
				var tabLink = getFirstChildElement( tabListItems[i], "A" );
				var id = getAfterHash( tabLink.getAttribute("href") );

				if (tabLink.getAttribute("href").indexOf("#") != -1) {
					_tabLinks[id] = tabLink;
					_tabContentDivs[id] = document.getElementById( id );
				}
			}
		
		i = 0;

		for (item in _tabLinks) {
			if (currHash.replace("#", "") == item)
				initWith = i;
			i++;
		}
		
		i = 0;
		
		for (item in _tabLinks) {
			_tabLinks[item].onclick = showTab;
			_tabLinks[item].onfocus = function() { this.blur() };
			
			if (i == initWith)
				_tabLinks[item].className = "Selected";
			i++;
		}
		
		i = 0;
		
		for (item in _tabContentDivs) {
			if (i != initWith)
				_tabContentDivs[item].className = "TabContent Hide";
			i++;
		}
	}
}

function showTab() {
	var selectedId = getAfterHash(this.getAttribute("href"));
	var id;
	
	window.location.hash = "#" + selectedId;
	
	for (id in _tabContentDivs) {
		if (id == selectedId) {
			_tabLinks[id].className = "Selected";
			_tabContentDivs[id].className = "TabContent";
		}
		else {
			_tabLinks[id].className = "";
			_tabContentDivs[id].className = "TabContent Hide";
		}
	}
	
	return false;
}

function getFirstChildElement(element, tagName) {
	var i = 0;
	
	for (i = 0; i < element.childNodes.length; i++)
		if (element.childNodes[i].nodeName == tagName)
			return element.childNodes[i];
}

function getAfterHash(url) {
	var hashPos = url.lastIndexOf("#");
	return url.substring(hashPos + 1);
}

function switchToTab(toWhat) {
	if (document.getElementById(toWhat)) {
		var i = 0;
		var item;
		
		window.location.hash = "#" + toWhat;
		
		for (item in _tabLinks) {
			_tabLinks[item].onclick = showTab;
			_tabLinks[item].onfocus = function() { this.blur() };
			
			_tabLinks[item].className = "";
			
			if (item == toWhat)
				_tabLinks[item].className = "Selected";
			i++;
		}
		
		i = 0;
		
		for (item in _tabContentDivs) {
			if (item == toWhat) {
				_tabLinks[item].className = "Selected";
				_tabContentDivs[item].className = "TabContent";
			}
			else
				_tabContentDivs[item].className = "TabContent Hide";
			i++;
		}
	}
}

addLoadEvent(initialiseTabs);