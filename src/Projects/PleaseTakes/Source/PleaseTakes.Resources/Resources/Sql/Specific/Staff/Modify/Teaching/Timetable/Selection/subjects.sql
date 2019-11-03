SELECT
	Subjects.SubjectId AS Id,
	Subjects.Name,
	Subjects.Abbreviation
FROM
	Subjects
WHERE
	Subjects.Name LIKE '%' + @SearchTerm + '%'
ORDER BY
	Subjects.Name ASC