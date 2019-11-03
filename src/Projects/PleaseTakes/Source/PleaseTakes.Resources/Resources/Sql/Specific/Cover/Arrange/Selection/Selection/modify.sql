DECLARE
	@ResponseTable TABLE(
		Id int,
		IsRequest bit,
		IsInternalCover bit,
		RequestHasCover bit,
		RequestPeriod int,
		RequestCommitmentName varchar(MAX),
		RequestAbsentForename varchar(MAX),
		RequestAbsentSurname varchar(MAX),
		RequestAbsentHoldingName varchar(MAX),
		RequestCoverForename varchar(MAX),
		RequestCoverSurname varchar(MAX),
		RequestCoverHoldingName varchar(MAX),
		SelectionIsSelected bit,
		SelectionEntitlement int,
		SelectionForename varchar(MAX),
		SelectionSurname varchar(MAX),
		SelectionHoldingName varchar(MAX))

IF EXISTS(SELECT Cover.CoverId FROM Cover WHERE Cover.CoverId = @CoverId)
	BEGIN
		IF (@IsInternalStaff = 1)
			BEGIN
				IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
					BEGIN
						/** Part A **/
						DELETE FROM OutsideCover WHERE OutsideCover.CoverId = @CoverId
						
						IF EXISTS(SELECT InternalCover.Id FROM InternalCover WHERE InternalCover.CoverId = @CoverId)
							BEGIN
								IF EXISTS(SELECT InternalCover.Id FROM InternalCover WHERE InternalCover.CoverId = @CoverId AND InternalCover.StaffId = @StaffId)
									BEGIN
										/** Cover request exists, and is current staff ID **/
										
										/** Part B **/
										DELETE FROM InternalCover WHERE InternalCover.CoverId = @CoverId
										
										/** Part C **/
										INSERT INTO
											@ResponseTable
											(Id, IsRequest, IsInternalCover, RequestHasCover, RequestPeriod, RequestCommitmentName, RequestAbsentForename, RequestAbsentSurname, RequestAbsentHoldingName)
											SELECT
												@CoverId,
												CAST(1 AS bit),
												CAST(1 AS bit),
												CAST(0 AS bit),
												Cover.Period,
												CommitmentName =
													CASE
														WHEN Classes.Name IS NOT NULL THEN Classes.Name
														WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
														ELSE NULL
													END,
												StaffNames.Forename,
												StaffNames.Surname,
												StaffHoldingNames.Name
											FROM
												Attendance
												INNER JOIN
													Cover
													ON Attendance.AttendanceId = Cover.AttendanceId
													AND Cover.CoverId = @CoverId
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
													TimetableRelations
													ON AttendanceStaff.StaffId = TimetableRelations.StaffId
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
													TimetableUniqueEntries
													ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId

										/** Part D **/
										INSERT INTO
											@ResponseTable
											(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname, SelectionHoldingName)
											SELECT
												StaffList.StaffId,
												CAST(0 AS bit),
												CAST(1 AS bit),
												CAST(0 AS bit),
												CoverCount.RemainingEntitlement,
												StaffNames.Forename,
												StaffNames.Surname,
												StaffHoldingNames.Name
											FROM
												Staff AS StaffList
												LEFT JOIN(
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
											WHERE
												StaffList.StaffId = @StaffId

										/** Part E **/
										SELECT CAST(1 AS bit) AS Status
									END
								ELSE
									BEGIN
										/** Cover request exists, but not the current staff ID **/

/** Part F **/
INSERT INTO
	@ResponseTable
	(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname, SelectionHoldingName)
	SELECT
		StaffList.StaffId,
		CAST(0 AS bit),
		CAST(1 AS bit),
		CAST(0 AS bit),
		CoverCount.RemainingEntitlement,
		StaffNames.Forename,
		StaffNames.Surname,
		StaffHoldingNames.Name
	FROM
		Staff AS StaffList
		LEFT JOIN(
				SELECT
					Staff.StaffId,
					((Staff.Entitlement - COUNT(InternalCover.CoverId)) + 1) AS RemainingEntitlement
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
		INNER JOIN
			InternalCover
			ON InternalCover.CoverId = @CoverId
		LEFT JOIN
			StaffNames
			ON StaffList.StaffId = StaffNames.StaffId
		LEFT JOIN
			StaffHoldingNames
			ON StaffList.StaffId = StaffHoldingNames.StaffId
	WHERE
		StaffList.StaffId = InternalCover.StaffId

/** Part G **/
UPDATE
	InternalCover
SET
	InternalCover.StaffId = @StaffId
WHERE
	InternalCover.CoverId = @CoverId

/** Part H **/
INSERT INTO
	@ResponseTable
	(Id, IsRequest, IsInternalCover, RequestHasCover, RequestPeriod, RequestCommitmentName, RequestAbsentForename, RequestAbsentSurname, RequestAbsentHoldingName, RequestCoverForename, RequestCoverSurname, RequestCoverHoldingName)
	SELECT
		@CoverId,
		CAST(1 AS bit),
		CAST(1 AS bit),
		CAST(1 AS bit),
		Cover.Period,
		CommitmentName =
			CASE
				WHEN Classes.Name IS NOT NULL THEN Classes.Name
				WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
				ELSE NULL
			END,
		AbsentName.Forename,
		AbsentName.Surname,
		AbsentHoldingName.Name,	
		CoveringName.Forename,
		CoveringName.Surname,
		CoveringHoldingName.Name
	FROM
		Attendance
		INNER JOIN
			Cover
			ON Attendance.AttendanceId = Cover.AttendanceId
			AND Cover.CoverId = @CoverId
		INNER JOIN
			AttendanceStaff
			ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
		LEFT JOIN
			StaffNames AS AbsentName
			ON AttendanceStaff.StaffId = AbsentName.StaffId
		LEFT JOIN
			StaffHoldingNames AS AbsentHoldingName
			ON AttendanceStaff.StaffId = AbsentHoldingName.StaffId
		LEFT JOIN
			StaffNames AS CoveringName
			ON CoveringName.StaffId = @StaffId
		LEFT JOIN
			StaffHoldingNames AS CoveringHoldingName
			ON CoveringHoldingName.StaffId = @StaffId
		LEFT JOIN
			TimetableRelations
			ON AttendanceStaff.StaffId = TimetableRelations.StaffId
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
			TimetableUniqueEntries
			ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId

/** Part I **/
INSERT INTO
	@ResponseTable
	(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname, SelectionHoldingName)
	SELECT
		StaffList.StaffId,
		CAST(0 AS bit),
		CAST(1 AS bit),
		CAST(1 AS bit),
		CoverCount.RemainingEntitlement,
		StaffNames.Forename,
		StaffNames.Surname,
		StaffHoldingNames.Name
	FROM
		Staff AS StaffList
		LEFT JOIN(
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
	WHERE
		StaffList.StaffId = @StaffId

/** Part J **/
SELECT CAST(1 AS bit) AS Status


									END
							END
						ELSE
							BEGIN
								/** Internal cover request does not exist **/
								
								/** Part K **/
								INSERT INTO
									InternalCover
									(CoverId, StaffId)
								VALUES
									(@CoverId, @StaffId)
								
								/** Part L **/
								INSERT INTO
									@ResponseTable
									(Id, IsRequest, IsInternalCover, RequestHasCover, RequestPeriod, RequestCommitmentName, RequestAbsentForename, RequestAbsentSurname, RequestAbsentHoldingName, RequestCoverForename, RequestCoverSurname, RequestCoverHoldingName)
									SELECT
										@CoverId,
										CAST(1 AS bit),
										CAST(1 AS bit),
										CAST(1 AS bit),
										Cover.Period,
										CommitmentName =
											CASE
												WHEN Classes.Name IS NOT NULL THEN Classes.Name
												WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
												ELSE NULL
											END,
										AbsentName.Forename,
										AbsentName.Surname,
										AbsentHoldingName.Name,	
										CoveringName.Forename,
										CoveringName.Surname,
										CoveringHoldingName.Name
									FROM
										Attendance
										INNER JOIN
											Cover
											ON Attendance.AttendanceId = Cover.AttendanceId
											AND Cover.CoverId = @CoverId
										INNER JOIN
											AttendanceStaff
											ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
										LEFT JOIN
											StaffNames AS AbsentName
											ON AttendanceStaff.StaffId = AbsentName.StaffId
										LEFT JOIN
											StaffHoldingNames AS AbsentHoldingName
											ON AttendanceStaff.StaffId = AbsentHoldingName.StaffId
										LEFT JOIN
											StaffNames AS CoveringName
											ON CoveringName.StaffId = @StaffId
										LEFT JOIN
											StaffHoldingNames AS CoveringHoldingName
											ON CoveringHoldingName.StaffId = @StaffId
										LEFT JOIN
											TimetableRelations
											ON AttendanceStaff.StaffId = TimetableRelations.StaffId
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
											TimetableUniqueEntries
											ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId

								/** Part M **/
								INSERT INTO
									@ResponseTable
									(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname, SelectionHoldingName)
									SELECT
										StaffList.StaffId,
										CAST(0 AS bit),
										CAST(1 AS bit),
										CAST(1 AS bit),
										CoverCount.RemainingEntitlement,
										StaffNames.Forename,
										StaffNames.Surname,
										StaffHoldingNames.Name
									FROM
										Staff AS StaffList
										LEFT JOIN(
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
									WHERE
										StaffList.StaffId = @StaffId

								/** Part N **/
								SELECT CAST(1 AS bit) AS Status
							END
					END
				ELSE
					BEGIN
						SELECT CAST(0 AS bit) AS Status
					END
					
			END
		ELSE
			BEGIN
				IF EXISTS(SELECT OutsideCoverStaff.OutsideCoverStaffId FROM OutsideCoverStaff WHERE OutsideCoverStaff.OutsideCoverStaffId = @StaffId)
					BEGIN
						/** Part O **/
						DELETE FROM InternalCover WHERE InternalCover.CoverId = @CoverId
						
						IF EXISTS(SELECT OutsideCover.Id FROM OutsideCover WHERE OutsideCover.CoverId = @CoverId)
							BEGIN
								IF EXISTS(SELECT OutsideCover.Id FROM OutsideCover WHERE OutsideCover.CoverId = @CoverId AND OutsideCover.OutsideCoverStaffId = @StaffId)
									BEGIN
										/** Part P **/
										DELETE FROM OutsideCover WHERE OutsideCover.CoverId = @CoverId
										
										/** Part Q **/
										INSERT INTO
											@ResponseTable
											(Id, IsRequest, IsInternalCover, RequestHasCover, RequestPeriod, RequestCommitmentName, RequestAbsentForename, RequestAbsentSurname, RequestAbsentHoldingName)
											SELECT
												@CoverId,
												CAST(1 AS bit),
												CAST(0 AS bit),
												CAST(0 AS bit),
												Cover.Period,
												CommitmentName =
													CASE
														WHEN Classes.Name IS NOT NULL THEN Classes.Name
														WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
														ELSE NULL
													END,
												StaffNames.Forename,
												StaffNames.Surname,
												StaffHoldingNames.Name
											FROM
												Attendance
												INNER JOIN
													Cover
													ON Attendance.AttendanceId = Cover.AttendanceId
													AND Cover.CoverId = @CoverId
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
													TimetableRelations
													ON AttendanceStaff.StaffId = TimetableRelations.StaffId
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
													TimetableUniqueEntries
													ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
										
										/** Part R **/
										INSERT INTO
											@ResponseTable
											(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname)
											SELECT
												StaffList.OutsideCoverStaffId,
												CAST(0 AS bit),
												CAST(0 AS bit),
												CAST(0 AS bit),
												CoverCount.RemainingEntitlement,
												StaffList.Forename,
												StaffList.Surname
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
											WHERE
												StaffList.OutsideCoverStaffId = @StaffId
										
										/** Part S **/
										SELECT CAST(1 AS bit) AS Status
									END
								ELSE
									BEGIN
										/** Part T **/
										INSERT INTO
											@ResponseTable
											(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname)
											SELECT
												StaffList.OutsideCoverStaffId,
												CAST(0 AS bit),
												CAST(0 AS bit),
												CAST(0 AS bit),
												CoverCount.RemainingEntitlement,
												StaffList.Forename,
												StaffList.Surname
											FROM
												OutsideCoverStaff AS StaffList
												LEFT JOIN(
														SELECT
															OutsideCoverStaff.OutsideCoverStaffId,
															((OutsideCoverStaff.Entitlement - COUNT(OutsideCover.CoverId)) + 1) AS RemainingEntitlement
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
												INNER JOIN
													OutsideCover
													ON OutsideCover.CoverId = @CoverId
											WHERE
												StaffList.OutsideCoverStaffId = OutsideCover.OutsideCoverStaffId
										
										/** Part U **/
										UPDATE
											OutsideCover
										SET
											OutsideCover.OutsideCoverStaffId = @StaffId
										WHERE
											OutsideCover.CoverId = @CoverId
										
										/** Part V **/
										INSERT INTO
											@ResponseTable
											(Id, IsRequest, IsInternalCover, RequestHasCover, RequestPeriod, RequestCommitmentName, RequestAbsentForename, RequestAbsentSurname, RequestAbsentHoldingName, RequestCoverForename, RequestCoverSurname)
											SELECT
												@CoverId,
												CAST(1 AS bit),
												CAST(0 AS bit),
												CAST(1 AS bit),
												Cover.Period,
												CommitmentName =
													CASE
														WHEN Classes.Name IS NOT NULL THEN Classes.Name
														WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
														ELSE NULL
													END,
												StaffNames.Forename,
												StaffNames.Surname,
												StaffHoldingNames.Name,
												CoverName.Forename,
												CoverName.Surname
											FROM
												Attendance
												INNER JOIN
													Cover
													ON Attendance.AttendanceId = Cover.AttendanceId
													AND Cover.CoverId = @CoverId
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
													OutsideCoverStaff AS CoverName
													ON CoverName.OutsideCoverStaffId = @StaffId
												LEFT JOIN
													TimetableRelations
													ON AttendanceStaff.StaffId = TimetableRelations.StaffId
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
													TimetableUniqueEntries
													ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
										
/** Part W **/
INSERT INTO
	@ResponseTable
	(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname)
	SELECT
		StaffList.OutsideCoverStaffId,
		CAST(0 AS bit),
		CAST(0 AS bit),
		CAST(1 AS bit),
		CoverCount.RemainingEntitlement,
		StaffList.Forename,
		StaffList.Surname
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
	WHERE
		StaffList.OutsideCoverStaffId = @StaffId

										
										/** Part X **/
										SELECT CAST(1 AS bit) AS Status
									END
							END
						ELSE
							BEGIN
								/** Part Y **/
								INSERT INTO
									OutsideCover
									(CoverId, OutsideCoverStaffId)
								VALUES
									(@CoverId, @StaffId)
								
								/** Part Z **/
								INSERT INTO
									@ResponseTable
									(Id, IsRequest, IsInternalCover, RequestHasCover, RequestPeriod, RequestCommitmentName, RequestAbsentForename, RequestAbsentSurname, RequestAbsentHoldingName, RequestCoverForename, RequestCoverSurname)
									SELECT
										@CoverId,
										CAST(1 AS bit),
										CAST(0 AS bit),
										CAST(1 AS bit),
										Cover.Period,
										CommitmentName =
											CASE
												WHEN Classes.Name IS NOT NULL THEN Classes.Name
												WHEN TimetableUniqueEntries.Name IS NOT NULL THEN TimetableUniqueEntries.Name
												ELSE NULL
											END,
										AbsentName.Forename,
										AbsentName.Surname,
										AbsentHoldingName.Name,
										CoverName.Forename,
										CoverName.Surname
									FROM
										Attendance
										INNER JOIN
											Cover
											ON Attendance.AttendanceId = Cover.AttendanceId
											AND Cover.CoverId = @CoverId
										INNER JOIN
											AttendanceStaff
											ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
										LEFT JOIN
											StaffNames AS AbsentName
											ON AttendanceStaff.StaffId = AbsentName.StaffId
										LEFT JOIN
											StaffHoldingNames AS AbsentHoldingName
											ON AttendanceStaff.StaffId = AbsentHoldingName.StaffId
										LEFT JOIN
											OutsideCoverStaff AS CoverName
											ON CoverName.OutsideCoverStaffId = @StaffId
										LEFT JOIN
											TimetableRelations
											ON AttendanceStaff.StaffId = TimetableRelations.StaffId
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
											TimetableUniqueEntries
											ON TimetableEntries.EntryId = TimetableUniqueEntries.EntryId
								
								/** Part AA **/
								INSERT INTO
									@ResponseTable
									(Id, IsRequest, IsInternalCover, SelectionIsSelected, SelectionEntitlement, SelectionForename, SelectionSurname)
									SELECT
										StaffList.OutsideCoverStaffId,
										CAST(0 AS bit),
										CAST(0 AS bit),
										CAST(1 AS bit),
										CoverCount.RemainingEntitlement,
										StaffList.Forename,
										StaffList.Surname
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
									WHERE
										StaffList.OutsideCoverStaffId = @StaffId
								
								/** Part AB **/
								SELECT CAST(1 AS bit) AS Status
							END
					END
				ELSE
					BEGIN
						SELECT CAST(0 AS bit) AS Status
					END
			END
	END
ELSE
	BEGIN
		SELECT CAST(0 AS bit) AS Status
	END

SELECT * FROM @ResponseTable