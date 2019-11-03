SELECT DISTINCT
	Staff.StaffId,
	StaffNames.Forename,
	StaffNames.Surname,
	StaffHoldingNames.Name AS HoldingName,
	Departments.Name AS DepartmentName
FROM
	Staff
	LEFT JOIN
		StaffNames
		ON Staff.StaffId = StaffNames.StaffId
	LEFT JOIN
		StaffHoldingNames
		ON Staff.StaffId = StaffHoldingNames.StaffId
	LEFT JOIN
		DepartmentsStaff
		ON Staff.StaffId = DepartmentsStaff.StaffId
	LEFT JOIN
		Departments
		ON Departments.DepartmentId = DepartmentsStaff.DepartmentId
WHERE
	StaffNames.Forename LIKE '%' + @SearchTerm + '%'
	OR StaffNames.Surname LIKE '%' + @SearchTerm + '%'
	OR StaffHoldingNames.Name LIKE '%' + @SearchTerm + '%'
	OR Departments.Name LIKE '%' + @SearchTerm + '%'
	OR Departments.Abbreviation LIKE '%' + @SearchTerm + '%'
ORDER BY
	StaffNames.Surname