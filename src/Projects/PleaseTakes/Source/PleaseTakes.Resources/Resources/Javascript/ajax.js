var ajaxInProgress = false;

var xmlHttpFactories = [
	function() { return new XMLHttpRequest() },
	function() { return new ActiveXObject("Msxml2.XMLHTTP") },
	function() { return new ActiveXObject("Msxml3.XMLHTTP") },
	function() { return new ActiveXObject("Microsoft.XMLHTTP") }
];

function initialiseAjax() {
	var divs = document.getElementsByTagName("div");
	var ajaxElementNames = [];
	
	for (var i = 0, length = divs.length, div, divName; i < length; i++) {
		divName = divs[i].getAttribute("id");
		
		if (divName && (divName.indexOf("AjaxContainer") == 0)) {
			var name = divName.replace("AjaxContainer", "");
			getInitialResponse(name);
		}
	}
}

function getInitialResponse(element) {
	getResponse("AjaxContent" + element, document.getElementById("AjaxUrl" + element).getAttribute("value"), parseBool(document.getElementById("AjaxShowLoadingMessage" + element).getAttribute("value")), parseBool(document.getElementById("AjaxGetPath" + element).getAttribute("value")), false);
}

function getUpdatedResponse(element, newUrl) {
	getResponse("AjaxContent" + element, newUrl, parseBool(document.getElementById("AjaxShowLoadingMessage" + element).getAttribute("value")), parseBool(document.getElementById("AjaxGetPath" + element).getAttribute("value")), false);
}

function createXmlHttpObject() {
	var xmlHttp = false;
	
	for (var i = 0; i < xmlHttpFactories.length; i++) {
		try {
			xmlHttp = xmlHttpFactories[i]();
		}
		catch(e) {
			continue;
		}
		break;
	}
	
	return xmlHttp;
}

function getResponse(element, url, showLoadingMessage, getPagePath, parseXml) {
	var xmlHttp = createXmlHttpObject();
	
	if (!xmlHttp) {
		alert("PleaseTakes cannot complete the current request.\nA browser supporting AJAX is required.");
		return;
	}
	
	if (getPagePath)
		xmlHttp.open("GET", url + "&sourcePath=" + getPath() + "&ajaxSid=" + Math.random(), true);
	else
		xmlHttp.open("GET", url + "&ajaxSid=" + Math.random(), true);
	
	xmlHttp.onreadystatechange = function() {
		if (xmlHttp.readyState < 4) {
			ajaxInProgress = true;
			document.getElementById("AjaxIndicator").style.display = "block";
			
			if (showLoadingMessage && !parseXml)
				document.getElementById(element).innerHTML = "<div class=\"Alert Yellow\"><h4>Loading Content...</h4>Hold on to your hats!</div>";
		}
		else {
			if (parseXml) {
				var xmlDoc = xmlHttp.responseXML;
				var elements = xmlDoc.getElementsByTagName("ResponseRoot").item(0).getElementsByTagName("Element");
				
				for (var nodeCounter = 0; nodeCounter < elements.length; nodeCounter++) {
					var node = elements[nodeCounter];
					var id = node.getAttribute("id");
					var valueAttr = node.getAttribute("value");
					var className = node.getAttribute("className");
					var innerText;
					
					if (document.all)
						innerText = node.text;
					else
						innerText = node.textContent;
					
					if ((id == undefined) || (id == null))
						id = element;
					
					if ((valueAttr != undefined) && (valueAttr != "") && (document.getElementById(id)))
						document.getElementById(id).setAttribute("value", valueAttr);
					
					if ((innerText != undefined) && (innerText != "") && (document.getElementById(id)))
						document.getElementById(id).innerHTML = innerText;
					
					if (document.getElementById(id))
						document.getElementById(id).className = className;
				}
			}
			else
				document.getElementById(element).innerHTML = xmlHttp.responseText;
				document.getElementById("AjaxIndicator").style.display = "none";
				ajaxInProgress = false;
			}
	}
	
	if (xmlHttp.readyState == 4)
		return;
	
	xmlHttp.send(null);
}

addLoadEvent(initialiseAjax);