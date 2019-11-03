DECLARE
	@PeriodsTable TABLE (
		WeekNo int,
		DayNo int,
		Period int,
		IsUnavailable bit,
		PeriodType int,
		CommitmentName varchar(MAX),
		RoomName varchar(MAX))

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
				TimetableRelations.StaffId
			FROM
				TimetableRelations
			WHERE
				TimetableRelations.StaffId = @StaffId)
			
			BEGIN
				SELECT CAST(1 AS int) AS Status
				
				INSERT INTO
					@PeriodsTable
						(WeekNo, DayNo, Period, IsUnavailable)
					SELECT
						TimetableUnavailability.WeekNo,
						TimetableUnavailability.DayNo,
						TimetableUnavailability.Period,
						1
					FROM
						TimetableUnavailability
					WHERE
						TimetableUnavailability.StaffId = @StaffId
						AND TimetableUnavailability.WeekNo = @WeekNo
						AND TimetableUnavailability.CurrentSession = @CurrentSession
				
				INSERT INTO
					@PeriodsTable
						(WeekNo, DayNo, Period, IsUnavailable, PeriodType, CommitmentName, RoomName)
					SELECT
						TimetableEntries.WeekNo,
						TimetableEntries.DayNo,
						TimetableEntries.Period,
						0,
						TimetableEntries.PeriodType,
						CommitmentName =
							CASE
								WHEN Classes.ClassId IS NOT NULL THEN Classes.Name
								ELSE TimetableUniqueEntries.Name
							END,
						Rooms.Name
					FROM
						TimetableEntries
						INNER JOIN
							TimetableRelations
							ON TimetableRelations.StaffId = @StaffId
						LEFT JOIN
							TimetableClasses
							ON TimetableEntries.EntryId = TimetableClasses.EntryId
						LEFT JOIN
							Classes
							ON TimetableClasses.ClassId = Classes.ClassId
						LEFT JOIN
							TimetableUniqueEntries
							ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
						LEFT JOIN
							TeachingRooms
							ON TimetableEntries.EntryId = TeachingRooms.EntryId
						LEFT JOIN
							Rooms
							ON TeachingRooms.RoomId = Rooms.RoomId
					WHERE
						TimetableEntries.TimetableId = TimetableRelations.TimetableId
						AND TimetableEntries.WeekNo = @WeekNo
						AND NOT EXISTS(
							SELECT
								*
							FROM
								@PeriodsTable
							WHERE
								WeekNo = TimetableEntries.WeekNo
								AND DayNo = TimetableEntries.DayNo
								AND Period = TimetableEntries.Period)
			END
		ELSE
			BEGIN
				SELECT CAST(2 AS int) AS Status
			END
	END
ELSE
	BEGIN
		SELECT CAST(0 AS int) AS Status
	END

SELECT
	*
FROM
	@PeriodsTable