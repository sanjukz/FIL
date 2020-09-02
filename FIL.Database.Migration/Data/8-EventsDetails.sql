DECLARE @EventCategoryName VARCHAR(1000), @VenueAddress VARCHAR(1000), @Name VARCHAR(1000), @StartDateTime DATETIME, @EndDateTime DATETIME,
@IsPrintathome INT, @IsVenuePickUp INT, @IsMTicket INT, @IsCourier INT, @PAHNotes VARCHAR(MAX), @PickUpnotes VARCHAR(MAX), @MTicketNotes VARCHAR(MAX), @CourierNotes VARCHAR(MAX),
@Match_No INT, @GateOpenTime NVARCHAR(20), @EventTimeZone NVARCHAR(20), @Matchday INT, @TimeZoneAbbreviation NVARCHAR(20), @TicketHtml NVARCHAR(20)

DECLARE EventCursor CURSOR FOR   
SELECT LTRIM(RTRIM(EventCatName)), LTRIM(RTRIM(VenueAddress)), LTRIM(RTRIM(EventName)), CONVERT(DATETIME,(CONVERT(VARCHAR, CONVERT(DATE, EventStartDate)) + ' ' + EventStarttime)), CONVERT(DATETIME,(CONVERT(VARCHAR, CONVERT(DATE, EventEndDate)) + ' ' + EventEndTime)),
ISNULL(IsPrintathome,0), ISNULL(IsVenuePickUp,0), ISNULL(IsMTicket,0), CASE WHEN CourierDate >= GETDATE() THEN 1 ELSE 0 END, PAHNotes, PickUpnotes, MTicketNotes, CourierNotes,
ISNULL(Match_No,0), ISNULL(GateOpenTime,''), ISNULL(EventTimeZone,''), ISNULL(Matchday,0), ISNULL(TimeZoneAbbreviation,'IST'), ISNULL(TicketHtml,'')
FROM LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventCategoryDetails ECD WITH (NOLOCK)
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventDetails ED WITH (NOLOCK) ON ECD.EventCatId = ED.EventType
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueDetails VD ON ED.EventVenue = VD.VenueId
WHERE ECD.EventCatId IN (13,14,1758,1759,1760,1761,1762,1763,1764,1765,1766,1767,1769,1875,1908,2494,2550,2880,4361,4367,4371,4373,4376)
AND EventStartDate >= CASE EventFlowId WHEN 6 THEN GETDATE() ELSE EventStartDate END
ORDER BY ECD.EventCatId

OPEN EventCursor  

FETCH NEXT FROM EventCursor    
INTO @EventCategoryName, @VenueAddress, @Name, @StartDateTime, @EndDateTime, @IsPrintathome, @IsVenuePickUp, @IsMTicket, @IsCourier, @PAHNotes, @PickUpnotes, @MTicketNotes, @CourierNotes,
@Match_No, @GateOpenTime, @EventTimeZone, @Matchday, @TimeZoneAbbreviation, @TicketHtml

WHILE @@FETCH_STATUS = 0  
BEGIN  
	DECLARE @EventDetailId INT
	IF EXISTS(SELECT ED.Id 
				FROM EventDetails ED WITH (NOLOCK) 
				INNER JOIN Events E ON ED.EventId = E.Id
				INNER JOIN Venues V ON ED.VenueId = V.Id 
				WHERE LTRIM(RTRIM(ED.Name)) = @Name 
				AND LTRIM(RTRIM(E.Name)) = @EventCategoryName 
				AND LTRIM(RTRIM(V.Name)) = @VenueAddress
				AND ED.StartDateTime = @StartDateTime
				AND ED.EndDateTime = @EndDateTime)
	BEGIN
		SELECT @EventDetailId = ED.Id 
		FROM EventDetails ED WITH (NOLOCK) 
		INNER JOIN Events E ON ED.EventId = E.Id
		INNER JOIN Venues V ON ED.VenueId = V.Id 
		WHERE LTRIM(RTRIM(ED.Name)) = @Name 
		AND LTRIM(RTRIM(E.Name)) = @EventCategoryName 
		AND LTRIM(RTRIM(V.Name)) = @VenueAddress
		AND ED.StartDateTime = @StartDateTime
		AND ED.EndDateTime = @EndDateTime
		PRINT CONVERT(VARCHAR, @EventDetailId) + ' Already Added'	

		DELETE FROM EventDeliveryTypeDetails WHERE EventDetailId = @EventDetailId
	END
	ELSE
	BEGIN
	    INSERT INTO EventDetails (Name,EventId,VenueId,StartDateTime,EndDateTime,GroupId,IsEnabled,CreatedUtc,CreatedBy)
		SELECT 
		@Name,
		(SELECT TOP 1 Id FROM Events WHERE Name = @EventCategoryName),
		(SELECT TOP 1 Id FROM Venues WHERE Name = @VenueAddress),
		@StartDateTime, @EndDateTime, 1, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' 		
		SET @EventDetailId = SCOPE_IDENTITY()
		PRINT @Name + ' Added Sucessfully '
	END

	IF(@IsPrintathome = 1)
	BEGIN
		INSERT INTO EventDeliveryTypeDetails (EventDetailId,DeliveryTypeId,Notes,EndDate,IsEnabled,CreatedUtc,CreatedBy)
		VALUES (@EventDetailId, 4, @PAHNotes, @EndDateTime, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
	END

	IF(@IsVenuePickUp = 1)
	BEGIN
		INSERT INTO EventDeliveryTypeDetails (EventDetailId,DeliveryTypeId,Notes,EndDate,IsEnabled,CreatedUtc,CreatedBy)
		VALUES (@EventDetailId, 2, @PickUpnotes, @EndDateTime, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
	END

	IF(@IsMTicket = 1)
	BEGIN
		INSERT INTO EventDeliveryTypeDetails (EventDetailId,DeliveryTypeId,Notes,EndDate,IsEnabled,CreatedUtc,CreatedBy)
		VALUES (@EventDetailId, 8, @MTicketNotes, @EndDateTime, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
	END

	IF(@IsCourier = 1)
	BEGIN
		INSERT INTO EventDeliveryTypeDetails (EventDetailId,DeliveryTypeId,Notes,EndDate,IsEnabled,CreatedUtc,CreatedBy)
		VALUES (@EventDetailId, 1, @CourierNotes, @EndDateTime, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
	END

	IF(@Match_No <> 0 OR @Matchday <> 0 OR @GateOpenTime <> '' OR @EventTimeZone <> '' OR @TicketHtml <> '')
	BEGIN
		INSERT INTO EventAttributes (EventDetailId,MatchNo,MatchDay,GateOpenTime,TimeZone,TimeZoneAbbreviation,TicketHtml,IsEnabled,CreatedUtc,CreatedBy)
		VALUES (@EventDetailId, @Match_No, @Matchday, @GateOpenTime, @EventTimeZone, @TimeZoneAbbreviation, @TicketHtml, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
		PRINT CONVERT(VARCHAR, @EventDetailId) + ' Attribute Added Sucessfully '
	END
		
	FETCH NEXT FROM EventCursor   
	INTO @EventCategoryName, @VenueAddress, @Name, @StartDateTime, @EndDateTime, @IsPrintathome, @IsVenuePickUp, @IsMTicket, @IsCourier, @PAHNotes, @PickUpnotes, @MTicketNotes, @CourierNotes,
	@Match_No, @GateOpenTime, @EventTimeZone, @Matchday, @TimeZoneAbbreviation, @TicketHtml
END   
CLOSE EventCursor;  
DEALLOCATE EventCursor;  