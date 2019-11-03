SELECT
	YeargroupsShared.SharedId AS Id,
	YeargroupsShared.Name AS Name
FROM
	YeargroupsShared
WHERE
	YeargroupsShared.Name LIKE '%' + @SearchTerm + '%'
ORDER BY
	YeargroupsShared.Name ASC