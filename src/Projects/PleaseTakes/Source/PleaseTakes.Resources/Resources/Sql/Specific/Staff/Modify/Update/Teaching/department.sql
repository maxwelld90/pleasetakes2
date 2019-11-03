DECLARE
	@ResultsTable TABLE(
		DepartmentId int,
		ClassName varchar(6))

IF EXISTS(SELECT Staff.StaffId FROM Staff WHERE Staff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT Departments.DepartmentId FROM Departments WHERE Departments.DepartmentId = @DepartmentId)
			BEGIN
				SELECT CAST(2 AS int) AS Status

IF EXISTS(SELECT DepartmentsStaff.StaffId FROM DepartmentsStaff WHERE DepartmentsStaff.StaffId = @StaffId)
	BEGIN
		IF EXISTS(SELECT DepartmentsStaff.StaffId FROM DepartmentsStaff WHERE DepartmentsStaff.StaffId = @StaffId AND DepartmentsStaff.DepartmentId = @DepartmentId)
			BEGIN
				INSERT INTO
					@ResultsTable
					(DepartmentId, ClassName)
					SELECT
						DepartmentId,
						'Blue'
					FROM
						DepartmentsStaff
					WHERE
						DepartmentsStaff.StaffId = @StaffId
				
				DELETE FROM
					DepartmentsStaff
				WHERE
					DepartmentsStaff.StaffId = @StaffId
			END
		ELSE
			BEGIN
				INSERT INTO
					@ResultsTable
					(DepartmentId, ClassName)
					SELECT
						DepartmentId,
						'Blue'
					FROM
						DepartmentsStaff
					WHERE
						DepartmentsStaff.StaffId = @StaffId
				
				DELETE FROM
					DepartmentsStaff
				WHERE
					DepartmentsStaff.StaffId = @StaffId
				
				INSERT INTO
					@ResultsTable
					(DepartmentId, ClassName)
				VALUES
					(@DepartmentId, 'Green')
				
				INSERT INTO
					DepartmentsStaff
					(DepartmentId, StaffId)
				VALUES
					(@DepartmentId, @StaffId)
			END
	END
ELSE
	BEGIN
		INSERT INTO
			DepartmentsStaff
			(DepartmentId, StaffId)
		VALUES
			(@DepartmentId, @StaffId)
		
		INSERT INTO
			@ResultsTable
			(DepartmentId, ClassName)
		VALUES
			(@DepartmentId, 'Green')
	END

			END
		ELSE
			BEGIN
				SELECT CAST(1 AS int) AS Status
			END
	END
ELSE
	BEGIN
		SELECT CAST(0 AS int) AS Status
	END

SELECT * FROM @ResultsTable