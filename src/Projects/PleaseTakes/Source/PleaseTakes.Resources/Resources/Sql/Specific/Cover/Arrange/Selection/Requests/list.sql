SELECT DISTINCT
	Cover.CoverId,
	StaffNames.Forename AS AbsentForename,
	StaffNames.Surname AS AbsentSurname,
	StaffHoldingNames.Name AS AbsentHoldingName,
	Cover.Period,
	TeachingClass =
		CASE
			WHEN TimetableEntries.PeriodType IS NULL OR TimetableEntries.PeriodType <> 1 THEN CAST(0 AS bit)
			ELSE CAST(1 AS bit)
		END,
	IsBusy =
		CASE
			WHEN TimetableEntries.EntryId IS NOT NULL THEN CAST(1 AS bit)
			ELSE CAST(0 AS bit)
		END,
	CommitmentName =
		CASE
			WHEN Classes.Name IS NOT NULL THEN Classes.Name
			WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
			ELSE NULL
		END,
	IsYeargroupAway =
		CASE
			WHEN Classes.ClassId IS NOT NULL THEN ClassYeargroups.IsAway
			WHEN UniqueYeargroups.IsAway IS NOT NULL THEN UniqueYeargroups.IsAway
			ELSE CAST(0 AS bit)
		END,
	HasCover =
		CASE
			WHEN ((InternalCover.Id IS NULL) AND (OutsideCover.Id IS NULL)) THEN CAST(0 AS bit)
			ELSE CAST(1 AS bit)
		END,
	IsInternalCover = 
		CASE
			WHEN ((InternalCover.Id IS NULL) AND (OutsideCover.Id IS NULL)) THEN CAST(0 AS bit)
			WHEN InternalCover.Id IS NOT NULL THEN CAST(1 AS bit)
			ELSE CAST(0 AS bit)
		END,
	CoverForename = 
		CASE
			WHEN InternalCover.Id IS NOT NULL THEN
				(SELECT
					StaffNames.Forename
				FROM
					Staff
					LEFT JOIN
						StaffNames
						ON Staff.StaffId = StaffNames.StaffId
				WHERE
					Staff.StaffId = InternalCover.StaffId)
			ELSE
				(SELECT
					OutsideCoverStaff.Forename
				FROM
					OutsideCoverStaff
				WHERE
					OutsideCoverStaff.OutsideCoverStaffId = OutsideCover.OutsideCoverStaffId)
		END,
	CoverSurname =
		CASE
			WHEN InternalCover.Id IS NOT NULL THEN
				(SELECT
					StaffNames.Surname
				FROM
					Staff
					LEFT JOIN
						StaffNames
						ON Staff.StaffId = StaffNames.StaffId
				WHERE
					Staff.StaffId = InternalCover.StaffId)
			ELSE
				(SELECT
					OutsideCoverStaff.Surname
				FROM
					OutsideCoverStaff
				WHERE
					OutsideCoverStaff.OutsideCoverStaffId = OutsideCover.OutsideCoverStaffId)
		END,
	CoverHoldingName =
		CASE
			WHEN InternalCover.Id IS NOT NULL THEN
				(SELECT
					StaffHoldingNames.Name
				FROM
					Staff
					LEFT JOIN
						StaffHoldingNames
						ON Staff.StaffId = StaffHoldingNames.StaffId
				WHERE
					Staff.StaffId = InternalCover.StaffId)
			ELSE NULL
		END
FROM
	Cover
	INNER JOIN
		Attendance
		ON Attendance.AttendanceId = Cover.AttendanceId
		AND Attendance.Date = @Date
	INNER JOIN
		AttendanceStaff
		ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
	LEFT JOIN
		StaffNames
		ON AttendanceStaff.StaffId = StaffNames.StaffId
	LEFT JOIN
		StaffHoldingNames
		ON AttendanceStaff.StaffId = StaffHoldingNames.StaffId
	LEFT JOIN
		InternalCover
		ON Cover.CoverId = InternalCover.CoverId
	LEFT JOIN
		OutsideCover
		ON Cover.CoverId = OutsideCover.CoverId
	LEFT JOIN
		TimetableRelations
		ON AttendanceStaff.StaffId = TimetableRelations.StaffId
	LEFT JOIN
		TimetableEntries
		ON TimetableEntries.TimetableId = TimetableRelations.TimetableId
		AND TimetableEntries.WeekNo = @WeekNo
		AND TimetableEntries.DayNo = @DayNo
		AND TimetableEntries.Period = Cover.Period
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
		YeargroupsShared AS ClassYeargroups
		ON Blocks.YeargroupId = ClassYeargroups.SharedId
	LEFT JOIN
		TimetableUniqueEntries
		ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
	LEFT JOIN
		TimetableUniqueEntriesYeargroups
		ON TimetableUniqueEntries.UniqueEntryId = TimetableUniqueEntriesYeargroups.UniqueEntryId
	LEFT JOIN
		YeargroupsShared AS UniqueYeargroups
		ON TimetableUniqueEntriesYeargroups.YeargroupId = UniqueYeargroups.SharedId
	LEFT JOIN
		DepartmentsStaff
		ON AttendanceStaff.StaffId = DepartmentsStaff.StaffId
	LEFT JOIN Departments
		ON DepartmentsStaff.DepartmentId = Departments.DepartmentId
WHERE
	NOT EXISTS(
		SELECT
			TimetableUnavailability.Id
		FROM
			TimetableUnavailability
		WHERE
			TimetableUnavailability.StaffId = AttendanceStaff.StaffId
			AND TimetableUnavailability.WeekNo = @WeekNo
			AND TimetableUnavailability.DayNo = @DayNo
			AND TimetableUnavailability.Period = Cover.Period
			AND TimetableUnavailability.CurrentSession = 1)
	AND
		(((@GetNoCoverRequests = 1) AND ((TimetableEntries.EntryId IS NULL) OR (TimetableEntries.PeriodType <> 1) OR (ClassYeargroups.IsAway = 1) OR (UniqueYeargroups.IsAway = 1)))
		OR
		((@GetNoCoverRequests = 0) AND ((TimetableEntries.EntryId IS NOT NULL) AND (TimetableEntries.PeriodType = 1) AND ((ClassYeargroups.IsAway IS NULL OR ClassYeargroups.IsAway = 0) AND (UniqueYeargroups.IsAway IS NULL OR UniqueYeargroups.IsAway = 0)))))
	AND
		(StaffNames.Forename LIKE '%' + @SearchTerm + '%'
		OR StaffNames.Surname LIKE '%' + @SearchTerm + '%'
		OR StaffHoldingNames.Name LIKE '%' + @SearchTerm + '%'
		OR Classes.Name LIKE '%' + @SearchTerm + '%'
		OR TimetableUniqueEntries.Name LIKE '%' + @SearchTerm + '%'
		OR Departments.Name LIKE '%' + @SearchTerm + '%'
		OR Departments.Abbreviation LIKE '%' + @SearchTerm + '%'
		OR Cover.Period LIKE '%' + @SearchTerm + '%')
ORDER BY
	StaffNames.Forename ASC,
	Cover.Period ASC