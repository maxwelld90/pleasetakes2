IF EXISTS(
	SELECT
		Staff.StaffId
	FROM
		Staff
	WHERE
		Staff.StaffId = @StaffId)
	
	BEGIN
		IF ((@HoldingName IS NULL) OR (@HoldingName = ''))
			BEGIN
				DELETE
				FROM
					StaffHoldingNames
				WHERE
					StaffHoldingNames.StaffId = @StaffId
			END
		ELSE
			BEGIN
				IF EXISTS(
					SELECT
						StaffHoldingNames.StaffId
					FROM
						StaffHoldingNames
					WHERE
						StaffHoldingNames.StaffId = @StaffId)
					
					BEGIN
						UPDATE
							StaffHoldingNames
						SET
							StaffHoldingNames.Name = @HoldingName
						WHERE
							StaffHoldingNames.StaffId = @StaffId
					END
				ELSE
					BEGIN
						INSERT INTO
							StaffHoldingNames
							(StaffId, Name)
						VALUES
							(@StaffId, @HoldingName)
					END
			END
		
		IF ((@Title IS NULL) OR (@Title = ''))
			BEGIN
				DELETE
				FROM
					StaffNames
				WHERE
					StaffNames.StaffId = @StaffId
			END
		ELSE
			BEGIN
				IF EXISTS(
					SELECT
						StaffNames.StaffId
					FROM
						StaffNames
					WHERE
						StaffNames.StaffId = @StaffId)
					
					BEGIN
						UPDATE
							StaffNames
						SET
							StaffNames.Title = @Title,
							StaffNames.Forename = @Forename,
							StaffNames.Surname = @Surname
						WHERE
							StaffNames.StaffId = @StaffId
					END
				ELSE
					BEGIN
						INSERT INTO
							StaffNames
							(StaffId, Title, Forename, Surname)
						VALUES
							(@StaffId, @Title, @Forename, @Surname)
					END
			END
		
		SELECT CAST(1 AS bit) AS Status
	END
ELSE
	BEGIN
		SELECT CAST(0 AS bit) AS Status
	END