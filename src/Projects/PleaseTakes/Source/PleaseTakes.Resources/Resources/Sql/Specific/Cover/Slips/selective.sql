SELECT
	Cover.CoverId,
	IsInternal =
		CASE
			WHEN InternalCover.CoverId IS NOT NULL THEN CAST(1 AS bit)
			ELSE CAST(0 AS bit)
		END,
	Cover.Period AS Period,
	
	AbsentStaffName.Forename AS AbsentForename,
	AbsentStaffName.Surname AS AbsentSurname,
	AbsentHoldingName.Name AS AbsentHoldingName,
	
	CommitmentName =
		CASE
			WHEN Classes.Name IS NOT NULL THEN Classes.Name
			WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
			ELSE NULL
		END,
	Subjects.Name AS CommitmentSubject,
	CommitmentRoom.Name AS CommitmentRoom,
	
	CoverForename =
		CASE
			WHEN InternalCover.CoverId IS NOT NULL THEN InternalCoverStaffName.Forename
			ELSE OutsideCoverStaffName.Forename
		END,
	CoverSurname =
		CASE
			WHEN InternalCover.CoverId IS NOT NULL THEN InternalCoverStaffName.Surname
			ELSE OutsideCoverStaffName.Surname
		END,
	InternalCoverHoldingName.Name AS CoverHoldingName,
	CoveringMainRoom.Name AS CoveringMainRoom,
	CoveringDepartment.Name AS CoverDepartment
FROM
	Cover
	INNER JOIN
		Attendance
		INNER JOIN
			AttendanceStaff
			ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
		ON Cover.AttendanceId = Attendance.AttendanceId
		AND Attendance.Date = @Date
	LEFT JOIN
		InternalCover
		ON Cover.CoverId = InternalCover.CoverId
	LEFT JOIN
		OutsideCover
		ON Cover.CoverId = OutsideCover.CoverId
	
	LEFT JOIN
		StaffNames AS AbsentStaffName
		ON AttendanceStaff.StaffId = AbsentStaffName.StaffId
	LEFT JOIN
		StaffHoldingNames AS AbsentHoldingName
		ON AttendanceStaff.StaffId = AbsentHoldingName.StaffId
	
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
		Rooms AS CommitmentRoom
		ON TeachingRooms.RoomId = CommitmentRoom.RoomId
	
	LEFT JOIN
		StaffNames AS InternalCoverStaffName
		ON InternalCover.StaffId = InternalCoverStaffName.StaffId
	LEFT JOIN
		StaffHoldingNames AS InternalCoverHoldingName
		ON InternalCover.StaffId = InternalCoverHoldingName.StaffId
	LEFT JOIN
		OutsideCoverStaff AS OutsideCoverStaffName
		ON OutsideCover.OutsideCoverStaffId = OutsideCoverStaffName.OutsideCoverStaffId
	LEFT JOIN
		DepartmentsStaff AS CoveringDepartmentLink
		ON InternalCover.StaffId = CoveringDepartmentLink.StaffId
	LEFT JOIN
		Departments AS CoveringDepartment
		ON CoveringDepartmentLink.DepartmentId = CoveringDepartment.DepartmentId
	LEFT JOIN
		MainRooms AS CoveringMainRoomLink
		ON InternalCover.StaffId = CoveringMainRoomLink.StaffId
	LEFT JOIN
		Rooms AS CoveringMainRoom
		ON CoveringMainRoomLink.RoomId = CoveringMainRoom.RoomId
WHERE
	(InternalCover.Id IS NOT NULL
	OR OutsideCover.Id IS NOT NULL)
	AND
	(
	Cover.Period LIKE '%' + @SearchTerm + '%'
	OR AbsentStaffName.Forename LIKE '%' + @SearchTerm + '%'
	OR AbsentStaffName.Surname LIKE '%' + @SearchTerm + '%'
	OR AbsentHoldingName.Name LIKE '%' + @SearchTerm + '%'
	OR Classes.Name LIKE '%' + @SearchTerm + '%'
	OR TimetableUniqueEntries.Name LIKE '%' + @SearchTerm + '%'
	OR Subjects.Name LIKE '%' + @SearchTerm + '%'
	OR CommitmentRoom.Name LIKE '%' + @SearchTerm + '%'
	OR InternalCoverStaffName.Forename LIKE '%' + @SearchTerm + '%'
	OR OutsideCoverStaffName.Forename LIKE '%' + @SearchTerm + '%'
	OR InternalCoverStaffName.Surname LIKE '%' + @SearchTerm + '%'
	OR OutsideCoverStaffName.Surname LIKE '%' + @SearchTerm + '%'
	OR InternalCoverHoldingName.Name LIKE '%' + @SearchTerm + '%'
	OR CoveringMainRoom.Name LIKE '%' + @SearchTerm + '%'
	OR CoveringDepartment.Name LIKE '%' + @SearchTerm + '%'
	)
ORDER BY
	AbsentStaffName.Surname ASC,
	Cover.Period ASC