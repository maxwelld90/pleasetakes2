SELECT
	OutsideCoverStaff.OutsideCoverStaffId,
	OutsideCoverStaff.Forename,
	OutsideCoverStaff.Surname
FROM
	OutsideCoverStaff
WHERE
	OutsideCoverStaff.Forename LIKE '%' + @SearchTerm + '%'
	OR OutsideCoverStaff.Surname LIKE '%' + @SearchTerm + '%'
ORDER BY
	OutsideCoverStaff.Surname ASC