IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT TimetableRelations.StaffId FROM TimetableRelations WHERE TimetableRelations.StaffId = @StaffId)
			BEGIN
				SELECT CAST(2 AS int) AS Status
				
				DELETE FROM
					TimetableUnavailability
				WHERE
					TimetableUnavailability.CurrentSession = 1
					AND TimetableUnavailability.StaffId = @StaffId
					AND TimetableUnavailability.WeekNo = @WeekNo
					AND TimetableUnavailability.DayNo = @DayNo
					AND TimetableUnavailability.Period = @Period
				
				DELETE FROM
					TimetableEntries
				WHERE
					TimetableEntries.CurrentSession = 1
					AND TimetableEntries.TimetableId = (
						SELECT
							TimetableRelations.TimetableId
						FROM
							TimetableRelations
						WHERE
							TimetableRelations.StaffId = @StaffId)
					AND TimetableEntries.WeekNo = @WeekNo
					AND TimetableEntries.DayNo = @DayNo
					AND TimetableEntries.Period = @Period
			END
		ELSE
			BEGIN
				SELECT CAST(1 AS int) AS Status
			END
	END
ELSE
	BEGIN
		SELECT CAST(0 AS int) AS Status
	END