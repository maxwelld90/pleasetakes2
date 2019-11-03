SELECT
	Classes.ClassId AS Id,
	Classes.Name
FROM
	Classes
	INNER JOIN
		SubjectQualifications
		ON Classes.SubjectQualificationId = SubjectQualifications.SubjectQualificationId
WHERE
	SubjectQualifications.SubjectQualificationId = @SubjectQualificationId
	AND Classes.Name LIKE '%' + @SearchTerm + '%'