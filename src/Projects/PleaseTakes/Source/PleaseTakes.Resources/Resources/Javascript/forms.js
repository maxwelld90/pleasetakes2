var _forms = new Array();

function getFormName(id) {
	return id.slice(4, id.length);
}

function initialiseForms() {
	var forms = document.getElementsByTagName("form");
	
	if (forms.length > 0) {
		
		for (var i = 0; i < forms.length; i++) {
			var formDetails = new Array();
			var formDivs = forms[i].getElementsByTagName("div");
			var groupPosition = 0;
			
			formDetails[0] = getFormName(forms[i].getAttribute("id"));
			formDetails[1] = new Array();
			
			for (var j = 0; j < formDivs.length; j++) {
				
				if (startsWith(formDivs[j].getAttribute("id"), "Form" + formDetails[0] + "-Group-")) {
					
					formDetails[1][groupPosition] = formDivs[j].getAttribute("id").slice(formDetails[0].length + 11, formDivs[j].getAttribute("id").length);
					modifyFormGroup(formDetails[0], formDetails[1][groupPosition], false);
					
					++groupPosition;
				}
				
			}
			
			_forms[i] = formDetails;
		}
	
	}
	
	initialiseRadios();
}

function updateOtherGroups(formName, groupName) {
	for (var i = 0; i < _forms.length; i++)
		if (_forms[i][0] == formName)
			for (var j = 0; j < _forms[i][1].length; j++)
				if (_forms[i][1][j] != groupName)
					modifyFormGroup(formName, _forms[i][1][j], false);
}

function modifyFormGroup(formName, groupName, modifyOthers) {
	var label = document.getElementById("Form" + formName + "-" + groupName + "-Label");
	var radio = document.getElementById("Form" + formName + "-" + groupName + "-Radio");
	var elements = document.getElementById("Form" + formName + "-" + groupName + "-Elements");
	
	if (label && radio) {
		
		if (modifyOthers)
			updateOtherGroups(formName, groupName);
		
		if (radio.checked)
			label.className = "Green";
		else
			label.className = "";
		
		if (elements) {
			var elementsArray = elements.getElementsByTagName("input");
			
			for (var i = 0; i < elementsArray.length; i++) {
				elementsArray[i].disabled = !radio.checked;
			}
		}
		
	}
}

function setFormTo(formName, groupName) {
	for (var i = 0; i < _forms.length; i++)
		if (_forms[i][0] == formName)
			for (var j = 0; j < _forms[i][1].length; j++) {
				if (_forms[i][1][j] == groupName)
					document.getElementById("Form" + formName + "-" + groupName + "-Radio").checked = true;
				
				modifyFormGroup(formName, groupName, true);
			}
}

function ajaxFormsUpdate(element, ajaxPath) {
	getUpdatedResponse(element, ajaxPath);
	setTimeout("recallFormInitialisation()", 100);
}

function recallFormInitialisation() {
	if (!ajaxInProgress)
		initialiseForms();
	else {
		setTimeout("recallFormInitialisation()" , 100);
	}
}



function switchRadio(formName, radioName, radioValue) {
	
	var label = document.getElementById("Form" + formName + "-Radio" + radioName + "-" + radioValue + "-Label");
	var radio = document.getElementById("Form" + formName + "-Radio" + radioName + "-" + radioValue);
	
	if (label && radio) {
		updateOtherRadios(formName, radioName);
		
		radio.checked = true;
		label.className = "Radio Green";
		label.style.fontWeight = "bold";
	}
	
}

function updateOtherRadios(formName, radioName) {
	var labels = document.getElementsByTagName("label");
	
	for (var i = 0; i < labels.length; i++)
		if (labels[i].id.indexOf("Form" + formName + "-Radio" + radioName) == 0) {
			labels[i].className = "Radio";
			labels[i].style.fontWeight = "";
		}
}

function initialiseRadios() {
	var labels = document.getElementsByTagName("label");
	
	for (var i = 0; i < _forms.length; i++) {
		var formName = _forms[i][0];
		
		for (var j = 0; j < labels.length; j++) {
			if (labels[j].id.indexOf("Form" + formName) == 0) {
				var startLength = 5 + formName.length;
				var sliceId = labels[j].id.slice(startLength, labels[j].id.length);
				
				if (sliceId.indexOf("Radio") == 0) {
					sliceId = labels[j].id.slice(startLength + 5, labels[j].id.length);
					var radioName = sliceId.slice(0, sliceId.indexOf("-"));
					sliceId = sliceId.slice(radioName.length + 1, sliceId.length);
					var radioValue = sliceId.slice(0, sliceId.indexOf("-"));
					
					var label = document.getElementById("Form" + formName + "-Radio" + radioName + "-" + radioValue + "-Label");
					
					if (document.getElementById("Form" + formName + "-Radio" + radioName + "-" + radioValue).checked) {
						label.className = "Radio Green";
						label.style.fontWeight = "bold";
					}
				}
			}
		}
	}
	
}

addLoadEvent(initialiseForms);