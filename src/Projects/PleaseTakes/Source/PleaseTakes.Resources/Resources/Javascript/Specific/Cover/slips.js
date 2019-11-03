var isAjaxDate;
var requestDate;
var dateElement;

function checkSourceDate() {
	dateElement = document.getElementById("AjaxCustomDateRequests");
	getResponse("AjaxCustomDateRequests", "?path=/cover/slips/ajax/isvalid/", false, true, true);
}

function setRequestsToAjaxDate(year, month, day) {
	isAjaxDate = true;
	requestDate = year + "/" + month + "/" + day + "/";
	switchAjaxUrl("Requests", "?path=/cover/slips/ajax/requests/" + requestDate + "/");
	dateElement.setAttribute("value", requestDate);
	doSearch("Requests");
	switchToTab("Requests");
}

function printSlips() {
	if (hasValidDate())
		if (isAjaxDate)
			alert("printajax: " + requestDate);
		else
			alert("printstandard: " + dateElement.getAttribute("value"));
}

function hasValidDate() {
	if (isAjaxDate)
		return true;
	else {
		var dateElementValue = dateElement.getAttribute("value")
		
		if (dateElement && dateElementValue != "")
			return dateElementValue;
	}
	
	alert("You must first select a date from the Date Selection tab before you can print cover slips.");
	switchToTab("Calendar");
	return false;
}

addLoadEvent(checkSourceDate);