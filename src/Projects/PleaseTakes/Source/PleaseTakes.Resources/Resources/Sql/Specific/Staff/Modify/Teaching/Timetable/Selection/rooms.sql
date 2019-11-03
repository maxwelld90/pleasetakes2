SELECT
	Rooms.RoomId AS Id,
	Rooms.Name AS Name
FROM
	Rooms
WHERE
	Rooms.Name LIKE '%' + @SearchTerm + '%'
ORDER BY
	Rooms.Name ASC