var coverRequestId;
var coverYear;
var coverMonth;
var coverDay;
var coverIsInternal;
var coverZeroEntitlements;

function switchToSelection(coverId, year, month, day, isInternal) {
	coverRequestId = coverId;
	coverYear = year;
	coverMonth = month;
	coverDay = day;
	coverIsInternal = isInternal;
	
	if (coverZeroEntitlements == undefined)
		coverZeroEntitlements = true;
	
	doSelectionUpdate();
	resetSearch("Selection");
	switchToTab("Selection");
}

function doSelectionUpdate() {
	if (coverIsInternal)
		if (coverZeroEntitlements)
			switchAjaxUrl("Selection", "?path=/cover/arrange/selection/ajax/selection/internal/show/" + coverYear + "/" + coverMonth + "/" + coverDay + "/" + coverRequestId + "/search/");
		else
			switchAjaxUrl("Selection", "?path=/cover/arrange/selection/ajax/selection/internal/hide/" + coverYear + "/" + coverMonth + "/" + coverDay + "/" + coverRequestId + "/search/");
	else
		if (coverZeroEntitlements)
			switchAjaxUrl("Selection", "?path=/cover/arrange/selection/ajax/selection/outside/show/" + coverYear + "/" + coverMonth + "/" + coverDay + "/" + coverRequestId + "/search/");
		else
			switchAjaxUrl("Selection", "?path=/cover/arrange/selection/ajax/selection/outside/hide/" + coverYear + "/" + coverMonth + "/" + coverDay + "/" + coverRequestId + "/search/");
	
	doSearch("Selection");
}

function setStaffType() {
	if (isRequestSelected()) {
		if (coverIsInternal)
			coverIsInternal = false
		else
			coverIsInternal = true;
		
		doSelectionUpdate();
	}
}

function setZeroEntitlements() {
	if (isRequestSelected()) {
		if (coverZeroEntitlements)
			coverZeroEntitlements = false;
		else
			coverZeroEntitlements = true;
		
		doSelectionUpdate();
	}
}

function setSelection(staffId) {
	if (isRequestSelected()) {
		if (coverIsInternal)
			getResponse("Selection" + staffId, "?path=/cover/arrange/selection/ajax/selection/modify/" + coverYear + "/" + coverMonth + "/" + coverDay + "/internal/" + coverRequestId + "/" + staffId + "/", false, false, true);
		else
			getResponse("Selection" + staffId, "?path=/cover/arrange/selection/ajax/selection/modify/" + coverYear + "/" + coverMonth + "/" + coverDay + "/outside/" + coverRequestId + "/" + staffId + "/", false, false, true);	
	}
}

function resetSelection() {
	coverRequestId = undefined;
	coverYear = undefined;
	coverDay = undefined;
	coverIsInternal = undefined;
	coverZeroEntitlements = undefined;
	
	switchAjaxUrl("Selection", "?path=/cover/arrange/selection/ajax/selection/")
	resetSearch("Selection");
	switchToTab('Requests');
}

function isRequestSelected() {
	if (coverRequestId == undefined) {
		alert("You must first select a cover request from the Cover Requests tab in order to use this feature.");
		return false;
	}
	else
		return true;
}