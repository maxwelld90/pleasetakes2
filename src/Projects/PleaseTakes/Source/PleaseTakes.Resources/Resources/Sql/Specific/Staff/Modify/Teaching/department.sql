IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		SELECT CAST(1 AS bit) AS Status
		
		SELECT
			Departments.DepartmentId AS Id,
			Departments.Name,
			IsSelected =
				CASE
					WHEN (DepartmentsStaff.DepartmentId = Departments.DepartmentId) THEN CAST(1 AS bit)
					ELSE CAST(0 AS bit)
				END
		FROM
			Departments
			LEFT JOIN
				DepartmentsStaff
				ON DepartmentsStaff.StaffId = @StaffId
		WHERE
			Departments.Name LIKE '%' + @SearchTerm + '%'
			OR Departments.Abbreviation LIKE '%' + @SearchTerm + '%'
		ORDER BY
			Departments.Name ASC
	END
ELSE
	BEGIN
		SELECT CAST(0 AS bit) AS Status
	END