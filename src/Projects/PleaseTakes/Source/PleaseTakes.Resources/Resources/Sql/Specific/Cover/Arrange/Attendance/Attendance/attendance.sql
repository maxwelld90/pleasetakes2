SELECT DISTINCT
	StaffList.StaffId,
	StaffNames.Forename,
	StaffNames.Surname,
	StaffHoldingNames.Name AS HoldingName,
	Attendance.CountValue AS AttendanceRecord
FROM
	Staff AS StaffList
	LEFT JOIN(
			SELECT
				Staff.StaffId,
				COUNT(TimetableUnavailability.Id) AS CountValue
			FROM
				Staff
				LEFT JOIN
					TimetableUnavailability
					ON Staff.StaffId = TimetableUnavailability.StaffId
					AND TimetableUnavailability.CurrentSession = 1
					AND TimetableUnavailability.WeekNo = @WeekNo
					AND TimetableUnavailability.DayNo = @DayNo
					AND TimetableUnavailability.Period BETWEEN 1 AND @MaxPeriods
			GROUP BY
				Staff.StaffId) AS Unavailability
		ON StaffList.StaffId = Unavailability.StaffId
	LEFT JOIN(
			SELECT
				Staff.StaffId,
				COUNT(Attendance.AttendanceId) AS CountValue
			FROM
				Staff
				LEFT JOIN
					AttendanceStaff
					ON Staff.StaffId = AttendanceStaff.StaffId
				LEFT JOIN
					Attendance
					ON AttendanceStaff.AttendanceId = Attendance.AttendanceId
					AND Attendance.Date = @Date
			GROUP BY
				Staff.StaffId) AS Attendance
		ON StaffList.StaffId = Attendance.StaffId
	LEFT JOIN StaffNames
		ON StaffList.StaffId = StaffNames.StaffId
	LEFT JOIN StaffHoldingNames
		ON StaffList.StaffId = StaffHoldingNames.StaffId
	LEFT JOIN DepartmentsStaff
		ON StaffList.StaffId = DepartmentsStaff.StaffId
	LEFT JOIN Departments
		ON DepartmentsStaff.DepartmentId = Departments.DepartmentId
WHERE
	(Unavailability.CountValue < @MaxPeriods) AND
	(StaffNames.Forename LIKE '%' + @SearchTerm + '%' OR
	StaffNames.Surname LIKE '%' + @SearchTerm + '%' OR
	StaffHoldingNames.Name LIKE '%' + @SearchTerm + '%' OR
	Departments.Name LIKE '%' + @SearchTerm + '%' OR
	Departments.Abbreviation LIKE '%' + @SearchTerm + '%')
ORDER BY
	StaffNames.Surname ASC