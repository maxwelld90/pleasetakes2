IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT TimetableRelations.StaffId FROM TimetableRelations WHERE TimetableRelations.StaffId = @StaffId)
			BEGIN
				SELECT CAST(2 AS int) AS Status
				
				IF NOT EXISTS(
					SELECT
						TimetableUnavailability.Id
					FROM
						TimetableUnavailability
					WHERE
						TimetableUnavailability.StaffId = @StaffId
						AND TimetableUnavailability.CurrentSession = 1
						AND TimetableUnavailability.WeekNo = @WeekNo
						AND TimetableUnavailability.DayNo = @DayNo
						AND TimetableUnavailability.Period = @Period)
					
					BEGIN
						INSERT INTO
							TimetableUnavailability
							(StaffId, TimetableId, WeekNo, DayNo, Period, CurrentSession)
							SELECT
								@StaffId,
								TimetableRelations.TimetableId,
								@WeekNo,
								@DayNo,
								@Period,
								1
							FROM
								TimetableRelations
							WHERE
								TimetableRelations.StaffId = @StaffId
					END
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