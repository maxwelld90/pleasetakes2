IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT TimetableRelations.Id FROM TimetableRelations WHERE TimetableRelations.StaffId = @StaffId)
			BEGIN
				SELECT CAST(1 AS int) AS Status
			END
		ELSE
			BEGIN
				SELECT CAST(2 AS int) AS Status
				
				INSERT INTO
					TimetableIds
					DEFAULT VALUES

				DECLARE @NewTimetableId int
				SET @NewTimetableId = SCOPE_IDENTITY()
				
				INSERT INTO
					TimetableRelations
					(StaffId, TimetableId)
				VALUES
					(@StaffId, @NewTimetableId)
			END
	END
ELSE
	BEGIN
		SELECT CAST(0 AS int) AS Status
	END