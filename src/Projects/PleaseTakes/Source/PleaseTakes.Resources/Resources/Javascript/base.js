function addLoadEvent(inputFunc) {
	if (window.attachEvent)
		window.attachEvent("onload", inputFunc)
	else
		window.addEventListener("DOMContentLoaded", inputFunc, false);
}

function toggleAlert(id) {
	var element = document.getElementById("Alert" + id);
	
	if (element)
		if ((element.style.display == "") || (element.style.display == "block"))
			element.style.display = "none";
		else
			element.style.display = "block";
}

function parseBool(str) {
	switch (str.toLowerCase()) {
		case "true":
			return true;
		default:
			return false;
	}
}

function startsWith(str, lookFor) {
	if (str == undefined || str == null)
		return false;
	else {
		if (str.indexOf(lookFor) == 0)
			return true;
		else
			return false;
	}
}

function getPath() {
	var query = window.location.search.substring(1);
	var vars = query.split("&");
	
	for (var i = 0; i < vars.length; i++) {
		var pair = vars[i].split("=");
		
		if (pair[0] == "path")
			return pair[1];
	}
}