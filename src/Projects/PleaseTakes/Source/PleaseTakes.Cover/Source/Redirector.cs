using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PleaseTakes.Cover {

	internal static class Redirector {

		public static void Go(Core.Helpers.Path.Parser path) {
			if (path.HasNext()) {
				switch (path.Next()) {
					case "arrange":
						if (path.HasNext())
							switch (path.Next()) {
								case "calendar":
									if (path.HasNext())
										if (path.Next().Equals("ajax"))
											new Handlers.Arrange.Calendar.Ajax.Standard(path);
										else
											new Handlers.Arrange.Calendar.Standard(path);
									else
										new Handlers.Arrange.Calendar.Standard(path);
									break;
								case "attendance":
									if (path.HasNext())
										if (path.Next().Equals("ajax"))
											if (path.HasNext())
												switch (path.Next()) {
													case "attendance":
														if (path.HasNext())
															switch (path.Next()) {
																case "modify":
																	new Handlers.Arrange.Attendance.Ajax.Attendance.Modify(path);
																	break;
																case "status":
																	new Handlers.Arrange.Attendance.Ajax.Attendance.Status(path);
																	break;
																default:
																	new Handlers.Arrange.Attendance.Ajax.Attendance.Standard(path);
																	break;
															}
														else
															new Handlers.Arrange.Attendance.Ajax.Attendance.Standard(path);
															break;
													case "periods":
														if (path.HasNext())
															switch (path.Next()) {
																case "modify":
																	new Handlers.Arrange.Attendance.Ajax.Periods.Modify(path);
																	break;
																case "status":
																	new Handlers.Arrange.Attendance.Ajax.Periods.Status(path);
																	break;
																default:
																	new Handlers.Arrange.Attendance.Ajax.Periods.Standard(path);
																	break;
															}
														else
															new Handlers.Arrange.Attendance.Ajax.Periods.Standard(path);
														break;
													default:
														new Handlers.Arrange.Attendance.Standard(path);
														break;
												}
											else
												new Handlers.Arrange.Attendance.Standard(path);
										else
											new Handlers.Arrange.Attendance.Standard(path);
									else
										Core.WebServer.PleaseTakes.Redirect("/cover/arrange/calendar/invalid/");
									break;
								case "selection":
									if (path.HasNext())
										if (path.Next().Equals("ajax"))
											if (path.HasNext())
												switch (path.Next()) {
													case "requests":
														if (path.HasNext())
															switch (path.Next()) {
																case "status":
																	new Handlers.Arrange.Selection.Ajax.Requests.Status(path);
																	break;
																default:
																	new Handlers.Arrange.Selection.Ajax.Requests.Standard(path);
																	break;
															}
														else
															new Handlers.Arrange.Selection.Ajax.Requests.Standard(path);
														break;
													case "selection":
														if (path.HasNext())
															switch (path.Peek()) {
																case "status":
																	new Handlers.Arrange.Selection.Ajax.Selection.Status(path);
																	break;
																case "modify":
																	new Handlers.Arrange.Selection.Ajax.Selection.Modify(path);
																	break;
																default:
																	new Handlers.Arrange.Selection.Ajax.Selection.Standard(path);
																	break;
															}
														else
															new Handlers.Arrange.Selection.Ajax.Selection.Standard(path);
														break;
												}
											else
												new Handlers.Arrange.Selection.Standard(path);
										else
											new Handlers.Arrange.Selection.Standard(path);
									else
										new Handlers.Arrange.Selection.Standard(path);
									break;
								default:
									new Handlers.Arrange.Landing(path);
									break;
							}
						else
							new Handlers.Arrange.Landing(path);
						break;
					case "slips":
						Slips.Redirector.Go(path);
						break;
					default:
						new Handlers.Menu.Handler(path);
						break;
				}
			}
			else
				new Handlers.Menu.Handler(path);
		}

	}

}