SELECT
	InternalCover.CoverId,
	Cover.Period,
	CoverRoom.Name AS CoverRoom,
	AbsentStaffName.Forename AS AbsentForename,
	AbsentStaffName.Surname AS AbsentSurname,
	AbsentStaffHoldingName.Name AS AbsentHoldingName,
	CommitmentName =
		CASE
			WHEN Classes.Name IS NOT NULL THEN Classes.Name
			WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
			ELSE NULL
		END,
	Subjects.Name AS SubjectName,
	CoveringStaffName.Forename AS CoveringForename,
	CoveringStaffName.Surname AS CoveringSurname,
	CoveringStaffHoldingName.Name AS CoveringHoldingName,
	CoveringMainRoom.Name AS CoveringMainRoom,
	CoveringDepartment =
		CASE
			WHEN Departments.Abbreviation IS NULL AND Departments.Name IS NOT NULL THEN Departments.Name
			WHEN Departments.Abbreviation IS NOT NULL THEN Departments.Abbreviation
			ELSE Departments.Name
		END
FROM
	InternalCover
	INNER JOIN
		Cover
		INNER JOIN
			Attendance
			INNER JOIN
				AttendanceStaff
				ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
			ON Cover.AttendanceId = Attendance.AttendanceId
		ON InternalCover.CoverId = Cover.CoverId
		AND Attendance.Date = @Date
	LEFT JOIN
		StaffNames AS AbsentStaffName
		ON AttendanceStaff.StaffId = AbsentStaffName.StaffId
	LEFT JOIN
		StaffHoldingNames AS AbsentStaffHoldingName
		ON AttendanceStaff.StaffId = AbsentStaffHoldingName.StaffId
	LEFT JOIN
		TimetableRelations
		ON AttendanceStaff.StaffId = TimetableRelations.StaffId
	LEFT JOIN
		TimetableEntries
		ON TimetableRelations.TimetableId = TimetableEntries.TimetableId
		AND TimetableEntries.WeekNo = @WeekNo
		AND TimetableEntries.DayNo = @DayNo
		AND TimetableEntries.Period = Cover.Period
		AND TimetableEntries.CurrentSession = 1
	LEFT JOIN
		TimetableUniqueEntries
		ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
	LEFT JOIN
		TimetableClasses
		ON TimetableEntries.EntryId = TimetableClasses.EntryId
	LEFT JOIN
		Classes
		ON TimetableClasses.ClassId = Classes.ClassId
	LEFT JOIN
		SubjectQualifications
		ON Classes.SubjectQualificationId = SubjectQualifications.SubjectQualificationId
	LEFT JOIN
		Subjects
		ON SubjectQualifications.SubjectId = Subjects.SubjectId
	LEFT JOIN
		TeachingRooms
		ON TimetableEntries.EntryId = TeachingRooms.EntryId
	LEFT JOIN
		Rooms AS CoverRoom
		ON TeachingRooms.RoomId = CoverRoom.RoomId
	
	LEFT JOIN
		StaffNames AS CoveringStaffName
		ON InternalCover.StaffId = CoveringStaffName.StaffId
	LEFT JOIN
		StaffHoldingNames AS CoveringStaffHoldingName
		ON InternalCover.StaffId = CoveringStaffHoldingName.StaffId
	LEFT JOIN
		MainRooms AS CoveringMainRoomLink
		ON InternalCover.StaffId = CoveringMainRoomLink.StaffId
	LEFT JOIN
		Rooms AS CoveringMainRoom
		ON CoveringMainRoomLink.RoomId = CoveringMainRoom.RoomId
	LEFT JOIN
		DepartmentsStaff
		ON InternalCover.StaffId = DepartmentsStaff.StaffId
	LEFT JOIN
		Departments
		ON DepartmentsStaff.DepartmentId = Departments.DepartmentId
ORDER BY
	CoveringStaffName.Surname ASC,
	Cover.Period ASC

SELECT
	OutsideCover.CoverId,
	Cover.Period,
	CoverRoom.Name AS CoverRoom,
	AbsentStaffName.Forename AS AbsentForename,
	AbsentStaffName.Surname AS AbsentSurname,
	AbsentStaffHoldingName.Name AS AbsentHoldingName,
	CommitmentName =
		CASE
			WHEN Classes.Name IS NOT NULL THEN Classes.Name
			WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
			ELSE NULL
		END,
	Subjects.Name AS SubjectName,
	CoveringStaffName.Forename AS CoveringForename,
	CoveringStaffName.Surname AS CoveringSurname
FROM
	OutsideCover
	INNER JOIN
		Cover
		INNER JOIN
			Attendance
			INNER JOIN
				AttendanceStaff
				ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
			ON Cover.AttendanceId = Attendance.AttendanceId
		ON OutsideCover.CoverId = Cover.CoverId
		AND Attendance.Date = @Date
	LEFT JOIN
		StaffNames AS AbsentStaffName
		ON AttendanceStaff.StaffId = AbsentStaffName.StaffId
	LEFT JOIN
		StaffHoldingNames AS AbsentStaffHoldingName
		ON AttendanceStaff.StaffId = AbsentStaffHoldingName.StaffId
	LEFT JOIN
		TimetableRelations
		ON AttendanceStaff.StaffId = TimetableRelations.StaffId
	LEFT JOIN
		TimetableEntries
		ON TimetableRelations.TimetableId = TimetableEntries.TimetableId
		AND TimetableEntries.WeekNo = @WeekNo
		AND TimetableEntries.DayNo = @DayNo
		AND TimetableEntries.Period = Cover.Period
		AND TimetableEntries.CurrentSession = 1
	LEFT JOIN
		TimetableUniqueEntries
		ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
	LEFT JOIN
		TimetableClasses
		ON TimetableEntries.EntryId = TimetableClasses.EntryId
	LEFT JOIN
		Classes
		ON TimetableClasses.ClassId = Classes.ClassId
	LEFT JOIN
		SubjectQualifications
		ON Classes.SubjectQualificationId = SubjectQualifications.SubjectQualificationId
	LEFT JOIN
		Subjects
		ON SubjectQualifications.SubjectId = Subjects.SubjectId
	LEFT JOIN
		TeachingRooms
		ON TimetableEntries.EntryId = TeachingRooms.EntryId
	LEFT JOIN
		Rooms AS CoverRoom
		ON TeachingRooms.RoomId = CoverRoom.RoomId
	
	LEFT JOIN
		OutsideCoverStaff AS CoveringStaffName
		ON OutsideCover.OutsideCoverStaffId = CoveringStaffName.OutsideCoverStaffId
ORDER BY
	CoveringStaffName.Surname ASC,
	Cover.Period ASC