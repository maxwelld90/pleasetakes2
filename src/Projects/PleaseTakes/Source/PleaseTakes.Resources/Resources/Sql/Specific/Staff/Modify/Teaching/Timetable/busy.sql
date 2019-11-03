IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT TimetableRelations.StaffId FROM TimetableRelations WHERE TimetableRelations.StaffId = @StaffId)
			BEGIN
				DECLARE @EntryId int
				SELECT CAST(2 AS int) AS Status
				
				DELETE FROM
					TimetableUnavailability
				WHERE
					TimetableUnavailability.CurrentSession = 1
					AND TimetableUnavailability.StaffId = @StaffId
					AND TimetableUnavailability.WeekNo = @WeekNo
					AND TimetableUnavailability.DayNo = @DayNo
					AND TimetableUnavailability.Period = @Period
				
				IF EXISTS(
					SELECT
						TimetableEntries.EntryId
					FROM
						TimetableEntries
						INNER JOIN
							TimetableRelations
							ON TimetableRelations.StaffId = @StaffId
					WHERE
						TimetableEntries.TimetableId = TimetableRelations.TimetableId
						AND TimetableEntries.CurrentSession = 1
						AND TimetableEntries.WeekNo = @WeekNo	
						AND TimetableEntries.DayNo = @DayNo
						AND TimetableEntries.Period = @Period
						AND TimetableEntries.PeriodType = 2)
					
					BEGIN
						UPDATE
							TimetableUniqueEntries
						SET
							TimetableUniqueEntries.Name = @CommitmentName
						WHERE
							TimetableUniqueEntries.EntryId = (
							SELECT
								TimetableEntries.EntryId
							FROM
								TimetableEntries
								INNER JOIN
									TimetableRelations
									ON TimetableRelations.StaffId = @StaffId
							WHERE
								TimetableEntries.TimetableId = TimetableRelations.TimetableId
								AND TimetableEntries.CurrentSession = 1
								AND TimetableEntries.WeekNo = @WeekNo	
								AND TimetableEntries.DayNo = @DayNo
								AND TimetableEntries.Period = @Period
								AND TimetableEntries.PeriodType = 2)
					END
				ELSE
					BEGIN
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
						
						INSERT INTO
							TimetableEntries
							(TimetableId, WeekNo, DayNo, Period, PeriodType, CurrentSession)
							SELECT
								TimetableRelations.TimetableId,
								@WeekNo,
								@DayNo,
								@Period,
								2,
								1
							FROM
								TimetableRelations
							WHERE
								TimetableRelations.StaffId = @StaffId
						
						SET @EntryId = SCOPE_IDENTITY()
						
						INSERT INTO
							TimetableUniqueEntries
							(EntryId, Name)
						VALUES
							(@EntryId, @CommitmentName)
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