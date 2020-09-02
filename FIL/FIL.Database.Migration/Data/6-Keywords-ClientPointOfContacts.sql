IF NOT EXISTS(SELECT * FROM ClientPointOfContacts WHERE Email = 'developer@kyazoonga.com')
BEGIN
	INSERT INTO ClientPointOfContacts (AltId,Name,Email,PhoneNumber,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
	VALUES
	(NEWID(), 'Developer', 'developer@kyazoonga.com', '44881061', 1, GETUTCDATE(), NULL, 'C043DDEE-D0B1-48D8-9C3F-309A77F44781', NULL)
END

IF NOT EXISTS(SELECT * FROM Keywords)
BEGIN
	INSERT INTO Keywords (Keywords,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
	SELECT EventCategory, 1, GETUTCDATE(), NULL, 'C043DDEE-D0B1-48D8-9C3F-309A77F44781', NULL FROM EventCategories

	INSERT INTO Keywords (Keywords,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
	SELECT EventType, 1, GETUTCDATE(), NULL, 'C043DDEE-D0B1-48D8-9C3F-309A77F44781', NULL FROM EventTypes
END