IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT Accounts.Id FROM Accounts WHERE Accounts.Username = @Username AND Accounts.StaffId <> @StaffId)
			BEGIN
				SELECT CAST(1 AS int) AS Status
			END
		ELSE
			BEGIN				
				IF (@HasAccount = 1)
					BEGIN
						IF EXISTS(SELECT Accounts.Id FROM Accounts WHERE Accounts.StaffId = @StaffId)
							BEGIN
								SELECT CAST(2 AS int) AS Status
								
								UPDATE
									Accounts
								SET
									Accounts.Username = @Username,
									Accounts.IsAdmin = @IsAdmin,
									Accounts.IsActive = @IsActive
								WHERE
									Accounts.StaffId = @StaffId
							END
						ELSE
							BEGIN
								SELECT CAST(3 AS int) AS Status
								
								INSERT INTO
									Accounts
									(StaffId, Username, Password, IsAdmin, IsActive)
								VALUES
									(@StaffId, @Username, @Password, @IsAdmin, @IsActive)
							END
					END
				ELSE
					BEGIN
						IF EXISTS(SELECT Accounts.Id FROM Accounts WHERE Accounts.StaffId = @StaffId)
							BEGIN
								IF (@CurrentStaffId = @StaffId)
									BEGIN
										SELECT CAST(6 AS int) AS Status
									END
								ELSE
									BEGIN
										SELECT CAST(4 AS int) AS Status
										
										DELETE FROM
											Accounts
										WHERE
											Accounts.StaffId = @StaffId
									END
							END
						ELSE
							BEGIN
								SELECT CAST(5 AS int) AS Status
							END
					END
			END
	END
ELSE
	BEGIN
		SELECT CAST(0 AS int) AS Status
	END