SELECT
	Staff.StaffId,
	Staff.Entitlement,
	StaffNames.Title,
	StaffNames.Forename,
	StaffNames.Surname,
	StaffHoldingNames.Name AS HoldingName,
	HasAccount =
		CASE
			WHEN Accounts.Id IS NOT NULL THEN CAST(1 AS bit)
			ELSE CAST(0 AS bit)
		END,
	IsAdmin =
		CASE
			WHEN Accounts.IsAdmin IS NOT NULL THEN Accounts.IsAdmin
			ELSE CAST(0 AS bit)
		END,
	IsActive =
		CASE
			WHEN Accounts.IsActive IS NOT NULL THEN Accounts.IsActive
			ELSE CAST(0 AS bit)
		END,
	Accounts.Username
FROM
	Staff
	LEFT JOIN
		StaffNames
		ON Staff.StaffId = StaffNames.StaffId
	LEFT JOIN
		StaffHoldingNames
		ON Staff.StaffId = StaffHoldingNames.StaffId
	LEFT JOIN
		Accounts
		ON Staff.StaffId = Accounts.StaffId
WHERE
	Staff.StaffId = @StaffId