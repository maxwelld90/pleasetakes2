SELECT DISTINCT
	Staff.StaffId,
	StaffNames.Forename,
	StaffNames.Surname,
	StaffHoldingNames.Name AS HoldingName
FROM
	Staff
	INNER JOIN
		AttendanceStaff
		ON Staff.StaffId = AttendanceStaff.StaffId
	INNER JOIN
		Attendance
		ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
		AND Attendance.Date = @Date
	LEFT JOIN
		StaffNames
		ON Staff.StaffId = StaffNames.StaffId
	LEFT JOIN
		StaffHoldingNames
		ON Staff.StaffId = StaffHoldingNames.StaffId
	LEFT JOIN
		DepartmentsStaff
		ON Staff.StaffId = DepartmentsStaff.StaffId
	LEFT JOIN
		Departments
		ON DepartmentsStaff.DepartmentId = Departments.DepartmentId
WHERE
	StaffNames.Forename LIKE '%' + @SearchTerm + '%' OR
	StaffNames.Surname LIKE '%' + @SearchTerm + '%' OR
	StaffHoldingNames.Name LIKE '%' + @SearchTerm + '%' OR
	Departments.Name LIKE '%' + @SearchTerm + '%' OR
	Departments.Abbreviation LIKE '%' + @SearchTerm + '%'
ORDER BY
	StaffNames.Surname ASC

DECLARE
	@PeriodsTable TABLE(
		StaffId int,
		Period int,
		Status varchar(27))

INSERT INTO
	@PeriodsTable
		(StaffId, Period, Status)
	SELECT
		Staff.StaffId,
		TimetableUnavailability.Period,
		'Unavailable' /* Job Share */
	FROM
		Staff
		INNER JOIN
			TimetableUnavailability
				ON Staff.StaffId = TimetableUnavailability.StaffId
				AND TimetableUnavailability.WeekNo = @WeekNo
				AND TimetableUnavailability.DayNo = @DayNo
				AND TimetableUnavailability.CurrentSession = 1
		INNER JOIN
			AttendanceStaff
				ON Staff.StaffId = AttendanceStaff.StaffId
		INNER JOIN
			Attendance
				ON AttendanceStaff.AttendanceId = Attendance.AttendanceId
				AND Attendance.Date = @Date

INSERT INTO
	@PeriodsTable
		(StaffId, Period, Status)
	SELECT
		Staff.StaffId,
		Cover.Period,
		'Absent' /* Absent */
	FROM
		Staff
		INNER JOIN
			AttendanceStaff
				ON Staff.StaffId = AttendanceStaff.StaffId
		INNER JOIN
			Attendance
				ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
				AND Attendance.Date = @Date
		INNER JOIN
			Cover
				ON Attendance.AttendanceId = Cover.AttendanceId
	WHERE
		NOT EXISTS(
			SELECT
				StaffId
			FROM
				@PeriodsTable
			WHERE
				StaffId = Staff.StaffId
				AND Period = Cover.Period)

INSERT INTO
	@PeriodsTable
		(StaffId, Period, Status)
	SELECT
		Staff.StaffId,
		TimetableEntries.Period,
		PeriodType = 
			CASE
				WHEN YeargroupsShared.IsAway = 1 THEN 'PresentAndFreeYeargroupAway' /* Yeargroup away, free */
				ELSE 'PresentIsBusy' /* Busy - class (yg is not away) or non-class tt entry */
			END
	FROM
		Staff
		INNER JOIN
			TimetableRelations
				ON Staff.StaffId = TimetableRelations.StaffId
		RIGHT JOIN
			TimetableEntries
				ON TimetableRelations.TimetableId = TimetableEntries.TimetableId
				AND TimetableEntries.WeekNo = @WeekNo
				AND TimetableEntries.DayNo = @DayNo
				AND TimetableEntries.CurrentSession = 1
		LEFT JOIN
			TimetableClasses
				ON TimetableEntries.EntryId = TimetableClasses.EntryId
		LEFT JOIN
			Classes
				ON TimetableClasses.ClassId = Classes.ClassId
		LEFT JOIN
			Blocks
				ON Classes.BlockId = Blocks.BlockId
				AND Blocks.CurrentSession = 1
		LEFT JOIN
			YeargroupsShared
				ON Blocks.YeargroupId = YeargroupsShared.SharedId
		INNER JOIN
			AttendanceStaff
				ON Staff.StaffId = AttendanceStaff.StaffId
		INNER JOIN
			Attendance
				ON AttendanceStaff.AttendanceId = Attendance.AttendanceId
				AND Attendance.Date = @Date
	WHERE
		NOT EXISTS(
			SELECT
				StaffId
			FROM
				@PeriodsTable
			WHERE
				StaffId = Staff.StaffId
				AND Period = TimetableEntries.Period)

SELECT * FROM @PeriodsTable