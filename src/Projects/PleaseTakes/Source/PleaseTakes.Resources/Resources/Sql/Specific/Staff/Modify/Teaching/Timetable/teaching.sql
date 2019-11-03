DECLARE @EntryId int
DECLARE @UniqueEntryId int

IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT TimetableRelations.TimetableId FROM TimetableRelations WHERE TimetableRelations.StaffId = @StaffId)
			BEGIN
				IF (@IsUnique = 1)
					BEGIN
						IF EXISTS(SELECT YeargroupsShared.SharedId FROM YeargroupsShared WHERE YeargroupsShared.SharedId = @YeargroupId)
							BEGIN
								IF (@RoomId IS NOT NULL)
									BEGIN
										IF NOT EXISTS(SELECT Rooms.RoomId FROM Rooms WHERE Rooms.RoomId = @RoomId)
											BEGIN
												SELECT CAST(4 AS int) AS Status
												RETURN
											END
									END
								
								SELECT CAST(2 AS int) AS Status
								
								DELETE FROM
									TimetableUnavailability
								WHERE
									TimetableUnavailability.StaffId = @StaffId
									AND TimetableUnavailability.CurrentSession = 1
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
								
								INSERT INTO
									TimetableEntries
									(TimetableId, WeekNo, DayNo, Period, PeriodType, CurrentSession)
									SELECT
										TimetableRelations.TimetableId,
										@WeekNo,
										@DayNo,
										@Period,
										1,
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
								
								SET @UniqueEntryId = SCOPE_IDENTITY()
								
								INSERT INTO
									TimetableUniqueEntriesYeargroups
									(UniqueEntryId, YeargroupId)
								VALUES
									(@UniqueEntryId, @YeargroupId)
								
								INSERT INTO
									Teachers
									(EntryId, StaffId)
								VALUES
									(@EntryId, @StaffId)
								
								IF (@RoomId IS NOT NULL)
									BEGIN
										INSERT INTO
											TeachingRooms
											(EntryId, RoomId)
										VALUES
											(@EntryId, @RoomId)
									END
							END
						ELSE
							BEGIN
								SELECT CAST(3 AS int) AS Status
							END
					END
				ELSE
					BEGIN
						IF EXISTS(SELECT Subjects.SubjectId FROM Subjects WHERE Subjects.SubjectId = @SubjectId)
							BEGIN
								IF EXISTS(SELECT SubjectQualifications.SubjectQualificationId FROM SubjectQualifications WHERE SubjectQualifications.SubjectQualificationId = @SubjectQualificationId)
									BEGIN
										IF EXISTS(SELECT Classes.ClassId FROM Classes WHERE Classes.ClassId = @ClassId)
											BEGIN
												IF (@RoomId IS NOT NULL)
													BEGIN
														IF NOT EXISTS(SELECT Rooms.RoomId FROM Rooms WHERE Rooms.RoomId = @RoomId)
															BEGIN
																SELECT CAST(4 AS int) AS Status
																RETURN
															END
													END
												
												SELECT CAST(2 AS int) AS Status
												
												DELETE FROM
													TimetableUnavailability
												WHERE
													TimetableUnavailability.StaffId = @StaffId
													AND TimetableUnavailability.CurrentSession = 1
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
												
												INSERT INTO
													TimetableEntries
													(TimetableId, WeekNo, DayNo, Period, PeriodType, CurrentSession)
													SELECT
														TimetableRelations.TimetableId,
														@WeekNo,
														@DayNo,
														@Period,
														1,
														1
													FROM
														TimetableRelations
													WHERE
														TimetableRelations.StaffId = @StaffId
												
												SET @EntryId = SCOPE_IDENTITY()

INSERT INTO
	TimetableClasses
	(EntryId, ClassId)
VALUES
	(@EntryId, @ClassId)

INSERT INTO
	Teachers
	(EntryId, StaffId)
VALUES
	(@EntryId, @StaffId)

IF (@RoomId IS NOT NULL)
	BEGIN
		INSERT INTO
			TeachingRooms
			(EntryId, RoomId)
		VALUES
			(@EntryId, @RoomId)
	END


											END
										ELSE
											BEGIN
												SELECT CAST(7 AS int) AS Status
											END
									END
								ELSE
									BEGIN
										SELECT CAST(6 AS int) AS Status
									END
							END
						ELSE
							BEGIN
								SELECT CAST(5 AS int) AS Status
							END
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