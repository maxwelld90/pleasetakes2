function focusForm() {
	document.forms['LoginForm'].elements['Username'].focus();
}

function toLogin() {
	switchToTab('Login');
	setTimeout('focusForm()', 500);
}

addLoadEvent(focusForm);