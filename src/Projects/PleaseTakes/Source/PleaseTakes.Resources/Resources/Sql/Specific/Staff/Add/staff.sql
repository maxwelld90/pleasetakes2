INSERT INTO
	Staff
	(Entitlement)
	VALUES
	(0)

DECLARE @NewId int
SET @NewId = SCOPE_IDENTITY()

INSERT INTO
	StaffHoldingNames
	(StaffId, Name)
	VALUES
	(@NewId, 'New Staff Member')

SELECT @NewId AS NewTeachingStaffId