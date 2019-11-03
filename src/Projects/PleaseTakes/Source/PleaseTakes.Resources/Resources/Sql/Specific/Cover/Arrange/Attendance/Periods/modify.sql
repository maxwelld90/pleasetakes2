DECLARE
	@PeriodsTable TABLE (
		Period int,
		Status varchar(MAX))

IF EXISTS(
	SELECT
		Staff.StaffId
	FROM
		Staff
	WHERE
		Staff.StaffId = @StaffId)
	
	BEGIN
		IF EXISTS(
			SELECT
				Cover.CoverId
			FROM
				Cover
				INNER JOIN
					Attendance
					ON Attendance.AttendanceId = Cover.AttendanceId
					AND Attendance.Date = @Date
				INNER JOIN
					AttendanceStaff
					ON AttendanceStaff.AttendanceId = Attendance.AttendanceId
					AND AttendanceStaff.StaffId = @StaffId
			WHERE
				Cover.Period
					BETWEEN @PeriodMin AND @PeriodMax)
			
			BEGIN
				INSERT INTO
					@PeriodsTable
						(Period, Status)
					SELECT
						Cover.Period,
						Status =
							CASE
								WHEN TimetableEntries.EntryId IS NULL Then 'Present'
								WHEN YeargroupsShared.IsAway = 1 THEN 'PresentYeargroupAway'
								ELSE 'PresentIsBusy'
							END
					FROM
						Staff
						INNER JOIN
							AttendanceStaff
							ON Staff.StaffId = AttendanceStaff.StaffId
						INNER JOIN
							Attendance
							ON AttendanceStaff.AttendanceId = Attendance.AttendanceId
							AND Attendance.Date = @Date
						INNER JOIN
							Cover
							ON Attendance.AttendanceId = Cover.AttendanceId
						LEFT JOIN
							TimetableRelations
							ON Staff.StaffId = TimetableRelations.StaffId
						LEFT JOIN
							TimetableEntries
							ON TimetableRelations.TimetableId = TimetableEntries.TimetableId
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
							YeargroupsShared
							ON Blocks.YeargroupId = YeargroupsShared.SharedId
						/**
							NEED TO GET NON-CLASS YEARGROUPS WORKING
						**/
					WHERE
						Staff.StaffId = @StaffId
						AND Cover.Period
							BETWEEN @PeriodMin AND @PeriodMax
				DELETE
				FROM
					Cover
				WHERE
					Cover.AttendanceId IN (
						SELECT
							Attendance.AttendanceId
						FROM
							Attendance
							INNER JOIN
								AttendanceStaff
								ON AttendanceStaff.AttendanceId = Attendance.AttendanceId
								AND AttendanceStaff.StaffId = @StaffId
						WHERE
							Cover.Period
								BETWEEN @PeriodMin AND @PeriodMax)
			END
		ELSE
			BEGIN
				DECLARE @Counter int
				SET @Counter = @PeriodMin
				
				WHILE @Counter <= @PeriodMax
					BEGIN
						INSERT INTO
							Cover
								(AttendanceId, Period)
							SELECT
								Attendance.AttendanceId,
								@Counter
							FROM
								Attendance
								INNER JOIN
									AttendanceStaff
									ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
									AND Attendance.Date = @Date
									AND AttendanceStaff.StaffId = @StaffId
							WHERE
								AttendanceStaff.StaffId NOT IN (
									SELECT
										TimetableUnavailability.StaffId
									FROM
										TimetableUnavailability
									WHERE
										TimetableUnavailability.WeekNo = @WeekNo
										AND TimetableUnavailability.DayNo = @DayNo
										AND TimetableUnavailability.Period = @Counter
										AND TimetableUnavailability.StaffId = @StaffId
										AND TimetableUnavailability.CurrentSession = 1)
						INSERT INTO
							@PeriodsTable
								(Period, Status)
							SELECT
								@Counter,
								'Absent'
							WHERE NOT EXISTS(
								SELECT
									TimetableUnavailability.StaffId
								FROM
									TimetableUnavailability
								WHERE
									TimetableUnavailability.WeekNo = @WeekNo
									AND TimetableUnavailability.DayNo = @DayNo
									AND TimetableUnavailability.Period = @Counter
									AND TimetableUnavailability.StaffId = @StaffId
									AND TimetableUnavailability.CurrentSession = 1)
						
						SET @Counter = @Counter + 1
					END
			END
	END

SELECT
	*
FROM
	@PeriodsTable