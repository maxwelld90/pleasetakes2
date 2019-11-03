IF EXISTS(
	SELECT
		Staff.StaffId
	FROM
		Staff
	WHERE
		Staff.StaffId = @StaffId)

	BEGIN
		IF EXISTS(
			SELECT
				Attendance.AttendanceId
			FROM
				Attendance
				INNER JOIN
					AttendanceStaff
					ON Attendance.AttendanceId = AttendanceStaff.AttendanceId
					AND Attendance.Date = @Date
					AND AttendanceStaff.StaffId = @StaffId)

			BEGIN
				DELETE
				FROM
					Attendance
				WHERE
					Attendance.Date = @Date AND
					Attendance.AttendanceId IN (
						SELECT
							AttendanceStaff.AttendanceId
						FROM
							AttendanceStaff
						WHERE
							AttendanceStaff.StaffId = @StaffId)
				
				SELECT '' AS Colour;
			END
		ELSE
			BEGIN
				DECLARE @NewAttendanceId int
				
				INSERT INTO
					Attendance
						(Date)
					VALUES
						(@Date)
				
				SET @NewAttendanceId = SCOPE_IDENTITY()
				
				INSERT INTO
					AttendanceStaff
						(AttendanceId, StaffId)
					VALUES
						(@NewAttendanceId, @StaffId)

				SELECT 'Red' AS Colour;
			END
	END
ELSE
	BEGIN
		Select 'Yellow' AS Colour;
	END