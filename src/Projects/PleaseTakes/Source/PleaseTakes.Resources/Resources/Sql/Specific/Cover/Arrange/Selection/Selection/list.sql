DECLARE @Period int
DECLARE @AbsentStaffId int

IF EXISTS(
	SELECT
		Cover.CoverId
	FROM
		Cover
	WHERE
		Cover.CoverId = @CoverId)
	
	BEGIN
		SELECT CAST(1 AS bit) AS DoesExist
		
		SELECT
			@Period = Cover.Period,
			@AbsentStaffId = AttendanceStaff.StaffId
		FROM
			Cover
			INNER JOIN
				Attendance
				ON Cover.AttendanceId = Attendance.AttendanceId
			INNER JOIN
				AttendanceStaff
				ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
		WHERE
			Cover.CoverId = @CoverId
		
		IF (@GetInternalStaff = 1)
			BEGIN
				SELECT DISTINCT
					StaffList.StaffId AS Id,
					StaffNames.Forename,
					StaffNames.Surname,
					StaffHoldingNames.Name AS HoldingName,
					CoverCount.RemainingEntitlement,
					IsSelected =
					CASE
						WHEN ((InternalCover.CoverId = @CoverId) AND (InternalCover.StaffId = StaffList.StaffId)) THEN
							CAST(1 AS bit)
						ELSE
							CAST(0 AS bit)
					END
				FROM
					Staff AS StaffList
					LEFT JOIN (
							SELECT
								Staff.StaffId,
								(Staff.Entitlement - COUNT(InternalCover.CoverId)) As RemainingEntitlement
							FROM
								Staff
								LEFT JOIN
									InternalCover
									LEFT JOIN
										Cover
										LEFT JOIN
											Attendance
											ON Cover.AttendanceId = Attendance.AttendanceId
										ON InternalCover.CoverId = Cover.CoverId
									ON Staff.StaffId = InternalCover.StaffId
									AND Attendance.Date BETWEEN @StartDate AND @EndDate	
							GROUP BY
								Staff.StaffId,
								Staff.Entitlement) AS CoverCount
						ON StaffList.StaffId = CoverCount.StaffId
					LEFT JOIN
						StaffNames
						ON StaffList.StaffId = StaffNames.StaffId
					LEFT JOIN
						StaffHoldingNames
						ON StaffList.StaffId = StaffHoldingNames.StaffId
					LEFT JOIN
						InternalCover
						ON InternalCover.CoverId = @CoverId
					LEFT JOIN
						TimetableRelations
						ON StaffList.StaffId = TimetableRelations.StaffId
					LEFT JOIN
						TimetableEntries
						ON TimetableRelations.TimetableId = TimetableEntries.TimetableId
						AND TimetableEntries.WeekNo = @WeekNo
						AND TimetableEntries.DayNo = @DayNo
						AND TimetableEntries.Period = @Period
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
					LEFT JOIN
						DepartmentsStaff
						ON StaffList.StaffId = DepartmentsStaff.StaffId
					LEFT JOIN
						Departments
						ON DepartmentsStaff.DepartmentId = Departments.DepartmentId
				WHERE
					NOT EXISTS(
						SELECT
							TimetableUnavailability.Id
						FROM
							TimetableUnavailability
						WHERE
							TimetableUnavailability.StaffId = StaffList.StaffId
							AND TimetableUnavailability.WeekNo = @WeekNo
							AND TimetableUnavailability.DayNo = @DayNo
							AND TimetableUnavailability.Period = @Period
							AND TimetableUnavailability.CurrentSession = 1)
					/** New **/
					AND NOT EXISTS(
						SELECT
							InternalCover.StaffId
						FROM
							InternalCover
							INNER JOIN
								Cover
								INNER JOIN
									Attendance
									ON Cover.AttendanceId = Attendance.AttendanceId
									AND Attendance.Date = @CurrentDate
								ON InternalCover.CoverId = Cover.CoverId
								AND Cover.Period = @Period
						WHERE
							InternalCover.StaffId = StaffList.StaffId
							AND InternalCover.CoverId <> @CoverId)
					AND NOT EXISTS(
SELECT
	AttendanceStaff.StaffId
FROM
	AttendanceStaff
	INNER JOIN
		Attendance
		ON AttendanceStaff.AttendanceId = Attendance.AttendanceId
		AND Attendance.Date = @CurrentDate
	INNER JOIN
		Cover
		ON Attendance.AttendanceId = Cover.AttendanceId
		AND Cover.Period = @Period
WHERE
	AttendanceStaff.StaffId = StaffList.StaffId
)
					AND
						StaffList.StaffId <> @AbsentStaffId
					AND
						((TimetableEntries.EntryId IS NULL)
						AND (YeargroupsShared.IsAway IS NULL)
						OR (YeargroupsShared.IsAway = 1))
					AND
						(StaffNames.Forename LIKE '%' + @SearchTerm + '%'
						OR StaffNames.Surname LIKE '%' + @SearchTerm + '%'
						OR StaffHoldingNames.Name LIKE '%' + @SearchTerm + '%'
						OR Departments.Name LIKE '%' + @SearchTerm + '%'
						OR Departments.Abbreviation LIKE '%' + @SearchTerm + '%')
					AND
						((@ShowZeroEntitlement = 1
						OR (CoverCount.RemainingEntitlement > 0))
						OR (@ShowZeroEntitlement = 0
						AND (CoverCount.RemainingEntitlement <= 0)
						AND InternalCover.CoverId = @CoverId
						AND InternalCover.StaffId = StaffList.StaffId))
				ORDER BY
					StaffNames.Surname
			END
		ELSE
			BEGIN
				SELECT
					StaffList.OutsideCoverStaffId AS Id,
					StaffList.Forename,
					StaffList.Surname,
					CoverCount.RemainingEntitlement,
					IsSelected =
						CASE
							WHEN ((OutsideCover.CoverId = @CoverId) AND (OutsideCover.OutsideCoverStaffId = StaffList.OutsideCoverStaffId)) THEN
								CAST(1 AS bit)
							ELSE
								CAST(0 AS bit)
						END
				FROM
					OutsideCoverStaff AS StaffList
					LEFT JOIN(
							SELECT
								OutsideCoverStaff.OutsideCoverStaffId,
								(OutsideCoverStaff.Entitlement - COUNT(OutsideCover.CoverId)) AS RemainingEntitlement
							FROM
								OutsideCoverStaff
								LEFT JOIN
									OutsideCover
									LEFT JOIN
										Cover
										LEFT JOIN
											Attendance
											ON Cover.AttendanceId = Attendance.AttendanceId
										ON OutsideCover.CoverId = Cover.CoverId
									ON OutsideCoverStaff.OutsideCoverStaffId = OutsideCover.OutsideCoverStaffId
									AND Attendance.Date BETWEEN @StartDate AND @EndDate
							GROUP BY
								OutsideCoverStaff.OutsideCoverStaffId,
								OutsideCoverStaff.Entitlement) AS CoverCount
						ON StaffList.OutsideCoverStaffId = CoverCount.OutsideCoverStaffId
					LEFT JOIN
						OutsideCover
						ON OutsideCover.CoverId = @CoverId
				WHERE
					NOT EXISTS(
						SELECT
							OutsideCoverSpecificUnavailability.SpecificUnavailabilityId
						FROM
							OutsideCoverSpecificUnavailability
							INNER JOIN
								OutsideCoverSpecificUnavailabilityPeriods
								ON OutsideCoverSpecificUnavailability.SpecificUnavailabilityId = OutsideCoverSpecificUnavailabilityPeriods.SpecificUnavailabilityId
						WHERE
							OutsideCoverSpecificUnavailability.OutsideCoverStaffId = StaffList.OutsideCoverStaffId
							AND OutsideCoverSpecificUnavailability.Date = @CurrentDate
							AND OutsideCoverSpecificUnavailabilityPeriods.Period = @Period)
					AND NOT EXISTS(
						SELECT
							OutsideCoverPermanentUnavailability.PermanentUnavailabilityId
						FROM
							OutsideCoverPermanentUnavailability
							INNER JOIN
								OutsideCoverPermanentUnavailabilityPeriods
								ON OutsideCoverPermanentUnavailability.PermanentUnavailabilityId = OutsideCoverPermanentUnavailabilityPeriods.PermanentUnavailabilityId
						WHERE
							OutsideCoverPermanentUnavailability.OutsideCoverStaffId = StaffList.OutsideCoverStaffId
							AND OutsideCoverPermanentUnavailability.WeekNo = @WeekNo
							AND OutsideCoverPermanentUnavailability.DayNo = @DayNo
							AND OutsideCoverPermanentUnavailabilityPeriods.Period = @Period)
AND NOT EXISTS(
	SELECT
		OutsideCover.OutsideCoverStaffId
	FROM
		OutsideCover
		INNER JOIN
			Cover
			INNER JOIN
				Attendance
				ON Cover.AttendanceId = Attendance.AttendanceId
				AND Attendance.Date = @CurrentDate
			ON OutsideCover.CoverId = Cover.CoverId
			AND Cover.Period = @Period
	WHERE
		OutsideCover.OutsideCoverStaffId = StaffList.OutsideCoverStaffId
		AND OutsideCover.CoverId <> @CoverId)
					AND
						(StaffList.Forename LIKE '%' + @SearchTerm + '%'
						OR StaffList.Surname LIKE '%' + @SearchTerm + '%')
					AND
						((@ShowZeroEntitlement = 1
						OR (CoverCount.RemainingEntitlement > 0))
						OR (@ShowZeroEntitlement = 0
						AND (CoverCount.RemainingEntitlement <= 0)
						AND OutsideCover.CoverId = @CoverId
						AND OutsideCover.OutsideCoverStaffId = StaffList.OutsideCoverStaffId))

				ORDER BY
					StaffList.Surname ASC
			END
	END
ELSE
	BEGIN
		SELECT CAST(0 AS bit) AS DoesExist
	END