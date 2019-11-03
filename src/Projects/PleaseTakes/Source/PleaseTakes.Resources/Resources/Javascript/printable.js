function doPrintout() {
	print();
	history.back(-1);
}

addLoadEvent(doPrintout);