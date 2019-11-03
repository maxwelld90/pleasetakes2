function initialiseSearches() {
	var divs = document.getElementsByTagName("div");
	var searchNames = [];
	
	for (var i = 0, length = divs.length, div, divName; i < length; i++) {
		divName = divs[i].getAttribute("id");
		
		if (divName && (divName.indexOf("SearchContainer") == 0)) {
			var name = divName.replace("SearchContainer", "");
			
			searchNames.push(name);
			doSearch(name);
		}
		
	}
}

function doSearch(searchName) {
	var ajaxUrl = document.getElementById("AjaxUrl" + searchName).getAttribute("value");
	var ajaxStatusUrl = document.getElementById("AjaxStatusUrl" + searchName).getAttribute("value");
	var ajaxGetSourcePath = parseBool(document.getElementById("AjaxGetSourcePath" + searchName).value);
	var searchValue = document.getElementById("SearchInput" + searchName).value;
	
	doResultsUpdate(searchName, ajaxUrl, searchValue, ajaxGetSourcePath);
	doStatusUpdate(searchName, ajaxStatusUrl, searchValue);
}

function doResultsUpdate(searchName, ajaxUrl, searchValue, ajaxGetSourcePath) {
	getResponse("SearchResults" + searchName, ajaxUrl + "/" + searchValue, true, ajaxGetSourcePath, false);
}

function doStatusUpdate(searchName, ajaxStatusUrl, searchValue) {
	if (!ajaxInProgress)
		getResponse("SearchStatus" + searchName, ajaxStatusUrl + "/" + searchValue, false, false, false);
	else
		setTimeout("doStatusUpdate(\"" + searchName + "\", \"" + ajaxStatusUrl + "\", \"" + searchValue.replace(/\"/g, "") + "\")", 100);
}

function switchAjaxUrl(searchName, newAjaxUrl) {
	document.getElementById("AjaxUrl" + searchName).setAttribute("value", newAjaxUrl);
}

function resetSearch(searchName) {
	document.getElementById("SearchInput" + searchName).value = "";
	doSearch(searchName);
}

function checkForKeyPress(e, searchName) {
	var characterCode;
	
	if (e && e.which) {
		e = e;
		characterCode = e.which;
	}
	else {
		e = event;
		characterCode = e.keyCode;
	}
	
	if (characterCode == 13) {
		doSearch(searchName);
		return false;
	}
	else
		return true;
}

addLoadEvent(initialiseSearches);