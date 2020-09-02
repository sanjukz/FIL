DECLARE @CategoryName VARCHAR(1000)

DECLARE TicketCategoriesCursor CURSOR FOR   
SELECT DISTINCT LTRIM(RTRIM(VenueCatName)) AS VenueCatName 
FROM LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueMapCategoryClass VMCC WITH (NOLOCK) 
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueCategoryDetails VCD WITH (NOLOCK) ON VMCC.VenueCatID = VCD.VenueCatId
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventDetails ED WITH (NOLOCK) ON VMCC.EventId = ED.EventId
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventCategoryDetails ECD WITH (NOLOCK) ON ED.EventType = ECD.EventCatId
WHERE ECD.EventCatId IN (13,14,1758,1759,1760,1761,1762,1763,1764,1765,1766,1767,1769,1875,1908,2494,2550,2880,4361,4367,4371,4373,4376)
ORDER BY VenueCatName

OPEN TicketCategoriesCursor  

FETCH NEXT FROM TicketCategoriesCursor   
INTO @CategoryName

WHILE @@FETCH_STATUS = 0  
BEGIN  
	IF NOT EXISTS(SELECT Id FROM TicketCategories WHERE LTRIM(RTRIM(Name)) = @CategoryName)	
	BEGIN
		INSERT INTO TicketCategories (Name,IsEnabled,CreatedUtc,CreatedBy)
		SELECT TOP 1 @CategoryName, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' 
		FROM LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueCategoryDetails WITH (NOLOCK) 
		WHERE LTRIM(RTRIM(VenueCatName)) = @CategoryName		
		PRINT @CategoryName + ' Added Sucessfully '
	END	
	ELSE
	BEGIN
		PRINT 'Already Added'
	END

	FETCH NEXT FROM TicketCategoriesCursor   
	INTO @CategoryName
END   
CLOSE TicketCategoriesCursor;  
DEALLOCATE TicketCategoriesCursor;  