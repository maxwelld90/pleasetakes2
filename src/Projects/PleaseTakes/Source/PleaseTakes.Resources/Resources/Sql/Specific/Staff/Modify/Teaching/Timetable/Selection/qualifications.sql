SELECT DISTINCT
	Qualifications.Name,
	SubjectQualifications.SubjectQualificationId AS Id
FROM
	Qualifications
	INNER JOIN
		SubjectQualifications
		ON Qualifications.QualificationId = SubjectQualifications.QualificationId
WHERE
	SubjectQualifications.SubjectId = @SubjectId
	AND Qualifications.Name LIKE '%' + @SearchTerm + '%'
ORDER BY
	Qualifications.Name ASC