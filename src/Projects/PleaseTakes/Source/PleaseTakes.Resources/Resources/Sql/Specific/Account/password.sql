UPDATE
	Accounts
SET
	Accounts.Password = @NewPassword
WHERE
	Accounts.StaffId = @StaffId