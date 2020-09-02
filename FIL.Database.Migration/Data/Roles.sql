--KzSuite Admin
IF NOT EXISTS(SELECT * FROM Roles WHERE RoleName = 'Admin' AND PermissionId = 15 AND ModuleId = 6)
BEGIN
	INSERT INTO Roles (RoleName,PermissionId,ModuleId,IsEnabled,CreatedUtc,CreatedBy)
	VALUES ('Admin', 15, 6, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END

IF NOT EXISTS(SELECT * FROM Roles WHERE RoleName = 'User' AND PermissionId = 5 AND ModuleId = 1)
BEGIN
	INSERT INTO Roles (RoleName,PermissionId,ModuleId,IsEnabled,CreatedUtc,CreatedBy)
	VALUES ('User', 5, 3, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END

