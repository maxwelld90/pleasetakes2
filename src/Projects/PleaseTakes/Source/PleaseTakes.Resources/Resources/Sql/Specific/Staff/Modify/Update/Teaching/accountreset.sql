IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT Accounts.Id FROM Accounts WHERE Accounts.StaffId = @StaffId)
			BEGIN
				SELECT CAST(2 AS int) AS Status
				
				UPDATE
					Accounts
				SET
					Accounts.Password = @Password
				WHERE
					Accounts.StaffId = @StaffId
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