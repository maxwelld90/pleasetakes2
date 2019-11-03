IF EXISTS(
	SELECT
		Staff.StaffId
	FROM
		Staff
	WHERE
		Staff.StaffId = @StaffId)
	
	BEGIN
		IF NOT EXISTS(
			SELECT
				Staff.Entitlement
			FROM
				Staff
			WHERE
				Staff.StaffId = @StaffId
				AND Staff.Entitlement = @Entitlement)
			
			BEGIN
				UPDATE
					Staff
				SET
					Staff.Entitlement = @Entitlement
				WHERE
					Staff.StaffId = @StaffId
			END
		
		SELECT CAST(1 AS bit) AS Status
	END
ELSE
	BEGIN
		SELECT CAST(0 AS bit) AS Status
	END