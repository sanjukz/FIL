DECLARE @EventCategoryName VARCHAR(1000), @VenueAddress VARCHAR(1000), @Name VARCHAR(1000), @StartDateTime DATETIME, @EndDateTime DATETIME, @SalesStartDateTime DATETIME, @SalesEndDateTime DATETIME,
@VenueCatName VARCHAR(1000), @AvailableTicketForSale INT, @RemainingTicketForSale INT, @TicketCategoryDescription VARCHAR(1000), @ViewFromStand VARCHAR(1000), @IsSeatSelection BIT,
@Price DECIMAL(18,2), @LocalPrice DECIMAL(18,2), @IsInternationalCardAllowed BIT, @IsEMIApplicable BIT, @ConvenceCharge DECIMAL(18,2), @TicketServiceTax DECIMAL(18,2), 
@AddtitionalConvenceCharge DECIMAL(18,2), @TicketSMSCharge DECIMAL(18,2)

DECLARE EventCursor CURSOR FOR   
SELECT LTRIM(RTRIM(EventCatName)), LTRIM(RTRIM(VenueAddress)), LTRIM(RTRIM(EventName)), CONVERT(DATETIME,(CONVERT(VARCHAR, CONVERT(DATE, EventStartDate)) + ' ' + EventStarttime)), CONVERT(DATETIME,(CONVERT(VARCHAR, CONVERT(DATE, EventEndDate)) + ' ' + EventEndTime)),
GETUTCDATE(), CONVERT(DATETIME,(CONVERT(VARCHAR, CONVERT(DATE, EventEndDate)) + ' ' + EventEndTime)),
LTRIM(RTRIM(VenueCatName)), TotalTic, TicketForSale, LTRIM(RTRIM(VenueCatDetail)), ISNULL(ViewFromStand,''), ISNULL(IsLayoutAvail,0), ISNULL(VMCC.PricePerTic, 0), ISNULL(VMCC.LocalPricePerTic, 0),
ISNULL(IsInternationalCardAllowed, 0), ISNULL(IsEMIApplicable, 0),
ISNULL(REPLACE(CM.ConvenceCharge, '%',''),0), ISNULL(REPLACE(CM.TicketServiceTax, '%',''),0), ISNULL(REPLACE(CM.AddtitionalConvenceCharge, '%',''),0), 
ISNULL(REPLACE(CM.TicketSMSCharge, '%',''),0)
FROM LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventCategoryDetails ECD WITH (NOLOCK)
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventDetails ED WITH (NOLOCK) ON ECD.EventCatId = ED.EventType
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueDetails VD ON ED.EventVenue = VD.VenueId
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueMapCategoryClass VMCC WITH (NOLOCK) ON VMCC.EventId = ED.EventId 
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueCategoryDetails VCD WITH (NOLOCK) ON VCD.VenueCatID = VMCC.VenueCatId
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTS_CurrencyMapping CM WITH (NOLOCK) ON CM.VMCC_Id = VMCC.VMCC_Id
WHERE ECD.EventCatId IN (13,14,1758,1759,1760,1761,1762,1763,1764,1765,1766,1767,1769,1875,1908,2494,2550,2880,4361,4367,4371,4373,4376)
AND EventStartDate >= CASE EventFlowId WHEN 6 THEN GETDATE() ELSE EventStartDate END
ORDER BY ECD.EventCatId

OPEN EventCursor  

FETCH NEXT FROM EventCursor    
INTO @EventCategoryName, @VenueAddress, @Name, @StartDateTime, @EndDateTime, @SalesStartDateTime, @SalesEndDateTime, @VenueCatName, @AvailableTicketForSale, @RemainingTicketForSale, @TicketCategoryDescription, 
@ViewFromStand, @IsSeatSelection, @Price, @LocalPrice, @IsInternationalCardAllowed, @IsEMIApplicable, @ConvenceCharge, @TicketServiceTax, @AddtitionalConvenceCharge, @TicketSMSCharge

WHILE @@FETCH_STATUS = 0  
BEGIN  
	DECLARE @EventDetailId INT, @EventTicketDetailsId INT, @EventTicketAttributesId INT, @TicketTypeId INT = 1, @ChannelId INT = 31, @CurrencyId INT = 21, @LocalCurrencyId INT = 18
	SELECT @TicketTypeId = Id FROM TicketTypes WHERE TicketType = 'Regular'
	SELECT @ChannelId = Id FROM Channels WHERE Channels = 'Website, Retail, Boxoffice, Corporate, MobileApp'
	SELECT @CurrencyId = Id FROM CurrencyTypes WHERE Code = 'INR'
	SELECT @LocalCurrencyId = Id FROM CurrencyTypes WHERE Code = 'XCD'
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
	
		IF EXISTS(SELECT * FROM EventTicketDetails WHERE EventDetailId = @EventDetailId AND TicketCategoryId = (SELECT TOP 1 Id FROM TicketCategories WHERE Name = @VenueCatName))
		BEGIN
			SELECT @EventTicketDetailsId = ETD.Id 
			FROM EventTicketDetails ETD WITH (NOLOCK) 		
			WHERE ETD.EventDetailId = @EventDetailId AND ETD.TicketCategoryId = (SELECT TOP 1 Id FROM TicketCategories WHERE Name = @VenueCatName)	
			PRINT CONVERT(VARCHAR, @EventTicketDetailsId) + ' Already Added'	
		END
		ELSE
		BEGIN
			INSERT INTO EventTicketDetails (EventDetailId,TicketCategoryId,IsEnabled,CreatedUtc,CreatedBy)
			VALUES (@EventDetailId, (SELECT TOP 1 Id FROM TicketCategories WHERE Name = @VenueCatName), 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
			SET @EventTicketDetailsId = SCOPE_IDENTITY()
			PRINT CONVERT(VARCHAR, @EventTicketDetailsId) + ' Added Sucessfully'	
		END

		IF EXISTS (SELECT * FROM EventTicketAttributes WHERE EventTicketDetailId = @EventTicketDetailsId AND SalesEndDatetime = @SalesEndDateTime
		AND TicketTypeId = @TicketTypeId AND ChannelId = @ChannelId AND CurrencyId = @CurrencyId)
		BEGIN
			SELECT @EventTicketAttributesId = ETA.Id 
			FROM EventTicketAttributes ETA WITH (NOLOCK) 		
			WHERE ETA.EventTicketDetailId = @EventTicketDetailsId AND ETA.SalesEndDatetime = @SalesEndDateTime AND ETA.TicketTypeId = @TicketTypeId AND ETA.ChannelId = @ChannelId AND ETA.CurrencyId = @CurrencyId
			DELETE FROM TicketFeeDetails WHERE EventTicketAttributeId = @EventTicketAttributesId
		END
		ELSE
		BEGIN
			INSERT INTO EventTicketAttributes (EventTicketDetailId,SalesStartDateTime,SalesEndDatetime,TicketTypeId,ChannelId,CurrencyId,AvailableTicketForSale,RemainingTicketForSale,
			TicketCategoryDescription,ViewFromStand,IsSeatSelection,Price,IsInternationalCardAllowed,IsEMIApplicable,IsEnabled,CreatedUtc,CreatedBy)
			VALUES (@EventTicketDetailsId, @SalesStartDateTime, @SalesEndDateTime, @TicketTypeId, @ChannelId, @CurrencyId, @AvailableTicketForSale, @RemainingTicketForSale, @TicketCategoryDescription,
			@ViewFromStand, @IsSeatSelection, @Price, 0, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
			SET @EventTicketAttributesId = SCOPE_IDENTITY()
		END

		IF(@EventTicketAttributesId IS NOT NULL)
		BEGIN
			IF(@ConvenceCharge <> 0)
			BEGIN
				INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
				VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'ConvenienceCharge'), 'Handling charges', 
				(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @ConvenceCharge, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
			END

			IF(@TicketServiceTax <> 0)
			BEGIN
				INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
				VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'ServiceCharge'), 'Service charges', 
				(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @TicketServiceTax, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
			END

			IF(@AddtitionalConvenceCharge <> 0)
			BEGIN
				INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
				VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'OtherCharge'), 'Bank', 
				(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @AddtitionalConvenceCharge, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
			END

			IF(@TicketSMSCharge <> 0)
			BEGIN
				INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
				VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'PrintAtHomeCharge'), 'Bank', 
				(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @TicketSMSCharge, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
			END

			IF(@LocalPrice <> 0)
			BEGIN
				IF NOT EXISTS (SELECT * FROM EventTicketAttributes WHERE EventTicketDetailId = @EventTicketDetailsId AND SalesEndDatetime = @SalesEndDateTime
				AND TicketTypeId = @TicketTypeId AND ChannelId = @ChannelId AND CurrencyId = @LocalCurrencyId)
				BEGIN
					SELECT @EventTicketAttributesId = ETA.Id 
					FROM EventTicketAttributes ETA WITH (NOLOCK) 		
					WHERE ETA.EventTicketDetailId = @EventTicketDetailsId AND ETA.SalesEndDatetime = @SalesEndDateTime AND ETA.TicketTypeId = @TicketTypeId AND ETA.ChannelId = @ChannelId AND ETA.CurrencyId = @LocalCurrencyId
				END
				ELSE
				BEGIN
					INSERT INTO EventTicketAttributes (EventTicketDetailId,SalesStartDateTime,SalesEndDatetime,TicketTypeId,ChannelId,CurrencyId,AvailableTicketForSale,RemainingTicketForSale,
					TicketCategoryDescription,ViewFromStand,IsSeatSelection,Price,IsInternationalCardAllowed,IsEMIApplicable,IsEnabled,CreatedUtc,CreatedBy)
					VALUES (@EventTicketDetailsId, @SalesStartDateTime, @SalesEndDateTime, @TicketTypeId, @ChannelId, @LocalCurrencyId, @AvailableTicketForSale, @RemainingTicketForSale, @TicketCategoryDescription,
					@ViewFromStand, @IsSeatSelection, @Price, 0, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
					SET @EventTicketAttributesId = SCOPE_IDENTITY()
				END

				IF(@ConvenceCharge <> 0)
				BEGIN
					INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
					VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'ConvenienceCharge'), 'Handling charges', 
					(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @ConvenceCharge, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
				END

				IF(@TicketServiceTax <> 0)
				BEGIN
					INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
					VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'ServiceCharge'), 'Service charges', 
					(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @TicketServiceTax, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
				END

				IF(@AddtitionalConvenceCharge <> 0)
				BEGIN
					INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
					VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'OtherCharge'), 'Bank', 
					(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @AddtitionalConvenceCharge, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
				END

				IF(@TicketSMSCharge <> 0)
				BEGIN
					INSERT INTO TicketFeeDetails (EventTicketAttributeId, FeeId, DisplayName, ValueTypeId, Value, IsEnabled, CreatedUtc, CreatedBy)
					VALUES (@EventTicketAttributesId, (select top 1 Id from FeeTypes where FeeType = 'PrintAtHomeCharge'), 'Bank', 
					(select top 1 Id from ValueTypes where ValueTypes = 'Percentage'), @TicketSMSCharge, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
				END
			END
		END
	END
			
	FETCH NEXT FROM EventCursor   
	INTO @EventCategoryName, @VenueAddress, @Name, @StartDateTime, @EndDateTime, @SalesStartDateTime, @SalesEndDateTime, @VenueCatName, @AvailableTicketForSale, @RemainingTicketForSale, @TicketCategoryDescription, 
	@ViewFromStand, @IsSeatSelection, @Price, @LocalPrice, @IsInternationalCardAllowed, @IsEMIApplicable, @ConvenceCharge, @TicketServiceTax, @AddtitionalConvenceCharge, @TicketSMSCharge
END   
CLOSE EventCursor;  
DEALLOCATE EventCursor;  