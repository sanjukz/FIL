DECLARE @EventCategoryName VARCHAR(1000), @EventTypeName VARCHAR(1000), @Name VARCHAR(1000), @Description VARCHAR(MAX), @ClientPointOfContactId INT, @MetaDetails VARCHAR(MAX), @TermsAndConditions VARCHAR(MAX)

DECLARE EventCursor CURSOR FOR   
SELECT REPLACE(REPLACE(PD.P_Name, ' ',''), '&', 'And'), 
CASE ECD.EventFlowId WHEN 6 THEN 'Perennial' WHEN 1 THEN 'Tournament' ELSE 'Regular' END,
LTRIM(RTRIM(ECD.EventCatName)),
ECD.EventDesc,
ECD.MetaDetails,
ISNULL(ECD.TnC, '')
FROM LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventCategoryDetails ECD WITH (NOLOCK)
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTS_Product PD WITH (NOLOCK) ON PD.P_Id = ECD.P_Id
WHERE ECD.EventCatId IN (13,14,1758,1759,1760,1761,1762,1763,1764,1765,1766,1767,1769,1875,1908,2494,2550,2880,4361,4367,4371,4373,4376)

OPEN EventCursor  

FETCH NEXT FROM EventCursor   
INTO @EventCategoryName, @EventTypeName, @Name, @Description, @MetaDetails, @TermsAndConditions

WHILE @@FETCH_STATUS = 0  
BEGIN  
	DECLARE @EventId INT
	IF EXISTS(SELECT Id FROM Events WITH (NOLOCK) WHERE LTRIM(RTRIM(Name)) = @Name)
	BEGIN
		SELECT @EventId = Id 
		FROM Events WITH (NOLOCK) 
		WHERE LTRIM(RTRIM(Name)) = @Name
		PRINT 'Already Added'

		DELETE FROM EventKeywords WHERE @EventId = @EventId
	END
	ELSE
	BEGIN
	    INSERT INTO Events (AltId,EventCategoryId,EventTypeId,Name,Description,ClientPointOfContactId,FbEventId,MetaDetails,TermsAndConditions,IsEnabled,IsPublishedOnSite,PublishedDateTime,CreatedUtc,CreatedBy)
		SELECT 
		NEWID(), 
		(SELECT TOP 1 Id FROM EventCategories WHERE EventCategory = @EventCategoryName),
		(SELECT TOP 1 Id FROM EventTypes WHERE EventType = @EventTypeName),
		@Name, @Description, 
		(SELECT TOP 1 Id FROM ClientPointOfContacts WHERE Email = 'developer@kyazoonga.com'),
		NULL, @MetaDetails, @TermsAndConditions, 
		1, 1, GETUTCDATE(), GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' 		
		SET @EventId = SCOPE_IDENTITY()
		PRINT @Name + ' Added Sucessfully '
	END

	IF EXISTS(SELECT Id FROM Keywords WHERE Keywords = @EventCategoryName)
	BEGIN
		INSERT INTO EventKeywords (EventId,SearchKeywordId,IsEnabled,CreatedUtc,CreatedBy)
		VALUES (@EventId, (SELECT TOP 1 Id FROM Keywords WHERE Keywords = @EventCategoryName), 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
	END

	IF EXISTS(SELECT Id FROM Keywords WHERE Keywords = @EventTypeName)
	BEGIN
		INSERT INTO EventKeywords (EventId,SearchKeywordId,IsEnabled,CreatedUtc,CreatedBy)
		VALUES (@EventId, (SELECT TOP 1 Id FROM Keywords WHERE Keywords = @EventTypeName), 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
	END

	FETCH NEXT FROM EventCursor   
	INTO @EventCategoryName, @EventTypeName, @Name, @Description, @MetaDetails, @TermsAndConditions
END   
CLOSE EventCursor;  
DEALLOCATE EventCursor;  