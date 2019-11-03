var isSelectionEnabled = false;

function selectUniqueYeargroup() {
	isSelectionEnabled = true;
	switchAjaxUrl('Selection', '?path=/staff/modify/ajax/teaching/timetable/selection/uniqueyeargroup/search/');
	resetSearch('Selection');
	switchToTab('Selection');
}

function selectUniqueRoom() {
	isSelectionEnabled = true;
	switchAjaxUrl('Selection', '?path=/staff/modify/ajax/teaching/timetable/selection/uniqueroom/search/');
	resetSearch('Selection');
	switchToTab('Selection');
}

function selectClassSubject() {
	isSelectionEnabled = true;
	switchAjaxUrl('Selection', '?path=/staff/modify/ajax/teaching/timetable/selection/classsubject/search/');
	resetSearch('Selection');
	switchToTab('Selection');
}

function selectClassQualification() {
	var subjectId = document.getElementById("ClassSubjectId").getAttribute("value");
	
	if (subjectId == null || subjectId == undefined || subjectId == "")
		alert("Please select a subject before attempting to select a qualification.");
	else {
		isSelectionEnabled = true;
		switchAjaxUrl('Selection', '?path=/staff/modify/ajax/teaching/timetable/selection/classqualification/' + subjectId + '/search/');
		resetSearch('Selection');
		switchToTab('Selection');
	}
}

function selectClassClass() {
	var subjectId = document.getElementById("ClassSubjectId").getAttribute("value");
	var subjectQualificationId = document.getElementById("ClassSubjectQualificationId").getAttribute("value");
	
	if ((subjectId == null || subjectId == undefined || subjectId == "") || (subjectQualificationId == null || subjectQualificationId == undefined || subjectQualificationId == ""))
		alert("Please select both a subject and qualification level before attempting to select a class.");
	else {
		isSelectionEnabled = true;
		switchAjaxUrl('Selection', '?path=/staff/modify/ajax/teaching/timetable/selection/classclass/' + subjectQualificationId + '/search/');
		resetSearch('Selection');
		switchToTab('Selection');
	}
}

function selectClassRoom() {
	isSelectionEnabled = true;
	switchAjaxUrl('Selection', '?path=/staff/modify/ajax/teaching/timetable/selection/classroom/search/');
	resetSearch('Selection');
	switchToTab('Selection');
}

function setUniqueYeargroup(yeargroupName, yeargroupId) {	
	document.getElementById("UniqueYeargroupName").innerHTML = yeargroupName;
	document.getElementById("UniqueYeargroupId").setAttribute("value", yeargroupId);
	setFormTo('Teaching', 'Unique');
	completeAction();
}

function setUniqueRoom(roomName, roomId) {
	if (roomName == undefined && roomId == undefined) {
		document.getElementById("UniqueRoomName").innerHTML = "None";
		document.getElementById("UniqueRoomId").setAttribute("value", "");
	}
	else {
		document.getElementById("UniqueRoomName").innerHTML = roomName;
		document.getElementById("UniqueRoomId").setAttribute("value", roomId);
	}
	
	setFormTo('Teaching', 'Unique');
	completeAction();
}

function setClassSubject(subjectName, subjectId) {
	if (document.getElementById("ClassSubjectId").getAttribute("value") != subjectId) {
		document.getElementById("ClassQualificationName").innerHTML = "None";
		document.getElementById("ClassSubjectQualificationId").setAttribute("value", "");
		document.getElementById("ClassClassName").innerHTML = "None";
		document.getElementById("ClassClassId").setAttribute("value", "");
	}
	
	document.getElementById("ClassSubjectName").innerHTML = subjectName;
	document.getElementById("ClassSubjectId").setAttribute("value", subjectId);
	setFormTo('Teaching', 'Class');
	
	completeAction();
}

function setClassQualification(qualificationName, subjectQualificationId) {
	if (document.getElementById("ClassSubjectQualificationId").getAttribute("value") != subjectQualificationId) {
		document.getElementById("ClassClassName").innerHTML = "None";
		document.getElementById("ClassClassId").setAttribute("value", "");
	}
	
	document.getElementById("ClassQualificationName").innerHTML = qualificationName;
	document.getElementById("ClassSubjectQualificationId").setAttribute("value", subjectQualificationId);
	setFormTo('Teaching', 'Class');
	completeAction();
}

function setClassClass(className, classId) {
	document.getElementById("ClassClassName").innerHTML = className;
	document.getElementById("ClassClassId").setAttribute("value", classId);
	setFormTo('Teaching', 'Class');
	completeAction();
}

function setClassRoom(roomName, roomId) {
	if (roomName == undefined && roomId == undefined) {
		document.getElementById("ClassRoomName").innerHTML = "None";
		document.getElementById("ClassRoomId").setAttribute("value", "");
	}
	else {
		document.getElementById("ClassRoomName").innerHTML = roomName;
		document.getElementById("ClassRoomId").setAttribute("value", roomId);
	}
	
	setFormTo('Teaching', 'Class');
	completeAction();
}

function resetNonFields() {
	document.getElementById("UniqueYeargroupName").innerHTML = document.getElementById("UniqueYeargroupName-Original").getAttribute("value");
	document.getElementById("UniqueYeargroupId").setAttribute("value", document.getElementById("UniqueYeargroupId-Original").getAttribute("value"));
	
	document.getElementById("UniqueRoomName").innerHTML = document.getElementById("UniqueRoomName-Original").getAttribute("value");
	document.getElementById("UniqueRoomId").setAttribute("value", document.getElementById("UniqueRoomId-Original").getAttribute("value"));

	document.getElementById("ClassSubjectName").innerHTML = document.getElementById("ClassSubjectName-Original").getAttribute("value");
	document.getElementById("ClassSubjectId").setAttribute("value", document.getElementById("ClassSubjectId-Original").getAttribute("value"));
	
	document.getElementById("ClassQualificationName").innerHTML = document.getElementById("ClassQualificationName-Original").getAttribute("value");
	document.getElementById("ClassSubjectQualificationId").setAttribute("value", document.getElementById("ClassSubjectQualificationId-Original").getAttribute("value"));
	
	document.getElementById("ClassClassName").innerHTML = document.getElementById("ClassClassName-Original").getAttribute("value");
	document.getElementById("ClassClassId").setAttribute("value", document.getElementById("ClassClassId-Original").getAttribute("value"));
	
	document.getElementById("ClassRoomName").innerHTML = document.getElementById("ClassRoomName-Original").getAttribute("value");
	document.getElementById("ClassRoomId").setAttribute("value", document.getElementById("ClassRoomId-Original").getAttribute("value"));
}

function doSelectionSearch() {
	if (hasEnabledSelection())
		doSearch('Selection');
}

function resetSelectionSearch() {
	if (hasEnabledSelection())
		resetSearch('Selection');
}

function completeAction() {
	switchToTab('Teaching');
	switchAjaxUrl('Selection', '?path=/staff/modify/ajax/teaching/timetable/selection/success/search/');
	resetSearch('Selection');
	isSelectionEnabled = false;
}

function hasEnabledSelection() {
	if (!isSelectionEnabled) {
		alert('You must first select an option from the Teaching tab in order to enable this feature.');
		return false;
	}
	
	return true;
}
