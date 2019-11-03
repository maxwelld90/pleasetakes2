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
				
				SELECT
					StaffNames.Forename,
					StaffNames.Surname,
					StaffHoldingNames.Name AS HoldingName
				FROM
					Staff
					LEFT JOIN
						StaffNames
						ON Staff.StaffId = StaffNames.StaffId
					LEFT JOIN
						StaffHoldingNames
						ON Staff.StaffId = StaffHoldingNames.StaffId
				WHERE
					Staff.StaffId = @StaffId
				
				IF EXISTS(
					SELECT
						TimetableUnavailability.Id
					FROM
						TimetableUnavailability
					WHERE
						TimetableUnavailability.CurrentSession = 1
						AND TimetableUnavailability.StaffId = @StaffId
						AND TimetableUnavailability.WeekNo = @WeekNo
						AND TimetableUnavailability.DayNo = @DayNo
						AND TimetableUnavailability.Period = @Period)
					
					BEGIN
						SELECT CAST(0 AS int) AS PeriodStatus
					END
				ELSE
					BEGIN
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
								AND TimetableEntries.WeekNo = @WeekNo
								AND TimetableEntries.DayNo = @DayNo
								AND TimetableEntries.Period = @Period)
							
							BEGIN
								SELECT
									CAST(2 AS int) AS PeriodStatus,
									TimetableEntries.PeriodType,
									IsUnique =
										CASE
											WHEN TimetableClasses.Id IS NULL THEN CAST(1 AS bit)
											ELSE CAST(0 AS bit)
										END,
									CommitmentName =
										CASE
											WHEN Classes.ClassId IS NOT NULL THEN Classes.Name
											ELSE TimetableUniqueEntries.Name
										END,
									YeargroupName =
										CASE
											WHEN Classes.ClassId IS NOT NULL THEN ClassYeargroup.Name
											ELSE UniqueYeargroup.Name
										END,
									YeargroupId =
										CASE
											WHEN Classes.ClassId IS NOT NULL THEN ClassYeargroup.SharedId
											ELSE UniqueYeargroup.SharedId
										END,
									Rooms.Name AS RoomName,
									Rooms.RoomId AS RoomId,
									Subjects.Name AS SubjectName,
									Subjects.SubjectId AS SubjectId,
									SubjectQualifications.SubjectQualificationId,
									Qualifications.Name AS QualificationName,
									Classes.Name AS ClassName,
									Classes.ClassId AS ClassId
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
										SubjectQualifications
										ON Classes.SubjectQualificationId = SubjectQualifications.SubjectQualificationId
									LEFT JOIN
										Subjects
										ON SubjectQualifications.SubjectId = Subjects.SubjectId
									LEFT JOIN
										Qualifications
										ON SubjectQualifications.QualificationId = Qualifications.QualificationId
									LEFT JOIN
										Blocks
										ON Classes.BlockId = Blocks.BlockId
										AND Blocks.CurrentSession = 1
									LEFT JOIN
										YeargroupsShared AS ClassYeargroup
										ON Blocks.YeargroupId = ClassYeargroup.SharedId
									LEFT JOIN
										TimetableUniqueEntries
										ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
									LEFT JOIN
										TimetableUniqueEntriesYeargroups
										ON TimetableUniqueEntries.UniqueEntryId = TimetableUniqueEntriesYeargroups.UniqueEntryId
									LEFT JOIN
										YeargroupsShared AS UniqueYeargroup
										ON TimetableUniqueEntriesYeargroups.YeargroupId = UniqueYeargroup.SharedId
									LEFT JOIN
										TeachingRooms
										ON TimetableEntries.EntryId = TeachingRooms.EntryId
									LEFT JOIN
										Rooms
										ON TeachingRooms.RoomId = Rooms.RoomId
								WHERE
									TimetableEntries.TimetableId = TimetableRelations.TimetableId
									AND TimetableEntries.WeekNo = @WeekNo
									AND TimetableEntries.DayNo = @DayNo
									AND TimetableEntries.Period = @Period 
							END
						ELSE
							BEGIN
								SELECT CAST(1 AS int) AS PeriodStatus
							END
					END
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