SELECT
	Accounts.Username,
	Accounts.Password,
	Accounts.IsAdmin,
	Accounts.IsActive,
	Staff.StaffId,
	StaffNames.Title,
	StaffNames.Forename,
	StaffNames.Surname,
	StaffHoldingNames.Name AS HoldingName,
	TimetableRelations.TimetableId

FROM
	Staff
	INNER JOIN
		Accounts ON Staff.StaffId = Accounts.StaffId LEFT JOIN
		TimetableRelations ON Staff.StaffId = TimetableRelations.StaffId LEFT OUTER JOIN
		StaffNames ON Staff.StaffId = StaffNames.StaffId LEFT OUTER JOIN
		StaffHoldingNames ON Staff.StaffId = StaffHoldingNames.StaffId

WHERE
	((Accounts.Username = @Username) AND (Accounts.Password = @Password))