USE [KzOLTP]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetTicketFeeDetails]    Script Date: 5/31/2018 4:21:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetTicketFeeDetails] --4200,1
(
	@EventTicketAttributeId BIGINT, 
	@FeeId INT
)
RETURNS VARCHAR(200)
AS
BEGIN
DECLARE @Charge VARCHAR(200)

SELECT @Charge = CONVERT(VARCHAR(200),Value) +''+ CASE WHEN ValueTypeId =1 THEN '%' ELSE ''  END FROM TicketFeeDetails
WHERE EventTicketAttributeId = @EventTicketAttributeId AND FeeId = @FeeId
RETURN (@Charge)

END
GO
/****** Object:  UserDefinedFunction [dbo].[ProperCase]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[ProperCase](@Text as varchar(8000))
returns varchar(8000)
as
begin
   declare @Reset bit;
   declare @Ret varchar(8000);
   declare @i int;
   declare @c char(1);

   select @Reset = 1, @i=1, @Ret = '';

   while (@i <= len(@Text))
    select @c= substring(@Text,@i,1),
               @Ret = @Ret + case when @Reset=1 then UPPER(@c) else LOWER(@c) end,
               @Reset = case when @c like '[a-zA-Z]' then 0 else 1 end,
               @i = @i +1
   return @Ret
end

GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  FUNCTION [dbo].[SplitString]       
(        
 @String VARCHAR (max),        
 @Delimiter CHAR (1)        
 )        
RETURNS @ValueTable TABLE (KeyWord VARCHAR(4000))        
BEGIN        
 SET @String=@String+@Delimiter+'Terminator'        
 DECLARE @Word VARCHAR(20)        
 WHILE CHARINDEX(@Delimiter,@String,0) <> 0        
 BEGIN        
  SELECT        
  @Word=RTRIM(LTRIM(SUBSTRING(@String,1,CHARINDEX(@Delimiter,@String,0)-1))),        
  @String=RTRIM(LTRIM(SUBSTRING(@String,CHARINDEX(@Delimiter,@String,0)+1,LEN(@String))))        
        
  IF LEN(@Word) > 0        
   insert into @ValueTable (Keyword) Values (@Word)        
 END        
RETURN        
END 
GO
/****** Object:  StoredProcedure [dbo].[Corp_GetAutoFillCorporateDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Corp_GetAutoFillCorporateDetails] 
(
	@CorpOrderId BIGINT
)
AS
BEGIN
	SELECT A.Id AS CorpOrderId,
	SponsorName AS CorpName,
	A.FirstName +' '+ A.LastName AS RepName,
	A.PhoneCode +'-'+ A.PhoneNumber AS MobNo,
	Email AS EmailId, 
	ISNULL(B.Address,'') AS Address,
	ISNULL(B.City,'') AS City, 
	ISNULL(B.State,'') AS State, 
	ISNULL(B.CountryId,0) AS CountryId,
	ISNULL(B.ZipCode,'') AS ZipCode,
	CASE WHEN PickupRepresentativeFirstName IS NOT NULL THEN PickupRepresentativeFirstName+' '+PickupRepresentativeLastName
	ELSE '' END AS PickUpRepName,
	ISNULL(PickupRepresentativeEmail,'') AS PickUpRepEmailId,
	CASE WHEN PickupRepresentativePhoneNumber IS NOT NULL THEN PickupRepresentativePhoneCode+'-'+PickupRepresentativePhoneNumber 
	ELSE '' END AS PickUpRepMobNo,
	ISNULL(A.Classification,'') AS Classification	
	FROM CorporateRequests A WITH(NOLOCK)
	INNER JOIN CorporateRequestCompanyAddressMappings B WITH(NOLOCK) ON A.Id = B.CorporateRequestId
	WHERE A.Id=@CorpOrderId
END
GO
/****** Object:  StoredProcedure [dbo].[Corp_GetCompanyRequesters]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Corp_GetCompanyRequesters] --'Kya'
(
	@CompanyName NVARCHAR(200)
)
AS
BEGIN
	SELECT SponsorName+' - '+FirstName +' '+ LastName AS CorporateName,Id
	FROM CorporateRequests
	WHERE SponsorName LIKE '%'+@CompanyName+'%'
	ORDER BY SponsorName ASC
END
GO
/****** Object:  StoredProcedure [dbo].[CorpOrder_CheckSeatAvailabilityBeforeBlock]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[CorpOrder_CheckSeatAvailabilityBeforeBlock]
(
--DECLARE 
@TicketNumberId VARCHAR(5000),
@retValue INT OUTPUT
)
AS
BEGIN
	IF EXISTS(SELECT A.Id FROM MatchSeatTicketDetails A
	INNER JOIN MatchLayoutSectionSeats B On A.MatchLayoutSectionSeatId = B.Id
	WHERE A.Id IN (SELECT * FROM dbo.SplitString(@TicketNumberId, ',')) AND B.SeatTypeId = 1
	AND SeatStatusId=1)
	BEGIN
		SET @retValue=1 --7304625,7304626
	END
	ELSE
	BEGIN
		SET @retValue=0
	END

	--SET @retValue=0
END
GO
/****** Object:  StoredProcedure [dbo].[CorpOrder_GetEventVenues]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CorpOrder_GetEventVenues]
(
	@EventCatId  BIGINT,
	@UserId INT=0
)
AS
BEGIN
	IF (@UserId=0)
	BEGIN
		SELECT DISTINCT VD.Id AS VenueId ,  ISNULL(VD.Name,'') + ', '+  ISNULL(CM.Name,'')  AS EventVenue
		FROM  EventDetails ED
		INNER JOIN Venues VD ON ED.VenueId=VD.Id
		INNER JOIN Cities CM ON VD.CityId=CM.Id
		WHERE ED.EventId = @EventCatId AND ED.IsEnabled =1
	END
	ELSE
		BEGIN
			SELECT DISTINCT VD.Id AS VenueId ,  ISNULL(VD.Name,'') + ', '+  ISNULL(CM.Name,'')  AS EventVenue
			FROM  EventDetails ED
			INNER JOIN Venues VD ON ED.VenueId=VD.Id
			INNER JOIN Cities CM ON VD.CityId=CM.Id
			INNER JOIN UserVenueMappings UV ON UV.VenueId=VD.Id
			WHERE ED.EventId = @EventCatId
	END
END
GO
/****** Object:  StoredProcedure [dbo].[ITKTS_ShowSeatsForReprintKITMSUser]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ITKTS_ShowSeatsForReprintKITMSUser] --210000371,'44896,44897,61556,61557,78216,78217'          
(          
 @Transid BIGINT,          
 @TicNumIds varchar(max)          
)           
AS           
BEGIN 

SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,
EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, TD.PricePerTicket AS LocalPricePerTic,
VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime, 
CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,
MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,
ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,
RowNumber AS Row_No, SeatTag AS Tic_No, TD.PricePerTicket AS PricePerTic, 0 AS IsChild, 0 AS IsSRCitizen, TC.Id AS VenueCatId,
0 AS IsWheelChair, 0 AS  IsFamilyPkg, ED.Id AS EventId, 0 AS IsLayoutAvail, EA.TicketHtml AS TicketHtml, 
ISNULL(EG.StreetInformation,'') AS RoadName,
ISNULL(MLS.StaircaseNumber,'') AS StaircaseNo
FROM Transactions T
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN EventTicketAttributes  ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id
INNER JOIN EventTicketDetails  ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
INNER JOIN TicketCategories  TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails  ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
INNER JOIN EventAttributes  EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId
INNER JOIN Events  E WITH(NOLOCK) ON ED.EventId = E.Id
INNER JOIN Venues  VD WITH(NOLOCK) ON ED.VenueId = VD.Id
INNER JOIN Cities  C WITH(NOLOCK) ON VD.CityId = C.Id
INNER JOIN CurrencyTypes  CT WITH(NOLOCK) ON T.CurrencyId = CT.Id
INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId
INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id
INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id
INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id
WHERE T.Id = @Transid AND T.TransactionStatusId =8 --AND PrintStatusId <>2         
AND MSTD.Id  IN (SELECT Keyword from dbo.SplitString(@TicNumIds,','))
ORDER BY ED.StartDateTime, ETA.Id
          
 SELECT  SM.SponsorName AS SponsorOrCustomerName, SM.SponsorName AS SponsorName FROM  CorporateTransactionDetails CTD  WITH(NOLOCK)                                                                                    
 INNER JOIN Transactions T WITH(NOLOCK) ON CTD.TransactionId = T.Id                                                                                       
 INNER JOIN  Sponsors SM WITH(NOLOCK) ON  SM.Id =  CTD.SponsorId
 WHERE  CTD.TransactionId=  @Transid 
          
END 

GO
/****** Object:  StoredProcedure [dbo].[KITMS__GetHandOverSheetSerialDetails_Temp]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS__GetHandOverSheetSerialDetails_Temp]    
(                          
	@TransId BIGINT                        
)
AS                                  
BEGIN                      
	SELECT DISTINCT A.TransactionId AS TransId, B.TotalTickets AS QuantityPrinted, SerialStart AS SerialFrom,
	SerialEnd AS SerialTo, TicketHandedBy AS HandedBy, TicketHandedTo AS SubmittedTo
	FROM HandoverSheets A
	INNER JOIN Transactions B On A.TransactionId =B.Id AND A.TransactionId = @TransId
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_AddCategorySponsor]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_AddCategorySponsor]  -- 44,58611,162395,10,''                   
(                 
	@SponsorId BIGINT,              
	@EventId BIGINT,               
	@CategoryId BIGINT,                    
	@BlockTicketQty INT, 
	@BlockBy VARCHAR(500)               
)                      
AS                    
BEGIN       
	DECLARE @Msg VARCHAR(100), @Price DECIMAL(18,2), @AltId VARCHAR(500)   
	SELECT @Price = LocalPrice FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=  @CategoryId
	SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@BlockBy

	IF NOT EXISTS(SELECT * FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId=@CategoryId
	AND IsEnabled = 1 AND SponsorId = @SponsorId)      
	BEGIN  
	INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price, 
	IsEnabled, CreatedUtc, CreatedBy)                      
	VALUES(NEWID(),@CategoryId, @SponsorId,@BlockTicketQty,@BlockTicketQty,@Price,1,GETDATE(),@AltId)
	SET @Msg = 'Inserted'      
	SELECT @Msg
END   
ELSE
BEGIN
	SET @Msg = 'Already Inserted'      
	SELECT @Msg      
END            
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_AddCategorySponsorByVenue2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_AddCategorySponsorByVenue2017]     
(                         
	@SponsorId BIGINT,   
	@VenueId BIGINT, 
	@EventCatId BIGINT,                
	@VenueCatId BIGINT,                       
	@BlockTic BIGINT,                            
	@BlockBy VARCHAR(500)                       
)                              
AS                            
BEGIN               
           
CREATE TABLE #Events(EventId BIGINT,VmccId BIGINT, Price DECIMAL(18,2))          
          
INSERT INTO #Events 
SELECT B.EventDetailId, A.Id, A.LocalPrice FROM EventTicketAttributes A
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId = B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT Id FROM EventDetails WHERE EventId=@EventCatId)
        
DECLARE @EventName VARCHAR(500),  @Count BIGINT, @Msg VARCHAR(500), @AltId VARCHAR(500)
SELECT  @AltId = AltId FROM USers WHERE UserName =@BlockBy
         
DECLARE @EventId BIGINT, @VmccId BIGINT, @Price DECIMAL(18,2)
DECLARE curEvent CURSOR FOR  SELECT * FROM #Events 
OPEN  curEvent;          
FETCH NEXT FROM curEvent INTO @EventId, @VmccId, @Price
WHILE @@FETCH_STATUS=0          
BEGIN      
IF((SELECT COUNT(*) FROM EventSponsorMappings WHERE SponsorId= @SponsorId AND EventDetailId= @EventId)=0)      
BEGIN  
	INSERT INTO EventSponsorMappings(SponsorId, EventDetailId, IsEnabled, CreatedBy, CreatedUtc, UpdatedBy, UpdatedUtc)                  
	VALUES(@SponsorId, @EventId, 1, @AltId, GETDATE(),NULL,NULL)  
END
            
SET @EventName = (SELECT Name FROM EventDetails WHERE Id =  @EventId)
IF((SELECT COUNT(*) FROM CorporateTicketAllocationDetails WHERE SponsorId= @SponsorId AND EventTicketAttributeId= @VmccId)=0)
BEGIN
	INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price, 
	IsEnabled, CreatedUtc, CreatedBy)                      
	VALUES(NEWID(),@VmccId, @SponsorId,@BlockTic,@BlockTic,@Price,1,GETDATE(),@AltId)
END
FETCH NEXT FROM curEvent INTO @EventId, @VmccId, @Price        
END
SET @Msg += 'Inserted for ' + @EventName + ','                    
CLOSE curEvent;        
DEALLOCATE curEvent;        
           
SELECT @Msg  

DROP TABLE #Events;
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_AddMatchSponsor]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[KITMS_AddMatchSponsor] --1195,8, 'sachin.jadhav@kyazoonga.com'
(          
	@EventId BIGINT,           
	@SponsorId BIGINT,          
	@UpdatedBy VARCHAR(500)           
)                  
AS                
BEGIN    
IF((SELECT COUNT(*) FROM EventSponsorMappings WHERE SponsorId= @SponsorId and EventDetailId = @EventId)=0)    
BEGIN
	DECLARE @AltId VARCHAR(500)
	SELECT  @AltId = AltId FROM USers WHERE UserName =@UpdatedBy
	INSERT INTO EventSponsorMappings(SponsorId, EventDetailId, IsEnabled, CreatedBy, CreatedUtc, UpdatedBy, UpdatedUtc)                  
	VALUES(@SponsorId, @EventId, 1, @AltId, GETDATE(),NULL,NULL)       
END       
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_AddNewCorporateBulkEntityByVenue]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROC [dbo].[KITMS_AddNewCorporateBulkEntityByVenue]
(                                
	@VenueId BIGINT,
	@EventCatId BIGINT,  
	@SponsorId BIGINT,          
	@UpdatedBy VARCHAR(500)                         
)                              
AS                            
BEGIN   
   DECLARE @Events TABLE   
   (        
	   EventId BIGINT        
   )        
           
   INSERT INTO @Events SELECT Id FROM EventDetails WHERE VenueId =  @VenueId  AND EventId= @EventCatId      
   DECLARE @EventId BIGINT        
   DECLARE @AltId VARCHAR(500)
   SELECT  @AltId = AltId FROM USers WHERE UserName =@UpdatedBy

   DECLARE curEventId CURSOR FOR                         
   SELECT DISTINCT EventId FROM @Events        
   OPEN  curEventId;        
          
   FETCH NEXT FROM  curEventId INTO @EventId        
   WHILE @@FETCH_STATUS=0        
   BEGIN      
		IF NOT EXISTS(SELECT Id FROM EventSponsorMappings WHERE SponsorId= @SponsorId AND EventDetailId= @EventId)      
		BEGIN  
			INSERT INTO EventSponsorMappings(SponsorId, EventDetailId, IsEnabled, CreatedBy, CreatedUtc, UpdatedBy, UpdatedUtc)                  
			VALUES(@SponsorId, @EventId, 1, @AltId, GETDATE(),NULL,NULL)  
		END  
		ELSE  
		BEGIN  
		PRINT 'Already inserted'  
		END  
     
  FETCH NEXT FROM  curEventId INTO @EventId        
  END        
                 
  CLOSE curEventId;  
  DEALLOCATE curEventId;
                            
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_AddVSponsorByVenue]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CRETAED BY SACHIN JADHAV 
CREATE  PROC [dbo].[KITMS_AddVSponsorByVenue]
(                                
	@VenueId BIGINT,   
	@SponsorId BIGINT,          
	@UpdatedBy VARCHAR(500)                         
)                              
AS                            
BEGIN   
   DECLARE @Events TABLE   
   (        
	   EventId BIGINT        
   )        
           
   INSERT INTO @Events SELECT EventId FROM EventDetails WHERE VenueId =  @VenueId        
   DECLARE @EventId BIGINT        
   DECLARE @AltId VARCHAR(500)
   SELECT  @AltId = AltId FROM USers WHERE UserName =@UpdatedBy

   DECLARE curEventId CURSOR FOR                         
   SELECT DISTINCT EventId FROM @Events        
   OPEN  curEventId;        
          
   FETCH NEXT FROM  curEventId INTO @EventId        
   WHILE @@FETCH_STATUS=0        
   BEGIN      
		IF((SELECT COUNT(*) FROM EventSponsorMappings WHERE SponsorId= @SponsorId AND EventDetailId= @EventId)=0)      
		BEGIN  
			INSERT INTO EventSponsorMappings(SponsorId, EventDetailId, IsEnabled, CreatedBy, CreatedUtc, UpdatedBy, UpdatedUtc)                  
			VALUES(@SponsorId, @EventId, 1, @AltId, GETDATE(),NULL,NULL)  
		END  
		ELSE  
		BEGIN  
		PRINT 'Already inserted'  
		END  
     
  FETCH NEXT FROM  curEventId INTO @EventId        
  END        
                 
  CLOSE curEventId;  
  DEALLOCATE curEventId;
                            
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_AssignTicketsToSponsor]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_AssignTicketsToSponsor] --1, 11909, 31364,5,'KyazoongaTom'  
(                            
  @KITMSMasterId BIGINT,                            
  @EventID BIGINT,                            
  @VMCCId BIGINT,                             
  @TicketQty INT,                            
  @UpdateBY VARCHAR(500)                             
)                            
AS                            
BEGIN      
    
BEGIN TRANSACTION trans    
BEGIN TRY   
  
DECLARE @TempTable TABLE(BlockTic INT, AvailableTic INT, Status INT)
DECLARE @RemainingTicketForSale BIGINT, @AltId VARCHAR(500), @Price DECIMAL(18,2), @SponsorId INT
SELECT @RemainingTicketForSale =RemainingTicketForSale, @Price = LocalPrice FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VMCCId
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY
    
IF EXISTS(SELECT * FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId AND EventTicketAttributeId = @VMCCId)  
BEGIN              
 IF(@RemainingTicketForSale-@TicketQty >= 0)                
 BEGIN
	UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets+@TicketQty, RemainingTickets = RemainingTickets+@TicketQty,
	UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id = @KITMSMasterId   
	UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @TicketQty WHERE Id=@VMCCId
	INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)
	VALUES(@KITMSMasterId,1,@TicketQty,@Price,1,GETDATE(),@AltId);
    INSERT INTO @TempTable VALUES(@TicketQty, @RemainingTicketForSale, 1)  
    SELECT * FROM @TempTable 
 END     
 ELSE                
 BEGIN                
    INSERT INTO @TempTable VALUES(@TicketQty, @RemainingTicketForSale, 0)  
    SELECT * FROM @TempTable    
 END        
END
COMMIT TRANSACTION trans    
END TRY    
BEGIN CATCH    
ROLLBACK TRANSACTION trans     
END CATCH                            
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_BlockkSeatsForSponsors]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_BlockkSeatsForSponsors]               
(             
	@Seats NVARCHAR(MAX),                      
	@EventId BIGINT,  
	@SponsorId INT  
)              
AS              
BEGIN                          
  
  DECLARE @SponsorType INT = 1
  SELECT @SponsorType = SponsorTypeId FROm Sponsors WHERE Id= @SponsorId
  IF(@SponsorType=0)
  BEGIN
	  UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1
	  UPDATE MatchLayoutSectionSeats SET SeatTypeId= 2 WHERE Id IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatTypeId = 1
  END
  ELSE
  BEGIN
	  UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1
	  UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatTypeId = 1
  END
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_BlockTicketsForSponsorByVenue_Temp]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_BlockTicketsForSponsorByVenue_Temp]
(                                        
	 @SponsorId BIGINT,
	 @EventIds VARCHAR(MAX),
	 @VenueCatId BIGINT,
	 @tics INT,
	 @UpdateBY VARCHAR(500)
)                                        
AS  
BEGIN
BEGIN TRANSACTION trans
BEGIN TRY
                                      
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @KM_ID BIGINT, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',
@Flag VARCHAR(50) = 'Default',@EventId BIGINT, @VmccId BIGINT,@AltId VARCHAR(500), @KITMSMasterId BIGINT, @Price DECIMAL(18,2)
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                
INSERT INTO #vmccIds
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))

DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds
OPEN  curEvent;                 
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
	WHILE @@FETCH_STATUS=0                
	BEGIN                
		SELECT @TicketForSale =RemainingTicketForSale, @Price= LocalPrice FROM EventTicketAttributes WHERE Id=@VmccId
		IF(@TicketForSale-@tics>=0)                
		BEGIN
			
			IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId
			AND SponsorId=@SponsorId AND IsEnabled = 1)
			BEGIN
				SELECT @KITMSMasterId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId
				AND SponsorId=@SponsorId AND IsEnabled = 1
				UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets+@tics, RemainingTickets = RemainingTickets+@tics,
				UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id = @KITMSMasterId 
			     --PRINT 'Updated CorporateTicketAllocationDetails ' +CONVERT(VARCHAR(200),@KITMSMasterId) +'-'+CONVERT(VARCHAR(200),@VmccId)+'-'+CONVERT(VARCHAR(200),@SponsorId)
			END
			ELSE
			BEGIN
				INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price, 
				IsEnabled, CreatedUtc, CreatedBy)                      
				VALUES(NEWID(),@VMCCId, @SponsorId,@tics,@tics,@Price,1,GETDATE(),@AltId)
				SET @KITMSMasterId = SCOPE_IDENTITY()
			END    
			UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @tics WHERE Id=@VMCCId
			INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)
			VALUES(@KITMSMasterId,1,@tics,@Price,1,GETDATE(),@AltId);             

			SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId
			SET @Flag='Changed'
			IF(@Flag='Changed')                            
			BEGIN                       
				SET @ReturnVal += CONVERT(VARCHAR(100),@tics) + ' Tickets Blocked Successfully for ' + @EventName+ ',<br/> '   
			END  
		END
		ELSE                            
		BEGIN                  
			SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId 
			SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@TicketForSale) + ' Tickets are Availble for '+ @EventName + ',<br/> '
	   END                            

FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
END                 
                      
CLOSE curEvent;                 
DEALLOCATE curEvent;                

DROP TABLE #vmccIds          
SELECT @ReturnVal                     
COMMIT TRANSACTION trans  
END TRY  
BEGIN CATCH  
ROLLBACK TRANSACTION trans   
END CATCH                                
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_BookCartByVenue_Temp]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_BookCartByVenue_Temp]    
(                                                                                                  
  @VmccId BIGINT=312579,                                                                                                  
  @NoOfTic INT,                                                                                                                                                                                              
  @TransId BIGINT = -1,                                                                                               
  @Othercharge DECIMAL(18,2) = 0,                                                                                                  
  @DiscountAmt DECIMAL(18,2)= 0,                                                                                                  
  @TotalCharge DECIMAL(18,2)= 0,                                                                                                  
  @TotalTicAmt DECIMAL(18,2)=0,                                                                                    
  @ConvenienceCharge DECIMAL(18,2)=0,                                                                                    
  @ServiceTax DECIMAL(18,2)=0,                                                                                                  
  @Tickettype INT =1,                                                                                        
  @DileveryType INT,                                                                                
  @UserId BIGINT = null,                                         
  @VenuePickupId INT=0,                                                                                 
  @PricePerTic DECIMAL(18,2)=0,                                         
  @CardNumber VARCHAR(100)=null ,                                                                                                  
  @NameOnCard VARCHAR(100)=null,                                                                                                  
  @CardType INT=null,                                                                                                  
  @CardExpire VARCHAR(10)=null,                                                                                                  
  @ClientIP VARCHAR(50)=null,                                                                                                  
  @PayType VARCHAR(500) = null,                                                                                                  
  @BookStatus INT = 1,                                                                                                  
  @BoughtStatus INT = 0,                                                                                                  
  @PayConfNumber VARCHAR(1000)= null,                                                                                                  
  @ReceiptNo VARCHAR(1000) = null,                                                                                                  
  @ErrorMessage VARCHAR(500) = null,                                      
  @SmsSend INT = 0,                                                                                                  
  @EmailSend INT = 0,                                                    
  @CurrencyType VARCHAR(20),                    
 --Parameter use in case of seat selection              
  @TicNumIds VARCHAR(MAX)=null,                                       
  @SeatNumbers VARCHAR(MAX)=null,                            
  @KM_Id BIGINT=0,                  
  @TypeOfTransaction varchar(55),                  
  @Quantity BIGINT,            
  @SubSponsorId BIGINT=null,
  @TotlatTickets INT,
  @ConvChargePerItem DECIMAL(18,2) = 0,
  @DiscountAmtPerItem DECIMAL(18,2) = 0,
  @ServiceChargePerItem DECIMAL(18,2) = 0
 --end trans input                   
)         
AS      
BEGIN
BEGIN TRANSACTION                                                           
DECLARE @AltId NVARCHAR(500),@SponsorId BIGINT, @CurrencyId INT, @TicketCount INT
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE Id =@UserId
SELECT @CurrencyId = Id From CurrencyTypes WHERE Code=@CurrencyType       
 -- START TRANS INSERT
 SELECT @SponsorId = SponsorId FROM CorporateTicketAllocationDetails WHERE Id=@KM_Id
SET @TicketCount  = (SELECT ISNULL(RemainingTickets,0) FROM CorporateTicketAllocationDetails WHERE Id = @KM_Id)
IF (@TicketCount >= @NoOfTic)                                                                                  
BEGIN   
 IF @TransId = -1                                                                                                       
 BEGIN
	 INSERT INTO Transactions(ChannelId, CurrencyId, TotalTickets, GrossTicketAmount, DeliveryCharges, ConvenienceCharges, ServiceCharge,
	 DiscountAmount, NetTicketAmount, TransactionStatusId, CreatedUtc, CreatedBy)
	 VALUES(8,@CurrencyId,@TotlatTickets,@TotalCharge,0,@ConvenienceCharge,@ServiceTax,@DiscountAmt,@TotalTicAmt,0,GETDATE(),@AltId)
    SET @TransId = SCOPE_IDENTITY() 
	
	INSERT INTO TransactionPaymentDetails(TransactionId,PaymentOptionId,PaymentGatewayId,UserCardDetailId,RequestType,Amount,PayConfNumber,PaymentDetail,
	 CreatedUtc,CreatedBy)
	 VALUES(@TransId,NULL,NULL,NULL, @TypeOfTransaction, @TotalTicAmt, @TypeOfTransaction, @PayConfNumber,GETDATE(),@AltId)
	                                  
 END                          
 ELSE                                                          
 BEGIN                                                          
   IF @TotalTicAmt <> 0 AND @TransId <>-1
   BEGIN
		UPDATE Transactions SET
		TotalTickets = @TotlatTickets, GrossTicketAmount = @TotalCharge, DeliveryCharges = 0, ConvenienceCharges = @ConvenienceCharge, ServiceCharge=@ServiceTax,
		DiscountAmount=@DiscountAmt, NetTicketAmount=@TotalTicAmt,  UpdatedBy = @AltId, UpdatedUtc = GETDATE()
		WHERE Id= @TransId   
   END                                                       
END
DECLARE @ActionTypeId INT
SET @ActionTypeId  = CASE WHEN @TypeOfTransaction='Paid' THEN 1 ELSE 2 END
UPDATE CorporateTicketAllocationDetails SET RemainingTickets = RemainingTickets - @NoOfTic, UpdatedUtc= GETDATE(), UpdatedBy = @AltId WHERE Id = @KM_Id
INSERT INTO CorporateTransactionDetails(AltId,SponsorId,TransactionId,EventTicketAttributeId,TotalTickets,Price,TransactingOptionId,IsEnabled,CreatedUtc,CreatedBy)
VALUES(NewID(),@SponsorId,@TransId,@VmccId,@NoOfTic,@PricePerTic,@ActionTypeId,0,GETDATE(),@AltId)
                 
 -- END TRANS INSERT                         
-- INSERT INTO VMCC_MapTrans    TABLE    
 DECLARE @countVMCC INT                                                    
 SET @countVMCC = (SELECT COUNT(*) FROM TransactionDetails WHERE  TransactionId = @TransId                                                    
 AND  EventTicketAttributeId = @VmccId)                                                    
 IF  @countVMCC = 0                                     
 BEGIN
	INSERT INTO TransactionDetails(TransactionId,EventTicketAttributeId,TotalTickets,PricePerTicket,DeliveryCharges,ConvenienceCharges,ServiceCharge,
	DiscountAmount,TicketTypeId,CreatedUtc,CreatedBy)
	VALUES(@TransId,@VmccId,@NoOfTic,@PricePerTic,0,@ConvChargePerItem,@ServiceChargePerItem,@DiscountAmtPerItem,1,GETDATE(),@AltId)
  -- UPDATE SEAT NUMBER FOR BOOKED TKTS                                                    
  IF @SeatNumbers <>''                                        
  BEGIN
	UPDATE MatchSeatTicketDetails SET TransactionId =@TransId, SeatStatusId =2, SponsorId = @SponsorId,UpdatedUtc= GETDATE(), UpdatedBy = @AltId  WHERE 
	MatchLayoutSectionSeatId IN (SELECT CONVERT(BIGINT,Keyword) AS SeatId FROM dbo.SplitString(@TicNumIds,','))
	UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN (SELECT CONVERT(BIGINT,keyword) AS SeatId FROM dbo.SplitString(@TicNumIds,',')) 
  END                                                                                              
 END
 -- END VMCC_MapTrans                                                                                                       
END
ELSE                                                                           
BEGIN                                                          
 SET @TransId =0  -- IF NO TIC FOUND                                                                              
END                       
                                                               
   IF @@ERROR <> 0                                                                                                      
   BEGIN
   SELECT -1 AS Result                                
   ROLLBACK TRANSACTION                                                                                     
   END                                                                      
ELSE                                                                                                      
   BEGIN                                                                                                      
   SELECT @TransId AS TransId
   COMMIT TRANSACTION                                                                               
 END                                                                                 
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_BookSponsorTickets_New]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_BookSponsorTickets_New]      
(                                                                                                    
	@VMCC_Id1 BIGINT,
	@NoOfTic1 INT,
	@TransId bigint = -1,
	@Othercharge decimal(18,2) = 0,
	@DiscountAmt decimal(18,2)= 0,
	@TotalCharge decimal(18,2)= 0,
	@TotalTicAmt decimal(18,2)=0,
	@ConvenienceCharge decimal(18,2)=0,
	@ServiceTax decimal(18,2)=0,
	@Tickettype INT =1,
	@DileveryType INT,
	@UserId bigint = null,
	@VenuePickupId int,
	@PricePerTic decimal(18,2)=0,
	@CardNumber Varchar(100)=null,
	@NameOnCard Varchar(100)=null,
	@CardType int=null,
	@CardExpire Varchar(10)=null,
	@ClientIP Varchar(50)=null,
	@PayType varchar(500) = null,
	@BookStatus int = 1,
	@BoughtStatus int = 0,
	@PayConfNumber Varchar(1000)= null,
	@ReceiptNo Varchar(1000) = null,
	@ErrorMessage Varchar(500) = null,
	@SmsSend int = 0,
	@EmailSend int = 0,
	@CurrencyType varchar(20),
	--Parameter use in case of seat selection
	@TicNumIds Varchar(200)=null,
	@SeatNumbers Varchar(500)=null,
	@KM_Id bigint,
	@TypeOfTransaction varchar(55),
	@Quantity bigint=null,
	@SubSponsorId bigint=null,
	@PaymentMethod NVARCHAR(200)
	--end trans input
)                                                             
AS
 BEGIN
 BEGIN TRANSACTION 
 DECLARE @AltId NVARCHAR(500),@SponsorId BIGINT, @CurrencyId INT
 SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE Id =@UserId
 SELECT @CurrencyId = Id From CurrencyTypes WHERE Code=@CurrencyType

 DECLARE @totalTransactedTickets INT, @AllocatedTickets INT, @RemainingTickets INT
 SELECT @SponsorID=SponsorId,@AllocatedTickets = AllocatedTickets,@RemainingTickets = RemainingTickets FROM  
CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KM_Id

SET @totalTransactedTickets = (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) where STA.IsEnabled = 1                                     
AND STA.EventTicketAttributeId = @VMCC_Id1  AND STA.TransactingOptionId IN(4,5) AND STA.TransactionId IN 
(SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
INNER JOIN Transactions T WITH(NOLOCK) On TD.EventTicketAttributeId = @VMCC_Id1 AND T.TransactionStatusId<>8)),0))

IF(@totalTransactedTickets+@NoOfTic1<=@AllocatedTickets)
BEGIN
 IF @TransId = -1                                                                                                     
 BEGIN
	 
	 SELECT @SponsorId = SponsorId FROM CorporateTicketAllocationDetails WHERE Id= @KM_Id  
	 INSERT INTO Transactions(ChannelId, CurrencyId, TotalTickets, GrossTicketAmount, DeliveryCharges, ConvenienceCharges, ServiceCharge,
	 DiscountAmount, NetTicketAmount, TransactionStatusId, CreatedUtc, CreatedBy)
	 VALUES(8,@CurrencyId,@NoOfTic1,@TotalCharge,0,@ConvenienceCharge,@ServiceTax,@DiscountAmt,@TotalTicAmt,0,GETDATE(),@AltId)
	 SET @TransId = SCOPE_IDENTITY()

	 DECLARE @ActionTypeId INT
	 SET @ActionTypeId  = CASE WHEN @TypeOfTransaction='Paid' THEN 1 ELSE 2 END
	 INSERT INTO CorporateTransactionDetails(AltId,SponsorId,TransactionId,EventTicketAttributeId,TotalTickets,Price,TransactingOptionId,IsEnabled,CreatedUtc,CreatedBy)
	 VALUES(NewID(),@SponsorId,@TransId,@VMCC_Id1,@NoOfTic1,@PricePerTic,@ActionTypeId,0,GETDATE(),@AltId)

	 INSERT INTO TransactionDetails(TransactionId,EventTicketAttributeId,TotalTickets,PricePerTicket,DeliveryCharges,ConvenienceCharges,ServiceCharge,
	 DiscountAmount,TicketTypeId,CreatedUtc,CreatedBy)
	 VALUES(@TransId,@VMCC_Id1,@NoOfTic1,@PricePerTic,0,@ConvenienceCharge,@ServiceTax,@DiscountAmt,1,GETDATE(),@AltId)

	 UPDATE CorporateTicketAllocationDetails SET RemainingTickets = RemainingTickets - @NoOfTic1, UpdatedUtc= GETDATE(), UpdatedBy = @AltId WHERE Id = @KM_Id
	 INSERT INTO TransactionPaymentDetails(TransactionId,PaymentOptionId,PaymentGatewayId,UserCardDetailId,RequestType,Amount,PayConfNumber,PaymentDetail,
	 CreatedUtc,CreatedBy)
	 VALUES(@TransId,NULL,NULL,NULL, @TypeOfTransaction, @TotalTicAmt, @PaymentMethod, @PayConfNumber,GETDATE(),@AltId)

	 IF @TicNumIds IS NOT NULL                                                        
	 BEGIN
	   UPDATE MatchSeatTicketDetails SET TransactionId =@TransId, SeatStatusId =2, SponsorId = @SponsorId,UpdatedUtc= GETDATE(), UpdatedBy = @AltId  WHERE 
	   MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@TicNumIds,','))
	   UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN (SELECT * FROM dbo.SplitString(@TicNumIds,','))
	 END
 END
 END
IF @@ERROR <> 0                                                                                                    
BEGIN                                                                                                    
  SELECT -1 AS Result                                                                                       
  ROLLBACK TRANSACTION                                                                                                    
   END                                                                    
ELSE                                                                                                    
   BEGIN                                                                                                    
  SELECT @TransId As TransId
  COMMIT TRANSACTION                                                                                                    
END                                                                               
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CancelTransactTickets]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_CancelTransactTickets]
(
	@TransId BIGINT,
	@UpdateBY VARCHAR(200)
)
AS
BEGIN                             
BEGIN TRANSACTION trans

DECLARE @retValue INT,@AltId NVARCHAR(500),@SponsorId BIGINT, @EventTicketAttributeId BIGINT, @Quantity INT, @Count INT = 1, @Counter INT
SELECT  @AltId = CreatedBy FROM Transactions WITH(NOLOCK) WHERE Id =@TransId
SELECT  @SponsorId = SponsorId FROM CorporateTransactionDetails WITH(NOLOCK) WHERE Id =@TransId
DECLARE @tbltransactionDetails TABLE(RowId INT IDENTITY(1,1), EventTicketAttributeId BIGINT, Quantity INT)
INSERT INTO @tbltransactionDetails
SELECT EventTicketAttributeId, TotalTickets FROm TransactionDetails WHERE TransactionId = @TransId

SELECT @Counter = COUNT(*) FROM @tbltransactionDetails
WHILE(@Count<=@Counter)
BEGIN
	SELECT @EventTicketAttributeId = EventTicketAttributeId, @Quantity =  Quantity FROM @tbltransactionDetails WHERE RowId = @Count
	UPDATE CorporateTicketAllocationDetails SET RemainingTickets = RemainingTickets + @Quantity, UpdatedUtc= GETDATE(), 
	UpdatedBy = @AltId WHERE EventTicketAttributeId = @EventTicketAttributeId
	UPDATE CorporateTransactionDetails SET TotalTickets = TotalTickets - @Quantity, UpdatedUtc= GETDATE(), 
	UpdatedBy = @AltId WHERE TransactionId= @TransId AND EventTicketAttributeId = @EventTicketAttributeId
	SET @retValue = 1
	SET @Count = @Count + 1
END
IF(@@ERROR <> 0)        
BEGIN        
 SET @retValue = 0        
 ROLLBACK TRANSACTION        
END        
COMMIT TRANSACTION 
SELECT @retValue        
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CheckBlockTicketsAvailability]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_CheckBlockTicketsAvailability]                                 
(                                                             
	 @EventIds VARCHAR(MAX),                                        
	 @VenueCatId BIGINT,                                         
	 @TicketQty INT                                         
)     

AS                                        
BEGIN  
           
DECLARE @vmccIds TABLE(VmccId BIGINT, EventId BIGINT)  
  
INSERT INTO @vmccIds
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,',')) 
DECLARE @Status TABLE(VmccId BIGINT, EventId BIGINT, EventName VARCHAR(500), TicketAvaliable BIGINT, Status BIGINT) 

DECLARE @EventId BIGINT, @VmccId BIGINT, @EventName VARCHAR(500), @TicketForSale INT
DECLARE curEvent CURSOR FOR SELECT * FROM @vmccIds
OPEN  curEvent;                 
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
	WHILE @@FETCH_STATUS=0                
	BEGIN
		SELECT @TicketForSale =RemainingTicketForSale FROM EventTicketAttributes WHERE Id=@VmccId
		SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId

		IF(@TicketForSale-@TicketQty>=0)                
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@TicketForSale, 1)
		END
		ELSE
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@TicketForSale, 0)
		END    

FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
END                 
                      
CLOSE curEvent;                 
DEALLOCATE curEvent;  
        
SELECT * FROM  @Status              
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CheckExistingSponsor]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_CheckExistingSponsor]
(
	@SponsorName VARCHAR(500)
)
AS
BEGIN
	SELECT * FROM Sponsors WHERE SponsorName=@SponsorName
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CheckPostalCode]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_CheckPostalCode]
(
	@PostalCode VARCHAR(100)
)
AS
BEGIN
	SELECT * FROM Zipcodes WHERE Postalcode = @PostalCode
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CheckReleaseTicketsAvailability]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_CheckReleaseTicketsAvailability]                                 
(    
	 @SponsorId BIGINT,                                                
	 @EventIds VARCHAR(MAX),                                        
	 @VenueCatId BIGINT,                                         
	 @TicketQty INT                                         
)     

AS                                        
BEGIN  
           
DECLARE @vmccIds TABLE(VmccId BIGINT,EventId BIGINT)
INSERT INTO @vmccIds
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))       
DECLARE @Status TABLE(VmccId BIGINT, EventId BIGINT, EventName VARCHAR(500), TicketAvaliable BIGINT, Status BIGINT) 
DECLARE @EventId BIGINT, @VmccId BIGINT, @EventName VARCHAR(500), @AvailableTic INT

DECLARE curEvent CURSOR FOR SELECT * FROM @vmccIds            
OPEN  curEvent;                 
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
	WHILE @@FETCH_STATUS=0                
	BEGIN      
	    
		SELECT @AvailableTic =RemainingTickets FROM CorporateTicketAllocationDetails WHERE EventTicketAttributeId=@VmccId AND SponsorId = @SponsorId AND IsEnabled=1
		SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId

		IF(@TicketQty<=@AvailableTic)                
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@AvailableTic, 1)
		END
		ELSE
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@AvailableTic, 0)
		END    

FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
END                 
                      
CLOSE curEvent;                 
DEALLOCATE curEvent;  
        
SELECT * FROM  @Status              
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CheckReprintTransId]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_CheckReprintTransId]    
(    
  @TransId BIGINT    
)    
AS    
BEGIN   
IF EXISTS(SELECT Id FROm Transactions WHERE Id = @TransId AND TransactionStatusId=8) 
BEGIN
  SELECT ID AS EventTransId FROm Transactions WHERE Id = @TransId AND TransactionStatusId=8
END   
ELSE
BEGIN
	SELECT 0 AS EventTransId
END 
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CheckSeatAvailbility]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_CheckSeatAvailbility]
(             
	@Seats VARCHAR(MAX),                      
	@EventId BIGINT, 
	@retValue INT OUTPUT
)              
AS              
BEGIN                          
IF NOT EXISTS(SELECT COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK)
INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
WHERE A.SponsorId = NULL AND B.Id IN(SELECT * FROM dbo.SplitString(@Seats,',')) AND B.SeatTypeId =1
AND A.SeatStatusId=1)
  BEGIN
    SET @retValue = 1
  END         
  ELSE
  BEGIN
	SET @RetValue =0
  END
 SELECT @RetValue   
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_CreateSeats]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_CreateSeats] --210000375
(                                                                                                                   
  @EventTransId BIGINT                                                                                                     
)              
AS               
BEGIN                                                                                                            
                  
DECLARE @tblEventIds TABLE (Sno INT IDENTITY(1,1), EventId INT)
INSERT INTO @tblEventIds                            
SELECT EventDetailId FROM TransactionDetails A WITH(NOLOCK)
INNER JOIN EventTicketAttributes B WITH(NOLOCK) ON A.EventTicketAttributeId = B.Id
INNER JOIN EventTicketDetails C WITH(NOLOCK) ON B.EventTicketDetailId = C.Id
WHERE A.Id=@EventTransId

DECLARE @VMCC_Id BIGINT, @NoofTic INT, @PrintCount INT, @Barcode varchar(50), @CounterTic INT, @Counter INT = 1,
@EventTicketDetailId BIGINT, @SponsorId BIGINT
DECLARE @tblTics TABLE (Sno INT IDENTITY(1,1), VMCC_Id BIGINT, EventTicketDetailId BIGINT, NoofTic INT, VenueCatName varchar(100), EventStartDate DATETIME)                                
                        
INSERT INTO @tblTics               
SELECT  ETA.Id, ETD.Id,SUM(TD.TotalTickets), TC.Name, ED.StartDateTime
FROM Transactions T                    
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId               
INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id               
INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id              
WHERE T.TransactionStatusId = 8 AND T.Id=@EventTransID              
GROUP BY ETA.Id,ETD.Id, TD.TotalTickets, TC.Name, ED.StartDateTime                 
              
SELECT @CounterTic = COUNT(*) FROM @tblTics
WHILE (@Counter <= @CounterTic)               
BEGIN                
          
 SELECT @VMCC_Id = VMCC_Id, @NoofTic = NoofTic, @EventTicketDetailId = EventTicketDetailId FROM @tblTics WHERE Sno=@counter
 IF NOT EXISTS(SELECT Id FROM MatchSeatTicketDetails WHERE TransactionId=@EventTransID AND EventTicketDetailId=@EventTicketDetailId
 AND IsEnabled=1 AND SeatStatusId=2)               
 BEGIN                           
   SELECT @SponsorId= SponsorId FROm CorporateTransactionDetails WHERE TransactionId= @EventTransId
   DECLARE @BlockedSeatCount INT = 0, @UnBlockedSeatCount INT = 0
   
    SELECT @BlockedSeatCount = COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK)
	INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
	WHERE A.SponsorId = @SponsorId AND B.SeatTypeId =3 AND EventTicketDetailId=@EventTicketDetailId
    AND A.IsEnabled=1 AND A.SeatStatusId=1
              
   IF(@BlockedSeatCount >= @NoOfTic)              
   BEGIN              
		UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId              
		WHERE Id IN (SELECT TOP (@NoOfTic) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)
		INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
		WHERE A.SponsorId = @SponsorId AND EventTicketDetailId=@EventTicketDetailId
		AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (3) ORDER BY B.RowOrder,B.ColumnOrder)
   END              
   ELSE              
   BEGIN               
    IF(@BlockedSeatCount > 0)              
    BEGIN              
        UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId              
		WHERE Id IN (SELECT TOP (@BlockedSeatCount) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)
		INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
		WHERE A.SponsorId = @SponsorId AND EventTicketDetailId=@EventTicketDetailId
		AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (3) ORDER BY B.RowOrder,B.ColumnOrder)           
        
        SET @UnBlockedSeatCount = @NoOfTic - @BlockedSeatCount              
        UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId              
		WHERE Id IN (SELECT TOP (@UnBlockedSeatCount) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)
		INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
		WHERE EventTicketDetailId=@EventTicketDetailId
		AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1) ORDER BY B.RowOrder,B.ColumnOrder)
    END              
    ELSE              
    BEGIN              
        UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId              
		WHERE Id IN (SELECT TOP (@NoOfTic) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)
		INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
		WHERE  EventTicketDetailId=@EventTicketDetailId
		AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1) ORDER BY B.RowOrder,B.ColumnOrder)             
    END              
   END           
 END              
               
SELECT @PrintCount =SUM(ISNULL(PrintStatusId, 1))     
 FROM MatchSeatTicketDetails                    
 WHERE TransactionId=@EventTransID AND EventTicketDetailId=@EventTicketDetailId AND IsEnabled=1               
 
 IF(@PrintCount=@NoOfTic)
 BEGIN              
   DECLARE @ISSUERCODE VARCHAR(02) = '08', @IDFORLOCATION VARCHAR(02) = '07', @BARCODEEVENTID VARCHAR(04), @GATEID VARCHAR(02)=NULL,                                         
   @TicketType VARCHAR(2) = '01', @DiscountType VARCHAR(2) = '02' , @ChkPricePerTic DECIMAL(18,2) , @EventType INT, @EventID_1 BIGINT                                       
                            
  SELECT @BarCodeEventId = CASE LEN(DATEPART(DD,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(DD,StartDateTime))               
  ELSE CONVERT(VARCHAR,DATEPART(DD,StartDateTime)) END +               
  CASE LEN(DATEPART(MM,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MM,StartDateTime))               
  ELSE CONVERT(VARCHAR,DATEPART(MM,StartDateTime)) END,              
  @EventType = EventId, @EventID_1 = A.EventId                              
  FROM EventDetails A
  INNER JOIN EventTicketDetails B ON A.Id = B.EventDetailId
  INNER JOIN EventTicketAttributes C ON B.Id = C.EventTicketDetailId
  WHERE C.Id = @VMCC_Id
                
  DECLARE @tblBarcode TABLE (SrNo INT IDENTITY(1,1), SeatId BIGINT)               
  INSERT INTO @tblBarcode              
  SELECT MatchLayoutSectionSeatId FROM MatchSeatTicketDetails                    
  WHERE EventTicketDetailId= @EventTicketDetailId AND TransactionId = @EventTransID AND SeatStatusId=2
                
  DECLARE @CounterBarcode INT, @Counter_Barcode INT = 1, @SeatId BIGINT              
  SELECT @CounterBarcode = COUNT(*) FROM @tblBarcode              
  SET @Counter_Barcode = 1              
                
  WHILE (@Counter_Barcode <= @CounterBarcode)
  BEGIN                
    SELECT @SeatId = SeatId FROM @tblBarcode WHERE SrNo = @Counter_Barcode
    SET @Barcode = ISNULL(@BarcodeEventId,'01') +  STUFF(convert(varchar(500),(@SeatId)), 1, 0, REPLICATE('0', 6 - LEN(Convert(varchar(500),(@SeatId)))))
              
   DECLARE @chkBarcode INT              
   SELECT @chkBarcode = COUNT(*) FROM MatchSeatTicketDetails WHERE MatchLayoutSectionSeatId=@SeatId 
   AND EventTicketDetailId= @EventTicketDetailId  AND TransactionId=@EventTransID AND SeatStatusId=2 AND BarcodeNumber IS NULL                                
   IF(@chkBarcode > 0)                                
   BEGIN              
    UPDATE MatchSeatTicketDetails                      
    SET BarcodeNumber = CONVERT(VARCHAR(30),@Barcode)                                                                                                                   
    WHERE MatchLayoutSectionSeatId=@SeatId 
    AND EventTicketDetailId= @EventTicketDetailId  AND TransactionId=@EventTransID AND SeatStatusId=2 AND BarcodeNumber IS NULL             
   END              
              
   SET @Counter_Barcode = @Counter_Barcode + 1              
  END              
 END               
               
 SET @Counter = @Counter + 1              
END
SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,
EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, TD.PricePerTicket AS LocalPricePerTic,
VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime, 
CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,
MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,
ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,
RowNumber AS Row_No, SeatTag AS Tic_No, TD.PricePerTicket AS PricePerTic, 0 AS IsChild, 0 AS IsSRCitizen, TC.Id AS VenueCatId,
0 AS IsWheelChair, 0 AS  IsFamilyPkg, ED.Id AS EventId, 0 AS IsLayoutAvail, EA.TicketHtml AS TicketHtml, 
ISNULL(EG.StreetInformation,'') AS RoadName,
ISNULL(MLS.StaircaseNumber,'') AS StaircaseNo
FROM Transactions T
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN EventTicketAttributes  ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id
INNER JOIN EventTicketDetails  ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
INNER JOIN TicketCategories  TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails  ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
INNER JOIN EventAttributes  EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId
INNER JOIN Events  E WITH(NOLOCK) ON ED.EventId = E.Id
INNER JOIN Venues  VD WITH(NOLOCK) ON ED.VenueId = VD.Id
INNER JOIN Cities  C WITH(NOLOCK) ON VD.CityId = C.Id
INNER JOIN CurrencyTypes  CT WITH(NOLOCK) ON T.CurrencyId = CT.Id
INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId
INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id
INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id
INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id
WHERE T.Id = @EventTransID AND T.TransactionStatusId =8 AND PrintStatusId <>2
ORDER BY ED.StartDateTime, ETA.Id
                                        
 SELECT  SM.SponsorName AS SponsorOrCustomerName, SM.SponsorName AS SponsorName FROM  CorporateTransactionDetails CTD  WITH(NOLOCK)                                                                                    
 INNER JOIN Transactions T WITH(NOLOCK) ON CTD.TransactionId = T.Id                                                                                       
 INNER JOIN  Sponsors SM WITH(NOLOCK) ON  SM.Id =  CTD.SponsorId
 WHERE  CTD.TransactionId=  @EventTransID                                             
          
                                                                   
UPDATE MatchSeatTicketDetails                                                           
SET PrintStatusId=2, PrintCount = PrintCount + 1, PrintDateTime = GETDATE()
WHERE TransactionId=@EventTransID AND BarcodeNumber IS NOT NULL

END 



GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetAvailableCategorySponsors_New]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetAvailableCategorySponsors_New]
(
	@EventId BIGINT,
	@CategoryId BIGINT
)
AS
BEGIN

SELECT MS.Id AS SponsorId, MS.SponsorName                                                    
FROM Sponsors MS                                                    
INNER JOIN EventSponsorMappings S                                                    
ON MS.Id = S.SponsorId                                                    
WHERE S.EventDetailId = @EventId and MS.IsEnabled=1                                                    
AND  MS.Id NOT IN                                                     
(                                                    
      SELECT ST.SponsorId FROM CorporateTicketAllocationDetails ST WHERE EventTicketAttributeId = @CategoryId and ST.IsEnabled = 1                                                
   )     
ORDER BY MS.SponsorName 

END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetBarcodesByTransId]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetBarcodesByTransId]    
(    
  @TransId BIGINT    
)    
AS    
BEGIN    
	SELECT MSTD.Id AS TicNumId,T.Id AS TransId, BarcodeNumber AS barcode, 
	ED.name AS EventName, TC.Name AS VenueCatName, CONVERT(VARCHAR(20), StartDateTime, 107)  AS EventStartDate, 
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime,
    VD.Name AS VenueAddress, C.NAme As City_Name, MLSS.SeatTag, CONVERT(VARCHAR(20), EntryDateTime, 100) AS EntryDate
	FROM Transactions T
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
	INNER JOIN EventTicketAttributes  ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id
	INNER JOIN EventTicketDetails  ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	INNER JOIN TicketCategories  TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails  ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
	INNER JOIN EventAttributes  EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId
	INNER JOIN Events  E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Venues  VD WITH(NOLOCK) ON ED.VenueId = VD.Id
	INNER JOIN Cities  C WITH(NOLOCK) ON VD.CityId = C.Id
	INNER JOIN CurrencyTypes  CT WITH(NOLOCK) ON T.CurrencyId = CT.Id
	INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId
	INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id
	INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id
	INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id
	WHERE T.Id = @TransId AND T.TransactionStatusId =8 --AND PrintStatusId <>2
	ORDER BY ED.StartDateTime, ETA.Id
	  
END

GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetCategorySponsorDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetCategorySponsorDetails]                                                       
(                                                          
	 @EventId BIGINT,                                                          
	 @VmccId BIGINT                                                          
)                                                          
AS                                                          
BEGIN  

DECLARE @TempTable TABLE         
(KM_ID BIGINT, SponsorName VARCHAR(1000), AvailableTic INT, SponsorId BIGINT,        
 BlockTic INT, PaidQty INT, ComplimentaryQty INT, UsedPaidQty INT,         
 UsedComplimentaryQty INT, PaidBlockedQty INT, CompBlockedQty INT      
)
INSERT INTO  @TempTable
SELECT KM.Id AS KM_ID,(SELECT SponsorName FROM Sponsors WITH(NOLOCK) WHERE Id = KM.SponsorId) AS SponsorName,                                        
KM.RemainingTickets AS AvailableTic,KM.SponsorId, KM.AllocatedTickets AS BlockTic,
(SELECT ISNULL(SUM(STN.TotalTickets),0)FROM CorporateTransactionDetails STN WITH(NOLOCK) WHERE STN.EventTicketAttributeId = @VmccId AND STN.SponsorId = KM.SponsorId 
AND TransactingOptionId=4) AS PaidQty,                                        
(SELECT ISNULL(SUM(STN.TotalTickets),0)FROM CorporateTransactionDetails STN WITH(NOLOCK) WHERE STN.EventTicketAttributeId = @VmccId AND STN.SponsorId = KM.SponsorId AND
STN.TransactingOptionId=5 )AS ComplimentaryQty,
(ISNULL((SELECT SUM(STA.TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) where STA.IsEnabled = 1 AND STA.SponsorId = KM.SponsorId                      
AND STA.EventTicketAttributeId = @VmccId  AND STA.TransactingOptionId IN(4) AND STA.TransactionId IN 
(SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = @VmccId AND T.TransactionStatusId=8)),0)) AS UsedPaidQty,                                    
(ISNULL((SELECT SUM(STA.TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) where STA.IsEnabled = 1 AND STA.SponsorId = KM.SponsorId                                   
AND STA.EventTicketAttributeId = @VmccId  AND STA.TransactingOptionId IN(5) AND STA.TransactionId IN 
(SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = @VmccId AND T.TransactionStatusId=8)),0)) AS UsedComplimentaryQty,                                               
0 AS PaidBlockedQty,0 AS CompBlockedQty                          
FROM CorporateTicketAllocationDetails KM WITH(NOLOCK)             
INNER JOIN EventTicketAttributes VMC WITH(NOLOCK) ON KM.EventTicketAttributeId=VMC.Id                                                                           
WHERE KM.EventTicketAttributeId = @VmccId AND KM.IsEnabled = 1                                            
GROUP BY KM.RemainingTickets,KM.SponsorId,KM.AllocatedTickets,KM.Id,VMC.Id,KM.EventTicketAttributeId     

SELECT * FROM @TempTable

SELECT ISNULL(SUM(BlockTic),0) AS BlockTic, ISNULL(SUM(AvailableTic),0) AS AvailableTic,ISNULL(SUM(PaidQty),0) AS PaidQty, 
ISNULL(SUM(ComplimentaryQty),0) AS ComplimentaryQty,ISNULL(SUM(UsedPaidQty),0) AS UsedPaidQty, 
ISNULL(SUM(UsedComplimentaryQty),0) AS UsedComplimentaryQty,ISNULL(SUM(PaidBlockedQty),0) AS PaidBlockedQty, 
ISNULL(SUM(CompBlockedQty),0) AS CompBlockedQty FROM @TempTable

SELECT ISNULL(VMCC.RemainingTicketForSale,0) AS AvailableTicket, (SELECT ISNULL(SUM(ISNULL(BlockTic,0)),0) FROM @TempTable) AS BlockTic,
CM.Code AS CurrencyName, VMCC.Price, CMLocal.Code +' '+ CONVERT(VARCHAR(200),VMCC.LocalPrice) AS LocalPrice      
FROM EventTicketAttributes VMCC  WITH(NOLOCK)
INNER JOIN CurrencyTypes CMLocal WITH(NOLOCK) ON VMCC.LocalCurrencyId = CMLocal.Id
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON VMCC.CurrencyId = CM.Id
WHERE VMCC.Id=  @VmccId

SELECT KM.SponsorId, SM.SponsorName FROM Sponsors SM WITH(NOLOCK) 
INNER JOIN CorporateTicketAllocationDetails KM WITH(NOLOCK) ON SM.Id = KM.SponsorId                            
WHERE KM.EventTicketAttributeId = @VmccId and KM.IsEnabled =1       
ORDER BY SM.SponsorName
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetCategorySponsorListByVenue2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetCategorySponsorListByVenue2017]       
(                      
	 @VenueId BIGINT,
	 @EventCatId BIGINT
)                      
AS                      
BEGIN                      
    
SELECT  SM.SponsorName,SM.Id AS SponsorId                             
FROM Sponsors SM                               
WHERE SM.Id IN ( SELECT DISTINCT SponsorId FROM EventSponsorMappings MS WHERE MS.EventDetailId IN
(SELECT Id FROM EventDetails WHERE VenueId = @VenueId AND EventId= @EventCatId) AND  MS.IsEnabled =1) 
AND SM.IsEnabled=1 ORDER BY SM.SponsorName ASC   

END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetCountries]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetCountries]  
AS  
BEGIN 
	SELECT ID AS CountryId, Name AS CountryName, IsoAlphaTwoCode AS CountryCode, Phonecode AS Country_Code,Numcode AS AreaCode
	FROM Countries
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetEventSponsorDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetEventSponsorDetails]
(
	@EventCatId BIGINT,
	@SponsorId INT
)
AS
BEGIN
	SELECT Name AS EventCatName FROM Events WITH(NOLOCK) WHERE Id=@EventCatId
	SELECT SponsorName,FirstName,LastName, Email As EmailId,PhoneCode+'-'+PhoneNumber As MobileNo
	FROM Sponsors WITH(NOLOCK) WHERE Id=@SponsorId
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetHandOverSheetCustomerDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetHandOverSheetCustomerDetails]
(                            
	@TransId BIGINT    
)                      
AS                              
BEGIN                   
	SELECT DISTINCT SM.SponsorName AS SponsorName,SM.Id AS SponsorId ,FirstName,
	LastName,Email,SM.PhoneCode+'-'+PhoneNumber AS MobileNo FROM                           
	Sponsors SM                          
	INNER JOIN CorporateTransactionDetails ST                        
	ON ST.SponsorId=SM.Id                        
	WHERE ST.TransactionId=@TransId
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetHandOverSheetTransDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetHandOverSheetTransDetails]  --201178839                       
(                                                
	@TransId BIGINT            
)                                                
AS                                                
BEGIN
	SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, V.Name AS VenueAddress, C.Name AS CityName,CONVERT(VARCHAR(15),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN TransactingOptionId =4 THEN 'Paid' ELSE 'Complimentary' END AS PGType,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime, T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, 
	TC.Name AS Category, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, NetTicketAmount AS TotalTicAmt, T.CurrencyId, TD.PricePerTicket AS PricePerTic,
	T.TotalTickets AS Quantity
	FROM Transactions T
	INNER JOIN TransactionDetails TD ON T.Id=TD.TransactionId  AND T.Id = @TransId --AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD On T.Id = STD.TransactionId
	INNER JOIN EventTicketAttributes EA ON TD.EventTicketAttributeId=EA.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
	INNER JOIN EventTicketDetails ETD ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED On ETD.EventDetailId = ED.Id
	INNER JOIN Events E ON ED.EventId = E.Id
	INNER JOIN Sponsors S On STD.SponsorId = S.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id

END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetKMIdsByEventIds]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetKMIdsByEventIds]  
(  
 @SponsorId BIGINT,  
 @EventIds VARCHAR(1000),  
 @VenueCatId BIGINT  
)  
AS  
BEGIN  
	SELECT Id AS KM_Id FROM CorporateTicketAllocationDetails WHERE SponsorId=@SponsorId AND EventTicketAttributeId IN(
	SELECT Id FROm EventTicketAttributes WHERE EventTicketDetailId IN (SELECT Id FROM EventTicketDetails WHERE TicketCategoryId=@VenueCatId AND
	EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))))
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetMatchesByVenue2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetMatchesByVenue2017] --12,17
(                                                         
	 @VenueId BIGINT,
	 @EventCatId BIGINT                                                   
)                                                      
AS                                                      
BEGIN                                               

	SELECT ED.Id AS EventId,  ED.Name + ' - ' +CONVERT(VARCHAR(17),ED.StartDateTime,113)  AS EventName      
	FROM EventDetails ED  WITH(NOLOCK)                      
	WHERE ED.VenueId=@VenueId --AND Ed.StartDateTime>=(GETDATE()-45)
	AND ED.EventId= @EventCatId
	ORDER BY Ed.StartDateTime ASC

END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetMatchesTicketDetailsForSponsorByVenue2017_Temp]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetMatchesTicketDetailsForSponsorByVenue2017_Temp]
(      
	@SponsorId BIGINT,      
	@VenueCatId BIGINT,
	@EventCatId BIGINT,
	@VenueId BIGINT 
)      
AS      
BEGIN      

DECLARE  @VmccId TABLE(VmccId BIGINT) 

INSERT INTO @VmccId
SELECT A.Id FROM EventTicketAttributes A
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId = B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT Id FROM EventDetails WHERE EventId=@EventCatId)

DECLARE @tblSonsorReserveDetails TABLE(EventId BIGINT,EventName VARCHAR(200), Status INT)
DECLARE @tblSonsorTransactDetails TABLE(EventId BIGINT, EventName VARCHAR(200), Status INT)
DECLARE @tblMatches Table (RowId INT IDENTITY(1,1), EventId BIGINT)

INSERT INTO @tblMatches
SELECT DISTINCT Id FROM EventDetails WHERE VenueId = @VenueId AND EventId=@EventCatId

DECLARE  @MatchCounter INT = 1, @MatchCount INT =0, @EventIdTemp INT =0,@SumAvailableTic INT = 0

SELECT @MatchCount = ISNULL(COUNT(*),0) FROM @tblMatches
WHILE(@MatchCounter<=@MatchCount)
BEGIN
	SELECT @EventIdTemp = EventId FROM @tblMatches WHERE RowId = @MatchCounter
	SELECT @SumAvailableTic = ISNULL(SUM(RemainingTickets),0) FROM CorporateTicketAllocationDetails  A
	INNER JOIN EventTicketAttributes B ON A.EventTicketAttributeId = B.Id
	INNER JOIN EventTicketDetails C ON B.EventTicketDetailId = C.Id AND C.EventDetailId = @EventIdTemp
	WHERE EventTicketAttributeId
	IN (SELECT VmccId FROM @VmccId) AND SponsorId= @SponsorId 
	
	INSERT INTO @tblSonsorTransactDetails
	SELECT @EventIdTemp, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets For Transaction <b>' + 
	CONVERT(VARCHAR(200),@SumAvailableTic) +'</b> )' AS EventName,
	CASE WHEN  CAST(ED.StartDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status      
	FROM EventDetails ED 
	WHERE ED.Id = @EventIdTemp
	ORDER BY StartDateTime ASC	
	
	SET @MatchCounter= @MatchCounter + 1
END

	INSERT INTO @tblSonsorReserveDetails
	SELECT ED.Id, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets <b>' + 
	CONVERT(VARCHAR(200),ISNULL(SUM(VMCC.RemainingTicketForSale),0)) +'</b> )' AS EventName,
	CASE WHEN  CAST(ED.StartDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status      
	FROM EventDetails ED
	INNER JOIN EventTicketDetails B On ED.Id = b.EventDetailId
	INNER JOIN EventTicketAttributes VMCC ON B.Id = VMCC.EventTicketDetailId
	WHERE B.TicketCategoryId = @VenueCatId
	AND ED.Id IN(SELECT EventId FROM @tblMatches)
	GROUP BY ED.Id,ED.Name,VMCC.RemainingTicketForSale, ED.StartDateTime
	ORDER BY StartDateTime ASC
	
	SELECT DISTINCT * FROM @tblSonsorReserveDetails
	SELECT DISTINCT * FROM @tblSonsorTransactDetails

END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetMatchSponsors]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetMatchSponsors]  --1195               
(                    
  @MatchId BIGINT                    
)                    
AS                    
BEGIN                    
SELECT EVD.Name AS EventName, EVD.StartDateTime AS EventStartDate, V.Name AS VenueAddress                        
FROM EventDetails EVD                     
INNER JOIN Venues V ON EVD.VenueId = V.Id                    
WHERE EVD.Id = @MatchId                    

SELECT M.SponsorName,M.Id AS SponsorId,MS.EventDetailId AS EventId                     
FROM Sponsors M                    
INNER JOIN  EventSponsorMappings MS ON M.Id = MS.SponsorId                    
WHERE MS.EventDetailId = @MatchId                    

Select  M.SponsorName,M.Id AS SponsorId
FROM Sponsors M                       
WHERE M.Id NOT IN                   
(                  
	SELECT MM.SponsorId from EventSponsorMappings MM                  
	WHERE MM.EventDetailId = @MatchId
) AND M.IsEnabled=1 ORDER BY M.SponsorName ASC

Select  M.SponsorName,M.Id AS SponsorId
FROM Sponsors M                       
WHERE M.Id IN                   
(  
	SELECT MM.SponsorId from EventSponsorMappings MM                  
	WHERE MM.EventDetailId = @MatchId 
) AND M.IsEnabled=1 ORDER BY M.SponsorName ASC
                 
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetMatchStands]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetMatchStands] 
(  
	@EventId BIGINT  
)  
AS  
BEGIN 

	DECLARE @tblStandCoordinateIds TABLE(CoordinateId INT, VenueCatName VARCHAR(500), VenueCatId INT)
	
	INSERT INTO @tblStandCoordinateIds
	SELECT A.ID, C.Name AS VenueCatName,C.Id As VenueCatId FROM DynamicStadiumCoordinates A
	INNER JOIN DynamicStadiumTicketCategoriesDetails B ON A.Id=B.DynamicStadiumCoordinateId
	INNER JOIN TicketCategories C ON B.TicketCategoryId=C.Id
	WHERE A.VenueId IN  
    (SELECT VenueId FROM EventDetails WHERE Id=@EventId)  
	ORDER BY DisplayName
	
	SELECT CoordinateId AS ID, VenueCatName AS DisplayName FROM(
	SELECT *,ROW_NUMBER() OVER(PARTITION BY VenueCatId ORDER BY VenueCatId) AS RowRank FROM @tblStandCoordinateIds
	) RESULT
	WHERE RESULT.RowRank =1
END

GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetOrderSummaryByVenue]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetOrderSummaryByVenue]                                             
(                                                                          
 @TransId BIGINT                                                                        
)                                                                          
AS 
BEGIN
	SELECT DISTINCT T.Id AS EventTransID, E.Id AS EventCatId, E.Name AS EventCatName, ED.Name AS EventName, EA.Id AS VmCC_Id, V.Name +', '+ C.Name AS VenueAddress,CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN STD.TransactingOptionId =1 THEN 'Paid' ELSE 'Complimentary' END AS PGType, V.Id AS VenueId,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime,T.ServiceCharge AS ServiceTax, T.ConvenienceCharges AS ConvenienceCharge, 0 AS Othercharge,
	ED.Id AS EventId, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType, S.PhoneCode+'-'+ S.PhoneNumber AS ContactNumber,
	T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, S.FirstName AS Cust_FirstName, S.LastName As Cust_LastName, '' AS Cust_IdType, '' AS Cust_IdTypeNumber,
	S.PhoneCode+'-'+ S.PhoneNumber AS Cust_MobileNumber, S.Email AS Cust_Email,
	TC.Name AS VenueCatName, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, GrossTicketAmount  AS TotalCharge,NetTicketAmount AS TotalTicAmt, 4 AS DeliveryType,
	'' AS Courier_Charge, '' AS VP_Name, '' AS Venue_Add, T.CurrencyId, TD.PricePerTicket AS PricePerTic, TD.ConvenienceCharges AS ConvChargePerTicket,
	TD.ServiceCharge AS ServiceTaxPerTicket, ISNULL(T.ConvenienceCharges,0.00) + ISNULL(T.ServiceCharge,0.00) AS TotalConvFee,
	NetTicketAmount AS TotalTicketAmmount, TD.DiscountAmount AS DiscountPerTicket, 0 AS IsWaiveConvFee, 0 AS IsWaiveServiceTax, CM.Code AS LocalCurrency, '' AS SeatNumber,
	V.Name +', '+ C.Name AS VenueCity, TPD.PaymentDetail AS PayConfNumber, 4 AS DeliveryType, 0 AS Othercharge, '' AS VenuePickupName,TD.PricePerTicket AS LocalPricePerTicket,
	'' AS VenuePickupAddress
	FROM Transactions T
	INNER JOIN TransactionDetails TD ON T.Id=TD.TransactionId  AND T.Id = @TransId AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD On T.Id = STD.TransactionId --AND STD.SponsorId = @SponsorId
	INNER JOIN EventTicketAttributes EA ON TD.EventTicketAttributeId=EA.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
	INNER JOIN EventTicketDetails ETD ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED On ETD.EventDetailId = ED.Id
	INNER JOIN Events E ON ED.EventId = E.Id
	INNER JOIN Sponsors S On STD.SponsorId = S.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetPreConfirmationDetailsByVenue]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetPreConfirmationDetailsByVenue]                                                       
(                                                                          
 @TransId BIGINT,  
 @SponsorId INT                                                                          
)                                                                          
AS 
BEGIN
	SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, EA.Id AS VmCC_Id, V.Name +', '+ C.Name AS VenueAddress,CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN STD.TransactingOptionId =1 THEN 'Paid' ELSE 'Complimentary' END AS PGType, V.Id AS VenueId,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME))  AS EventStartTime,T.ServiceCharge AS ServiceTax, T.ConvenienceCharges AS ConvenienceCharge, 0 AS Othercharge,
	ED.Id AS EventId, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType, S.PhoneCode+'-'+ S.PhoneNumber AS ContactNumber,
	T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, S.FirstName AS Cust_FirstName, S.LastName As Cust_LastName, '' AS Cust_IdType, '' AS Cust_IdTypeNumber,
	S.PhoneCode+'-'+ S.PhoneNumber AS Cust_MobileNumber, S.Email AS Cust_Email,
	TC.Name AS VenueCatName, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, GrossTicketAmount  AS TotalCharge,NetTicketAmount AS TotalTicAmt, 4 AS DeliveryType,
	'' AS Courier_Charge, '' AS VP_Name, '' AS Venue_Add, T.CurrencyId, TD.PricePerTicket AS PricePerTic
	FROM Transactions T
	INNER JOIN TransactionDetails TD ON T.Id=TD.TransactionId  AND T.Id = @TransId --AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD On T.Id = STD.TransactionId AND STD.SponsorId = @SponsorId
	INNER JOIN EventTicketAttributes EA ON TD.EventTicketAttributeId=EA.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
	INNER JOIN EventTicketDetails ETD ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED On ETD.EventDetailId = ED.Id
	INNER JOIN Events E ON ED.EventId = E.Id
	INNER JOIN Sponsors S On STD.SponsorId = S.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetPreConfirmationDetailsForSponsor]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetPreConfirmationDetailsForSponsor]                                                      
(                                                                        
	@TransId BIGINT,
	@SponsorId INT                                                                        
)                                                                        
AS                      
BEGIN
	SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, EA.Id AS VmCC_Id, V.Name +', '+ C.Name AS VenueAddress,CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN TransactingOptionId =4 THEN 'Paid' ELSE 'Complimentary' END AS PGType, V.Id AS VenueId,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime,T.ServiceCharge AS ServiceTax, T.ConvenienceCharges AS ConvenienceCharge, 0 AS Othercharge,
	ED.Id AS EventId, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType, S.PhoneCode+'-'+ S.PhoneNumber AS ContactNumber,
	T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, S.FirstName AS Cust_FirstName, S.LastName As Cust_LastName, '' AS Cust_IdType, '' AS Cust_IdTypeNumber,
	S.PhoneCode+'-'+ S.PhoneNumber AS Cust_MobileNumber, S.Email AS Cust_Email,
	TC.Name AS VenueCatName, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, NetTicketAmount AS TotalTicAmt, 4 AS DeliveryType,
	'' AS Courier_Charge, '' AS VP_Name, '' AS Venue_Add, T.CurrencyId, TD.PricePerTicket AS PricePerTic
	FROM Transactions T
	INNER JOIN TransactionDetails TD ON T.Id=TD.TransactionId  AND T.Id = @TransId --AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD On T.Id = STD.TransactionId AND STD.SponsorId = @SponsorId
	INNER JOIN EventTicketAttributes EA ON TD.EventTicketAttributeId=EA.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
	INNER JOIN EventTicketDetails ETD ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED On ETD.EventDetailId = ED.Id
	INNER JOIN Events E ON ED.EventId = E.Id
	INNER JOIN Sponsors S On STD.SponsorId = S.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetPrintHandOverSheetDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetPrintHandOverSheetDetails] -- 20032
(
	@TransId BIGINT --= 201145872
)                
AS                
BEGIN

SELECT DISTINCT A.TransactionId AS TransId, B.TotalTickets AS QuantityPrinted, SerialStart AS SerialFrom,
C.PhoneCode+'-'+ C.PhoneNumber AS Usermobile,
SerialEnd AS SerialTo, TicketHandedBy AS HandedBy, TicketHandedTo AS SubmittedTo
FROM HandoverSheets A
INNER JOIN Transactions B On A.TransactionId =B.Id AND A.TransactionId = @TransId
INNER JOIN Users C On A.CreatedBy = C.AltId

SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, V.Name AS VenueAddress, C.Name AS CityName,CONVERT(VARCHAR(15),ED.StartDateTime,113) AS EventStartDate,
CASE WHEN TransactingOptionId =4 THEN 'Paid' ELSE 'Complimentary' END AS PGType,
CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime, T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, 
TC.Name AS VenueCatName, CM.Code AS LocalCurrency, T.DiscountAmount AS DiscountAmt, NetTicketAmount AS TotalTicAmt, T.CurrencyId, 
TD.PricePerTicket AS LocalPricePerTic, TD.TotalTickets AS Qty, CONVERT(VARCHAR, GETDATE(), 107) AS DATE
FROM Transactions T
INNER JOIN TransactionDetails TD ON T.Id=TD.TransactionId  AND T.Id = @TransId --AND T.TransactionStatusId = 8
INNER JOIN TransactionPaymentDetails TPD ON T.Id=TPD.TransactionId
INNER JOIN  CorporateTransactionDetails STD On T.Id = STD.TransactionId
INNER JOIN EventTicketAttributes EA ON TD.EventTicketAttributeId=EA.Id
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
INNER JOIN EventTicketDetails ETD ON EA.EventTicketDetailId  = ETD.Id
INNER JOIN TicketCategories TC ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails ED On ETD.EventDetailId = ED.Id
INNER JOIN Events E ON ED.EventId = E.Id
INNER JOIN Sponsors S On STD.SponsorId = S.Id
INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id

DECLARE @SponsorId BIGINT
 SELECT @SponsorId = SponsorId FROM CorporateTransactionDetails WHERE TransactionId = @TransId

SELECT DISTINCT SM.SponsorName AS CompanyName,
CASE WHEN ISNULL(CMO.SponsorId,0) > 0 THEN CASE WHEN CMO.PickupRepresentativeFirstName <> '' 
THEN CMO.PickupRepresentativeFirstName +' '+ PickupRepresentativeLastName ELSE CMO.FirstName +' '+CMO.LastName END ELSE SM.FirstName+' '+SM.LastName END 
AS SponsorName,SM.Email AS EmailId,
CONVERT(VARCHAR, GETDATE(), 107) AS DATE, SM.PhoneCode+'-'+SM.PhoneNumber AS MobNo, 0 AS AltMobNo               
FROM Sponsors SM  
LEFT OUTER JOIN CorporateRequests CMO ON SM.Id = CMO.SponsorId 
WHERE SM.Id = @SponsorId

END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSeatAvailability]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[KITMS_GetSeatAvailability] --9997           
(            
	@KITMSMasterId bigint       
)            
AS             
BEGIN   
	DECLARE @SponsorId BIGINT, @Vmcc_Id BIGINT
	SELECT @SponsorId=SponsorId, @Vmcc_Id=EventTicketAttributeId FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId
	SELECT RemainingTickets FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId  
	SELECT COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK)
	INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
	WHERE A.SponsorId = @SponsorId AND A.EventTicketDetailId = @Vmcc_Id AND B.SeatTypeId =3
	AND A.SeatStatusId=1
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSeatLayoutByVMCC]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetSeatLayoutByVMCC] --23
(
	@VmccId INT
)
AS
BEGIN

	SELECT  A.MatchLayoutSectionSeatId AS SeatId,B.SeatTag,C.Id AS SectionId, B.RowNumber,B.ColumnNumber, ISNULL(B.SeatTypeId,1) AS SeatType, A.IsEnabled AS Status,
	A.SeatStatusId AS SoldStatus, ISNULL(A.PrintStatusId,0) AS IsPrint, 
	CASE WHEN B.SeatTypeId = 3 THEN 1 ELSE 0 END AS IsBlock, A.Id AS TicNUmId, ISNULL(A.SponsorId,0) AS SponsorId,
	 0 AS IsWheelChair, C.SectionName
	FROM MatchSeatTicketDetails A WITH (NOLOCK)
	INNER JOIN MatchLayoutSectionSeats B ON A.MatchLayoutSectionSeatId=B.Id                        
	INNER JOIN MatchLayoutSections C ON B.MatchLayoutSectionId=C.Id                        
	INNER JOIN EventTicketDetails D ON A.EventTicketDetailId = D.Id
	INNER JOIN EventTicketAttributes G ON D.Id = G.EventTicketDetailId
	INNER JOIN TicketCategories E ON D.TicketCategoryId = E.Id
	WHERE A.Id=@VmccId
	ORDER BY A.MatchLayoutSectionSeatId ASC
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorConfirmationDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetSponsorConfirmationDetails]  --20008,11                                                    
(                                                                        
	@TransId BIGINT,
	@SponsorId INT                                                                        
)                                                                        
AS                      
BEGIN
	SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, EA.Id AS VmCC_Id, V.Name +', '+ C.Name AS VenueAddress,CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN TransactingOptionId =4 THEN 'Paid' ELSE 'Complimentary' END AS PGType, V.Id AS VenueId,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime,T.ServiceCharge AS ServiceTax, T.ConvenienceCharges AS ConvenienceCharge, 0 AS Othercharge,
	ED.Id AS EventId, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType, S.PhoneCode+'-'+ S.PhoneNumber AS ContactNumber,
	T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, S.FirstName AS Cust_FirstName, S.LastName As Cust_LastName, '' AS Cust_IdType, '' AS Cust_IdTypeNumber,
	S.PhoneCode+'-'+ S.PhoneNumber AS Cust_MobileNumber, S.Email AS Cust_Email,
	TC.Name AS VenueCatName, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, NetTicketAmount AS TotalTicAmt, 4 AS DeliveryType,
	'' AS Courier_Charge, '' AS VP_Name, '' AS Venue_Add, T.CurrencyId, TD.PricePerTicket AS PricePerTic
	FROM Transactions T
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id=TD.TransactionId  AND T.Id = @TransId AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD WITH(NOLOCK) On T.Id = STD.TransactionId AND STD.SponsorId = @SponsorId
	INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON TD.EventTicketAttributeId=EA.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Sponsors S WITH(NOLOCK) On STD.SponsorId = S.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetSponsorDetails]
(
	@SponsorId INT
)
AS
BEGIN
	SELECT A.Id AS SponsorId,SponsorName, CompanyAddress AS AddressLine1,'' AS AddressLine2,
	CompanyCity  AS City, CompanyState AS State, CompanyCountry AS Country, CompanyZipcodeId AS postalcode,
	FirstName As P_FName, LastName AS P_LName, 0 AS P_Gender, Email AS P_EmailId, A.Phonecode+'-'+A.PhoneNumber AS P_MobileNo,
	IdType AS P_IDType, IdNumber AS P_IDNo, '' AS S_FName, '' AS S_LName, 0 AS S_Gender, '' AS S_EmailId, '' AS S_MobileNo, '' AS S_IDType,
	'' AS S_IDNo
	FROM Sponsors A  WITH(NOLOCK)
	 WHERE A.Id=@SponsorId AND A.IsEnabled=1
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorDetailsByVenue2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[KITMS_GetSponsorDetailsByVenue2017] --  3214, 607          
(                    
	@VenueCatId BIGINT, 
	@EventCatId BIGINT,   
	@VenueId BIGINT                    
)                    
AS                    
BEGIN                
CREATE TABLE #Events(EventId BIGINT, VmccId BIGINT )                    

INSERT INTO #Events 
SELECT B.EventDetailId, A.Id FROM EventTicketAttributes A WITH(NOLOCK)
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT Id FROM EventDetails WHERE EventId=@EventCatId)

SELECT MS.Id AS SponsorId, MS.SponsorName                                                            
FROM Sponsors MS  WITH(NOLOCK)                                                           
INNER JOIN EventSponsorMappings S WITH(NOLOCK)                                                         
ON MS.Id = S.SponsorId                                                            
Where S.EventDetailId IN (SELECT EventId FROM #Events) AND MS.IsEnabled=1                                                         
AND  MS.Id NOT IN                                                             
(SELECT DISTINCT ST.SponsorId FROM CorporateTicketAllocationDetails ST WITH(NOLOCK) WHERE EventDetailId 
IN (SELECT EventId FROM #Events) AND EventTicketAttributeId IN (SELECT VmccId FROM #Events) and ST.IsEnabled = 1)                     
GROUP BY MS.Id, MS.SponsorName                                         

SELECT ST.SponsorId, SM.SponsorName, SUM(ST.AllocatedTickets) as SponsorBlocked, SUM(ST.RemainingTickets) as UnClassifiedBlocked
FROM CorporateTicketAllocationDetails ST  WITH(NOLOCK)
JOIN Sponsors SM WITH(NOLOCK) on SM.Id= ST.SponsorId
WHERE EventTicketAttributeId IN (SELECT VmccId FROM #Events) AND ST.IsEnabled = 1                     
GROUP BY ST.SponsorId, SM.SponsorName              

SELECT CONVERT(VARCHAR(MAX),ED.Name) + ' ( Available Tic ' + convert(varchar(200),ETA.RemainingTicketForSale) +' )' AS EventName FROM
EventDetails ED  WITH(NOLOCK)
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id= ETD.EventDetailId
INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON ETD.Id= ETA.EventTicketDetailId
WHERE ETD.TicketCategoryId = @VenueCatId AND ED.VenueId = @VenueId
   
SELECT ISNULL(SUM(ETA.RemainingTicketForSale),0) as AvailableTicket FROM
EventDetails ED  WITH(NOLOCK)
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id= ETD.EventDetailId
INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETD.Id= ETA.EventTicketDetailId
WHERE ETD.TicketCategoryId = @VenueCatId AND ED.VenueId = @VenueId AND ED.Id IN(SELECT Id FROM  EventDetails WHERE EventId=@EventCatId)
   
SELECT 
(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2) AND STD.IsEnabled=1
INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0))  ttlUsedPaidCompSeats,

(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId = 2 AND STD.IsEnabled=1
INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0)) Complimentary,

(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2)
INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId),0))  BlockUsed

FROM EventTicketDetails ETD
INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON ETD.Id=EA.EventTicketDetailId AND ETD.TicketCategoryId = @VenueCatId
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.CurrencyId=CM.Id
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
WHERE ETD.TicketCategoryId = @VenueCatId

DROP TABLE #Events
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorId]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetSponsorId]       
(          
	@KITMSMasterId BIGINT     
)          
AS           
BEGIN                  
 SELECT SponsorId FROM CorporateTicketAllocationDetails WITH(NOLOCK)  WHERE Id=@KITMSMasterId AND IsEnabled=1  
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorList]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetSponsorList] 
(
	@SponsorName VARCHAR(500)
)
AS
BEGIN
	SELECT Id AS SponsorId, SponsorName FROM Sponsors WITH(NOLOCK) WHERE UPPER(SponsorName) LIKE '%'+UPPER(@SponsorName)+'%' AND IsEnabled=1
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorListByVenue2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetSponsorListByVenue2017] --55, 55                    
(                            
	@VenueId BIGINT,
	@EventCatId BIGINT
)                            
AS                            
BEGIN    
                        
SELECT  SM.SponsorName,SM.Id AS SponsorId                             
FROM Sponsors SM WITH(NOLOCK)                               
WHERE SM.IsEnabled=1 ORDER BY SM.SponsorName ASC  
       
SELECT  SM.SponsorName,SM.Id AS SponsorId                             
FROM Sponsors SM  WITH(NOLOCK)                              
WHERE SM.Id IN ( SELECT DISTINCT SponsorId FROM EventSponsorMappings MS WITH(NOLOCK) WHERE MS.EventDetailId IN
	(SELECT Id FROM EventDetails WITH(NOLOCK) WHERE VenueId = @VenueId AND EventId= @EventCatId) AND  MS.IsEnabled =1) 
AND SM.IsEnabled=1 ORDER BY SM.SponsorName ASC  
                             
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorPreconfirmationDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetSponsorPreconfirmationDetails]                  
(                                        
	@KM_ID BIGINT                                                       
)                                        
AS                                        
BEGIN                        
                     
DECLARE @EventId BIGINT, @SponsorId BIGINT, @VMCC_Id BIGINT, @SPT_Id BIGINT                    
SELECT @SponsorId=SponsorId,@VMCC_Id = EventTicketAttributeId FROM  
CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KM_ID 
	   
SELECT S.SponsorName, SATD.SponsorId,ED.Id AS EventId,E.Name AS EventCatName, CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
E.Id AS EventCatId, ED.Name AS EventName, V.Name +', '+ C.Name AS VenueAddress, TC.Name AS Category, ED.VenueId, EA.LocalPrice AS PricePerTic,
EA.Id AS Vmcc_Id, ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,1),0) AS ConvenceCharge, ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,2),0) AS TicketServiceTax,
CM.Code AS CurrencyName, CM.Code AS LocalCurrencyName
FROM Sponsors S WITH(NOLOCK)
INNER JOIN CorporateTicketAllocationDetails SATD WITH(NOLOCK) ON S.Id=SATD.SponsorId AND SATD.Id = @KM_ID
INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON SATD.EventTicketAttributeId=EA.Id
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.LocalCurrencyId=CM.Id
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id

SELECT SponsorName,FirstName AS P_FName,LastName AS P_LName, Email AS P_EmailId, PhoneCode +'-'+ PhoneNumber AS P_MobileNo, '' AS P_IDType, '' AS  P_IDNo
FROM Sponsors WITH(NOLOCK) WHERE Id=@SponsorId  
                             
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetSponsorsDetailsByTransaction]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetSponsorsDetailsByTransaction]
(                                                                      
	@TransId BIGINT,
	@SponsorId INT                                                                      
)  
AS  
BEGIN
	SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, EA.Id AS VmCC_Id, V.Name +', '+ C.Name AS VenueAddress,CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN STD.TransactingOptionId =1 THEN 'Paid' ELSE 'Complimentary' END AS PGType, V.Id AS VenueId,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME))  AS EventStartTime,T.ServiceCharge AS ServiceTax, T.ConvenienceCharges AS ConvenienceCharge, 0 AS Othercharge,
	ED.Id AS EventId, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType, S.PhoneCode+'-'+ S.PhoneNumber AS ContactNumber,
	T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, S.FirstName AS Cust_FirstName, S.LastName As Cust_LastName, '' AS Cust_IdType, '' AS Cust_IdTypeNumber,
	S.PhoneCode+'-'+ S.PhoneNumber AS Cust_MobileNumber, S.Email AS Cust_Email,
	TC.Name AS VenueCatName, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, GrossTicketAmount  AS TotalCharge,NetTicketAmount AS TotalTicAmt, 4 AS DeliveryType,
	'' AS Courier_Charge, '' AS VP_Name, '' AS Venue_Add, T.CurrencyId,TD.PricePerTicket AS PricePerTic
	FROM Transactions T
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id=TD.TransactionId  AND T.Id = @TransId AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD WITH(NOLOCK) On T.Id = STD.TransactionId AND STD.SponsorId = @SponsorId
	INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON TD.EventTicketAttributeId=EA.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Sponsors S  WITH(NOLOCK) On STD.SponsorId = S.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetStandDetails_New]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetStandDetails_New] --2019,0
(                                                                              
	@EventId BIGINT,      
	@ID BIGINT=null                                                                              
)                                                                            
AS                                                                              
BEGIN

DECLARE @tblEventTicketCategory TABLE(Id BIGINT)
IF(@ID IS NOT NULL AND @ID<>0)
BEGIN
	INSERT INTO @tblEventTicketCategory
	SELECT TicketCategoryId FROm DynamicStadiumTicketCategoriesDetails WHERE DynamicStadiumCoordinateId= @Id
END 
ELSE
BEGIN
    INSERT INTO @tblEventTicketCategory
	SELECT DISTINCT TicketCategoryId FROm EventTicketDetails WHERE EventDetailId = @EventId
END 
 
SELECT  ED.EventId, ETA.Id AS VMCC_ID, CT.Code + ' ' + CONVERT(VARCHAR,CONVERT(DECIMAL(10,2),ETA.Price)) +' <br/> ' +
CTLocal.Code + ' ' + CONVERT(VARCHAR,CONVERT(DECIMAL(10,2),ETA.LocalPrice)) AS Price,
ED.IsEnabled AS EventStatus, TC.Name AS Category, TC.Name AS VenueCatName, ETA.AvailableTicketForSale AS TotalTics,
(ISNULL((SELECT SUM(VMT.TotalTickets) FROM Transactions T  WITH(NOLOCK)                                       
  INNER JOIN TransactionDetails VMT WITH(NOLOCK) ON T.Id = VMT.TransactionId                                                                            
  where T.TransactionStatusId = 8 AND VMT.EventTicketAttributeId = ETA.Id),0)                                                                           
) AS SoldTic, 0 AS BlockedTic, ETA.RemainingTicketForSale As RemainingTics,
(ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                             
 FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK) INNER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id = KM.SponsorId                                  
 WHERE KM.EventTicketAttributeId = ETA.Id and KM.IsEnabled = 1 AND SM.SponsorTypeId=1),0)) AS SponsoredTickets,
 (ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                             
 FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK) INNER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id = KM.SponsorId                                  
 WHERE KM.EventTicketAttributeId = ETA.Id and KM.IsEnabled = 1 AND SM.SponsorTypeId=0),0)) AS SeatKills,

 (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.IsEnabled = 1                                     
  AND STA.EventTicketAttributeId = ETA.Id  AND STA.TransactingOptionId IN(1,2) AND STA.TransactionId IN 
  (SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id AND T.TransactionStatusId=8)),0))as ttlUsedPaidCompSeats,

  (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.IsEnabled = 1                                     
  AND STA.EventTicketAttributeId = ETA.Id  AND STA.TransactingOptionId IN(2) AND STA.TransactionId IN 
  (SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id AND T.TransactionStatusId=8)),0))as Complimentary,

  (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.EventTicketAttributeId = ETA.Id  
  AND STA.TransactingOptionId IN(1,2) AND STA.TransactionId IN 
  (SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id)),0)) AS BlockUsed,

  (ISNULL((SELECT SUM(TD.TotalTickets) TransactionId FROM TransactionDetails TD WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id AND T.TransactionStatusId=8),0)) AS PublicSales,
  CASE ETA.IsEnabled WHEN 1 THEN 'Active' ELSE 'Inactive' END AS [Status],ETA.IsEnabled AS Status, ETA.LocalPrice AS PricePerTic
 FROM                                         
	EventTicketAttributes ETA WITH(NOLOCK)
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id       
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id                                                                  
	INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.CurrencyId=CT.Id
	INNER JOIN CurrencyTypes CTLocal WITH(NOLOCK) ON ETA.LocalCurrencyId=CTLocal.Id                                               
 WHERE                                                                               
 ED.Id = @EventId   AND ETD.TicketCategoryId IN(SELECT * FROM @tblEventTicketCategory)                                                    
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetStandDetailsByVenue2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetStandDetailsByVenue2017] --55,55,0
(                                                                                    
	@VenueId BIGINT,
	@EventCatId BIGINT,
	@VenueCatId BIGINT=0                                                                              
)                                                                                  
AS                                                                              
BEGIN   
DECLARE @tblMatches Table (EventId BIGINT)
DECLARE @tblVMCCIds Table (VMCC_Id BIGINT)
DECLARE @EventType BIGINT                         

SET @EventType = @EventCatId

INSERT INTO @tblMatches
SELECT Id FROM EventDetails WHERE VenueId = @VenueId AND EventId=@EventType

	DECLARE @tblEventTicketCategory TABLE(Id BIGINT)
	IF(@VenueCatId IS NOT NULL AND @VenueCatId<>0)
	BEGIN
		INSERT INTO @tblEventTicketCategory
		SELECT TicketCategoryId FROm DynamicStadiumTicketCategoriesDetails WHERE DynamicStadiumCoordinateId= @VenueCatId
	END 
	ELSE
	BEGIN
		INSERT INTO @tblEventTicketCategory
		SELECT DISTINCT TicketCategoryId FROm EventTicketDetails WHERE EventDetailId IN
		(SELECT Id FROm EventDetails WHERE VenueId = @VenueId AND EventId = @EventCatId)
	END
	    INSERT INTO @tblVMCCIds
		SELECT Id FROm EventTicketAttributes WHERE EventTicketDetailId IN 
		(SELECT Id FROM EventTicketDetails WHERE EventDetailId IN(SELECT EventId FROM @tblMatches)
		AND TicketCategoryId IN(SELECT Id from @tblEventTicketCategory))

	SELECT  DISTINCT TC.ID AS VenueCatId,-- VMCC.Status,
	TC.Name  AS Category,
	(SELECT ISNULL(COUNT(*),0) FROM @tblMatches) AS TotalMatches,
	(ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                             
	FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK) INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on SM.Id = KM.EventTicketAttributeId
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId                                  
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND KM.IsEnabled = 1),0)) AS SponsoredTickets,

	(SELECT ISNULL(SUM(AvailableTicketForSale),0) FROM EventTicketAttributes ETA
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId 
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalTickets,

	(SELECT ISNULL(SUM(RemainingTicketForSale),0) FROM EventTicketAttributes ETA
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId 
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalAvailTickets,

	(SELECT ISNULL(SUM(AvailableTicketForSale),0) FROM EventTicketAttributes ETA
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId 
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalTics,
	(SELECT ISNULL(SUM(RemainingTicketForSale),0) FROM EventTicketAttributes ETA
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId 
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) RemainingTics,
	0 AS BlockedTic,

	(ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                             
	FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK)  INNER JOIN Sponsors SM WITH(NOLOCK) on SM.Id = KM.SponsorId AND SM.SponsorTypeId=0
	INNER JOIN EventTicketAttributes ETATemp WITH(NOLOCK) on ETATemp.Id = KM.EventTicketAttributeId
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETATemp.EventTicketDetailId                                  
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND KM.IsEnabled = 1  AND SM.SponsorTypeId=0),0)) SeatKills,

	(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
	INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2) AND STD.IsEnabled=1
	INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0))  ttlUsedPaidCompSeats,

	(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
	INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId = 2 AND STD.IsEnabled=1
	INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0)) Complimentary,

	(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
	INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2)
	INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId),0))  BlockUsed,

	(ISNULL((SELECT SUM(TD.TotalTickets)                                                                             
	FROM  Transactions T WITH(NOLOCK)
	INNER JOIN TransactionDetails TD WITH(NOLOCK) on T.Id = TD.TransactionId AND T.TransactionStatusId =8 --AND T.ChannelId = 1
	INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on SM.Id = TD.EventTicketAttributeId
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId    
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND T.TransactionStatusId=8),0))  PublicSales,

	(ISNULL((SELECT SUM(TD.TotalTickets)                                                                             
	FROM  Transactions T WITH(NOLOCK)
	INNER JOIN TransactionDetails TD WITH(NOLOCK) on T.Id = TD.TransactionId AND T.TransactionStatusId =8
	INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on SM.Id = TD.EventTicketAttributeId
	INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId    
	WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId),0))  SoldTic

	FROM EventTicketDetails ETD
	INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON ETD.Id=EA.EventTicketDetailId AND EA.Id IN(SELECT VMCC_Id FROM @tblVMCCIds)
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.CurrencyId=CM.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
	WHERE EA.Id IN(SELECT VMCC_Id FROM @tblVMCCIds)
	AND ETD.TicketCategoryId IN(SELECT * FROM @tblEventTicketCategory)

END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetStandsByVenue2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetStandsByVenue2017] 
(  
	@VenueId BIGINT 
)  
AS  
BEGIN 

	DECLARE @tblStandCoordinateIds TABLE(CoordinateId INT, VenueCatName VARCHAR(500), VenueCatId INT)
	
	INSERT INTO @tblStandCoordinateIds
	SELECT A.ID, C.Name AS VenueCatName,C.Id As VenueCatId FROM DynamicStadiumCoordinates A
	INNER JOIN DynamicStadiumTicketCategoriesDetails B ON A.Id=B.DynamicStadiumCoordinateId
	INNER JOIN TicketCategories C ON B.TicketCategoryId=C.Id
	WHERE A.VenueId = @VenueId 
	ORDER BY DisplayName
	
	SELECT CoordinateId AS ID, VenueCatName AS DisplayName FROM(
	SELECT *,ROW_NUMBER() OVER(PARTITION BY VenueCatId ORDER BY VenueCatId) AS RowRank FROM @tblStandCoordinateIds
	) RESULT
	WHERE RESULT.RowRank =1
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetsubMenu]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetsubMenu]  
(      
 @MenuId INT, 
 @Userid int    
)      
AS      
BEGIN      
 SELECT @UserId AS UserId, Id AS MenuId, FeatureName AS MenuName, 0 AS DisplayOrder, RedirectUrl AS Url, ParentFeatureId AS ParentId
  FROM Features WHERE ParentFeatureId=@MenuId AND IsEnabled =1
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetTicketAvaialabilityForTransaction_Temp]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetTicketAvaialabilityForTransaction_Temp]  --140,'58618,58620,58622','14180,14181',2641,10,'KyaZoongaTom'                                    
(                                      
	 @SponsorId BIGINT,                                      
	 @EventIds VARCHAR(MAX),    
	 @KM_Ids VARCHAR(MAX),                                  
	 @VenueCatId BIGINT,                                       
	 @tics INT,                                      
	 @UpdateBY VARCHAR(500)                                       
)                                      
AS                                      
BEGIN

BEGIN TRANSACTION trans
BEGIN TRY
                                    
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @EventTicketAttributeId BIGINT, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='' 
DECLARE @ReturnValFail VARCHAR(5000) ='', @Count int =0                 
            
DECLARE  @KMIDS TABLE(KMID BIGINT)
INSERT INTO @KMIDS 
SELECT * FROM SplitString(@KM_Ids,',')

DECLARE @KM_ID BIGINT
DECLARE curEvent CURSOR FOR                                 
SELECT * FROM @KMIDS              
OPEN  curEvent;               
FETCH NEXT FROM curEvent INTO @KM_ID              
WHILE @@FETCH_STATUS=0              
BEGIN   

	SELECT @BlockTic = AllocatedTickets, @AvailableTic = RemainingTickets, @EventTicketAttributeId = EventTicketAttributeId
	FROM CorporateTicketAllocationDetails 
	WHERE SponsorId= @SponsorId AND Id=@KM_ID

	IF(@AvailableTic >= @tics)              
	BEGIN
		SET @ReturnVal=1   	                      
	END                  
	ELSE                          
	BEGIN                
		SELECT @EventName = ED.Name 
		FROm EventDetails ED
		INNER JOIN EventTicketDetails ETD WITH(NOLOCK) on ED.Id = ETD.EventDetailId
		INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on ETD.Id = SM.EventTicketDetailId
		WHERE SM.Id= @EventTicketAttributeId 
		SET @Count+=1   
		SET @ReturnValFail += 'Only '+ CONVERT(Varchar(100),@AvailableTic) + ' tickets are available for transaction for <b>'+ @EventName + '</b>,<br/> '           
	END                          
FETCH NEXT FROM curEvent INTO @KM_ID              
END               
CLOSE curEvent;               
DEALLOCATE curEvent;              

IF(@Count>0)                          
BEGIN                       
	SELECT @ReturnValFail AS Result 
END 
ELSE
BEGIN
	SELECT @ReturnVal AS Result
END             

COMMIT TRANSACTION trans
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION trans 
END CATCH                              
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetTicketCategoryDetailsByKMId]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetTicketCategoryDetailsByKMId]   
(  
 
	@KM_Id BIGINT 
)  
AS  
BEGIN  
	DECLARE @EventId BIGINT, @SponsorId BIGINT, @VMCC_Id BIGINT, @SPT_Id BIGINT                    
	SELECT @SponsorId=SponsorId,@VMCC_Id = EventTicketAttributeId FROM  
	CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KM_ID 
	   
	SELECT E.Id AS EventCatId, ED.Id AS EventId,ED.Name AS EventName, CONVERT(VARCHAR(17), ED.StartDateTime, 113) AS EventStartDate,
	EA.Id AS VMCC_Id, EA.LocalPrice AS PricePerTic, EA.LocalPrice As LocalPricePerTic, V.Name +', '+ C.Name AS VenueAddress, TC.Name AS VenueCatName, TC.Name AS VenueDisplayName,
	CM.Code AS LocalCurrencyName, CM.Id AS Currencyid, CM.Code AS CurrencyName, ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,1),0) AS ConvenceCharge,
	ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,2),0) AS TicketServiceTax
	FROM  EventTicketAttributes EA
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.LocalCurrencyId=CM.Id AND EA.Id = @VMCC_Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetUserMenu]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetUserMenu] --4 
(      
  @UserId INT      
)      
AS      
BEGIN
  SELECT @UserId AS UserId, Id AS MenuId, FeatureName AS MenuName, 0 AS DisplayOrder, RedirectUrl AS Url, ParentFeatureId AS ParentId
  FROM Features WHERE ParentFeatureId=0 AND IsEnabled =1
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_GetVenuesByUser2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetVenuesByUser2017] --3
(                    
   @UserId BIGINT                    
)                                                
AS                                                    
BEGIN                                  
	  SELECT DISTINCT UVM.VenueId,E.Id AS EventCatId, V.Name +', '+ C.Name +' - '+ E.Name AS VenueAddress
	FROM UserVenueMappings UVM WITH(NOLOCK)
	INNER JOIN Venues V WITH(NOLOCK) ON UVM.VenueId = V.Id AND UVM.UserId = @UserId
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) ON V.Id=ED.VenueId
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId=E.Id	  
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_InsertHandOverSheetTransDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_InsertHandOverSheetTransDetails]  
(  
   @TransId BIGINT,  
   @QuantityPrinted INT,  
   @SerialFrom VARCHAR(50),  
   @SerialTo VARCHAR(50),  
   @HandedBy VARCHAR(500),  
   @SubmittedTo VARCHAR(500),  
   @IsPartialPrinting INT,
   @CreatedBy VARCHAR(500)
)  
AS   
BEGIN  
	DECLARE @AltId VARCHAR(500)
	SELECT @AltId = AltId FROM Users WHERE UserName = @CreatedBy
	INSERT INTO HandoverSheets(TransactionId, SerialStart, SerialEnd, TicketHandedBy, TicketHandedTo, IsEnabled, CreatedUtc, CreatedBy)
	VALUES(@TransId,@SerialFrom,@SerialTo,@HandedBy,@SubmittedTo,1,GETDATE(),@AltId)
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_InsertPaidTransPaymentLog]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_InsertPaidTransPaymentLog]
(
	@EventTransId BIGINT,
	@PaymentType VARCHAR(100),
	@PaymentReceivedIn VARCHAR(500),
	@PaymentConfirmedBy VARCHAR(100),
	@PaymentApprovedDate VARCHAR(500),
	@ChequeNumber VARCHAR(100),
	@DrawnonBank VARCHAR(100),
	@ChequeDate VARCHAR(100),
	@ApprovalEmailFrom VARCHAR(2000),
	@ApprovalEmailReceivedOn VARCHAR(100),
	@ReferenceNumber VARCHAR(100)
)
AS
BEGIN
	DECLARE @PaymentTypeId INT, @AltId NVARCHAR(500), @TransactingOptionId INT
	SET @PaymentTypeId = CASE WHEN @PaymentType ='Bank Transfer' THEN 1
	WHEN @PaymentType ='Cash' THEN 2
	WHEN @PaymentType ='CC/DD' THEN 3
	WHEN @PaymentType ='Cheque' THEN 4
	WHEN @PaymentType ='Credit Card' THEN 5
	WHEN @PaymentType ='Sale on Credit' THEN 6
	WHEN @PaymentType ='Payment with Client' THEN 7
	WHEN @PaymentType ='Retail - Printed to be sold' THEN 8
	ELSE 0 END
	IF(@PaymentTypeId=0)
	BEGIN
		SELECT @PaymentTypeId = Id FROm PaymentTypes WHERE PaymentType = @PaymentType
	END


	SELECT  @AltId = CreatedBy FROM Transactions WITH(NOLOCK) WHERE Id =@EventTransId
	SELECT  @TransactingOptionId = TransactingOptionId FROM CorporateTransactionDetails WITH(NOLOCK) WHERE TransactionId =@EventTransId

	INSERT INTO CorporateTransactionPaymentDetails(AltId, TransactionId,TransactingOptionId,PaymentTypeId,PaymentReceivedIn,PaymentConfirmedBy,PaymentApprovedUtc,
	ChequeNumber,ChequeDrawnonBank, ChequeUtc,ApprovalEmailFrom,ApprovalEmailReceivedOn,BankReferenceNumber,IsEnabled,CreatedUtc,CreatedBy)
	VALUES(NEWID(), @EventTransId,@TransactingOptionId,@PaymentTypeId,@PaymentReceivedIn,@PaymentConfirmedBy, CONVERT(DATETIME,@PaymentApprovedDate), 
	@ChequeNumber,@DrawnonBank,CONVERT(DATETIME,@ChequeDate),
	@ApprovalEmailFrom, CONVERT(DATETIME,@ApprovalEmailReceivedOn),@ReferenceNumber,1,GETDATE(),@AltId)
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_InsertRePrintTransLog]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_InsertRePrintTransLog]
(
	@TransId BIGINT,
	@RePrintReason VARCHAR(500),
	@OtherReason VARCHAR(500),
	@UpdatedBy VARCHAR(500)
)
AS
BEGIN

IF(@OtherReason<>'')
BEGIN
	SET @RePrintReason = @RePrintReason +'-'+@OtherReason
END

DECLARE @AltId VARCHAR(500), @USerId BIGINT
SELECT @AltId = AltId, @USerId= Id FROm Users WHERE UserName =@UpdatedBy
 INSERT INTO ReprintRequests(UserId,TransactionId,RequestDateTime,Remarks,IsApproved,ApprovedBy,ApprovedDateTime,ModuleId,
CreatedUtc,CreatedBy)
 VALUES(@USerId, @TransId, GETUTCDATE(),@RePrintReason,	0, NULL,NULL,3,GETUTCDATE(),@AltId)
 
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_InsertSponsorDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_InsertSponsorDetails]   
(  
	@SponsorName VARCHAR(500),   
	@AddressLine1 VARCHAR(100),   
	@AddressLine2 VARCHAR(100),   
	@City VARCHAR(30),   
	@State VARCHAR(30),  
	@Country VARCHAR(30),  
	@PostalCode VARCHAR(30),  
	@PCFirstName VARCHAR(30),  
	@PCLastName VARCHAR(30),  
	@PCGender VARCHAR(30),  
	@PCEmail VARCHAR(50),   
	@PCMobile VARCHAR(50),  
	@PCIDType VARCHAR(50),   
	@PCIDNo VARCHAR(50),   
	@SCFirstName VARCHAR(30),   
	@SCLastName VARCHAR(30),   
	@SCGender VARCHAR(30),   
	@SCEmail VARCHAR(50),   
	@SCMobile VARCHAR(30),    
	@SCIDType VARCHAR(50),   
	@SCIDNo VARCHAR(50),
	@Updatedby VARCHAR(200)
)  
AS   
BEGIN
DECLARE @PhoneCode VARCHAR(10), @PhoneNumber VARCHAR(20), @ZipCodeId INT, @AltId NVARCHAR(200)
SELECT CHARINDEX('-',@PCMobile)
SET @PhoneCode = SUBSTRING(@PCMobile, 0, CHARINDEX('-',@PCMobile));
SET @PhoneNumber = SUBSTRING(@PCMobile, CHARINDEX('-',@PCMobile)+1, LEN(@PCMobile));
SELECT @ZipCodeId = ID FROM Zipcodes WHERE Postalcode = @PostalCode
SELECT @AltId = Altid FROm Users WHERE UserName = @Updatedby

INSERT INTO Sponsors(SponsorName,FirstName,LastName,Email,PhoneCode,PhoneNumber,CompanyAddress,CompanyCity,CompanyState,CompanyCountry,
CompanyZipcodeId,IsEnabled,CreatedUtc,CreatedBy,IdType,IdNumber,SponsorTypeId)
VALUES(@SponsorName, @PCFirstName, @PCLastName, @PCEmail,@PhoneCode, @PhoneNumber,@AddressLine1, @City, @State, @Country,@PostalCode,
1, GETUTCDATE(), @AltId,1, @PCIDType, @PCIDNo)
 
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_InsertTransPrintTypeLog]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROC [dbo].[KITMS_InsertTransPrintTypeLog]
(
  @TransId BIGINT,
  @PrintType VARCHAR(100),
  @IsPartialPrinting INT,
  @IsSponsorName INT,
  @UpdatedBy VARCHAR(100)
)
AS   
BEGIN  
	DECLARE @AltId VARCHAR(500), @TicketPrintingOptionId SMALLINT
	SELECT @AltId = AltId FROM Users WHERE UserName = @UpdatedBy
	SET @TicketPrintingOptionId  = CASE WHEN @PrintType ='Zero Price' THEN 5
			  WHEN @PrintType ='Price' THEN 2
			  WHEN @PrintType ='Hospitality' THEN 6
			  WHEN @PrintType ='GovernmentComplimentary' THEN 7
			  WHEN @PrintType ='Complimentary + Zero Price (local)' THEN 4
			  WHEN @PrintType ='Complimentary' THEN 3
			  WHEN @PrintType ='ChildPrice' THEN 9
			  WHEN @PrintType ='ChildComplimentary' THEN 8
			  WHEN @PrintType ='Blank' THEN 1
		END

	INSERT INTO CorporateTicketPrintingLogs(TransactionId, TicketPrintingOptionId, IsEnabled, CreatedUtc, CreatedBy)
	VALUES(@TransId,@TicketPrintingOptionId,1,GETDATE(),@AltId)
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_ITKTS_BuyTicketForIPLRetailForSponsor_New]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_ITKTS_BuyTicketForIPLRetailForSponsor_New] --20009, '', NULL, ''
(                                                                    
 @TransId bigint,                                                                              
 @PayConfNumber varchar(1000)='samba',                                                                              
 @ErrMsg varchar(500) = null ,                                      
 @Flag varchar(50) = 'Default'                                                                             
)                                                                    
AS
BEGIN
DECLARE @ReturnVal INT, @AltId NVARCHAR(500)
SELECT  @AltId = CreatedBy FROM Transactions WITH(NOLOCK) WHERE Id =@TransId
UPDATE Transactions SET ChannelId = 8, TransactionStatusId =8, UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id= @TransId
UPDATE CorporateTransactionDetails SET IsEnabled =1, UpdatedBy = @AltId, UpdatedUtc = GETDATE()  WHERE TransactionId = @TransId
SET @ReturnVal = 1
SELECT   @ReturnVal                     
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_ReleaseSeats]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_ReleaseSeats]          
(          
	@Seats VARCHAR(MAX),          
	@EventId BIGINT,    
	@SponsorId INT        
)          
AS           
BEGIN 
	UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1
	AND SponsorId = @SponsorId
	UPDATE MatchLayoutSectionSeats SET SeatTypeId= 1 WHERE Id IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatTypeId = 3  
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_ReleaseSponsorCategoryTickets]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_ReleaseSponsorCategoryTickets]-- 1, 11909, 31364,5,'KyazoongaTom'  
(                            
  @KITMSMasterId BIGINT,                            
  @EventID BIGINT,                            
  @VMCCId BIGINT,                             
  @TicketQty INT,                            
  @UpdateBY VARCHAR(500)                             
)                            
AS                            
BEGIN      
    
BEGIN TRANSACTION trans    
BEGIN TRY   
  
DECLARE @TempTable TABLE(BlockTic INT, AvailableTic INT, Status INT)
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @SponsorID BIGINT, @TicketForSale BIGINT, @BlockSeatCount INT,
@EventTicketDetailId BIGINT, @AltId VARCHAR(500), @Price DECIMAL(18,2)
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY
    
SELECT @TicketForSale = AvailableTicketForSale,@EventTicketDetailId = EventTicketDetailId FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VMCCId    
IF EXISTS(SELECT * FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId AND EventTicketAttributeId = @VMCCId)  
BEGIN  
    
 SELECT @SponsorID=SponsorId,@BlockTic = AllocatedTickets ,@AvailableTic = RemainingTickets, @Price =Price FROM  
 CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId                       
     
 IF(@AvailableTic+1>@TicketQty)                
 BEGIN   
   UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets - @TicketQty, 
   RemainingTickets = RemainingTickets -@TicketQty  WHERE Id = @KITMSMasterId                
   UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale + @TicketQty WHERE Id=@VMCCId

    DECLARE @tblBlockedSeatIds TABLE(MatchLayoutSectionSeatId BIGINT)
    INSERT INTO @tblBlockedSeatIds
    SELECT MatchLayoutSectionSeatId FROM MatchSeatTicketDetails A WITH(NOLOCK)
	INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
	WHERE A.SponsorId = @SponsorId AND A.EventTicketDetailId = @EventTicketDetailId AND B.SeatTypeId =3
	AND A.SeatStatusId=1

	SELECT @BlockSeatCount = ISNULL(COUNT(*),0) FROM @tblBlockedSeatIds
	IF(@BlockSeatCount > 0)
	BEGIN
		UPDATE MatchSeatTicketDetails SET SponsorId= NULL WHERE MatchLayoutSectionSeatId IN (SELECT MatchLayoutSectionSeatId FROM @tblBlockedSeatIds)
		UPDATE MatchLayoutSectionSeats SET SeatTypeId= 1 WHERE Id IN (SELECT MatchLayoutSectionSeatId FROM @tblBlockedSeatIds)
	END
	INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)
	VALUES(@KITMSMasterId,2,@TicketQty,@Price,1,GETDATE(),@AltId);
   
   INSERT INTO @TempTable VALUES(@TicketQty, @TicketForSale + @TicketQty, 1)  
   SELECT * FROM @TempTable  

 END     
 ELSE                
 BEGIN                
		INSERT INTO @TempTable VALUES(@TicketQty, @AvailableTic, 0)  
		SELECT * FROM @TempTable    
 END        
END                 
       
COMMIT TRANSACTION trans    
END TRY    
BEGIN CATCH    
ROLLBACK TRANSACTION trans     
END CATCH                            
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_ReleaseTicketsForSponsorByVenue]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_ReleaseTicketsForSponsorByVenue]                                        
(                                          
 @SponsorId BIGINT,                                          
 @EventIds varchar(max),                                          
 @VenueCatId BIGINT,                                           
 @tics INT,                                          
 @UpdateBY VARCHAR(500)                                           
)                                          
AS                                          
BEGIN  
  
BEGIN TRANSACTION trans  
BEGIN TRY  
                                
declare @BlockTic BIGINT=0, @AvailableTic BIGINT=0, @KM_ID BIGINT=0, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',
@Flag VARCHAR(50) = 'Default',@AltId VARCHAR(500),@EventId BIGINT, @VmccId BIGINT, @Price DECIMAL(18,2)
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                
INSERT INTO #vmccIds
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))

DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds
OPEN  curEvent;                 
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                 
while @@FETCH_STATUS=0                  
begin       
	SET @Flag='Default' 
	SELECT @KM_ID = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId AND SponsorId=@SponsorId AND IsEnabled = 1
	SELECT @BlockTic = ISNULL(AllocatedTickets,0),@AvailableTic=ISNULL(RemainingTickets,0),@Price =Price From  CorporateTicketAllocationDetails where Id= @KM_ID
	IF(@tics < @AvailableTic+1)      
	BEGIN      
		UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets - @tics, 
		RemainingTickets = RemainingTickets -@tics  WHERE Id = @KM_ID                
		UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale + @tics WHERE Id=@VmccId          
		INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)
		VALUES(@KM_ID,2,@tics,@Price,1,GETDATE(),@AltId);
		SET @Flag='Changed'
	END      
	SELECT @EventName = ED.Name from EventDetails ED WHERE Id= @EventId
	IF(@Flag='Changed')              
	BEGIN              
		SET @ReturnVal += CONVERT(Varchar(100),@tics) + ' tickets released successfully for ' +  @EventName+ ',<br/> '               
	END   
	ELSE              
	BEGIN             
		SET @ReturnVal += 'Only '+ CONVERT(Varchar(100),@AvailableTic) + ' tickets are avaialble to release for '+ @EventName + ',<br/> '              
	END              

	FETCH NEXT FROM curEvent INTO @VmccId, @EventId   
END                   

CLOSE curEvent;                   
DEALLOCATE curEvent;      

DROP TABLE #vmccIds             
SELECT @ReturnVal                       
COMMIT TRANSACTION trans  
END TRY  
BEGIN CATCH  
ROLLBACK TRANSACTION trans   
END CATCH                           
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_SeatTransfer]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_SeatTransfer]          
(          
	@Seats VARCHAR(MAX),          
	@EventId BIGINT,    
	@TransferSponsorId INT        
)          
AS           
BEGIN
 UPDATE MatchSeatTicketDetails SET SponsorId= @TransferSponsorId WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1 
END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_TransactSponsorCategoryTickets]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[KITMS_TransactSponsorCategoryTickets]             
(                                
	@KM_Id bigint,                                                            
	@Quantity int,                                
	@TransType varchar(255),                                
	@UpdateBY varchar(255)                                
)                                
as                                
begin   
BEGIN TRANSACTION trans
BEGIN TRY

DECLARE @EventId BIGINT, @SponsorId BIGINT, @VMCC_Id BIGINT, @Flag VARCHAR(10),@ReturnVal VARCHAR(5000),@BlockTic INT, @AvailableTic INT
SELECT @SponsorID=SponsorId,@BlockTic = AllocatedTickets,@AvailableTic = RemainingTickets FROM  
CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KM_ID
SET @ReturnVal = '0'
IF(@Quantity < @AvailableTic+1)          
BEGIN
    SET @ReturnVal = '1'
END
                                      
SELECT @ReturnVal                        
     COMMIT TRANSACTION trans
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION trans 
END CATCH                         
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_TransferSponsorCategoryTickets]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_TransferSponsorCategoryTickets] --1,39806,109868,5,'vimal'                     
(                      
	 @KITMSMasterId BIGINT,                      
	 @Eventd BIGINT,                      
	 @VmccId BIGINT,                       
	 @TicketQty INT,                      
	 @UpdateBY VARCHAR(500),         
	 @TransferSponsorId BIGINT                     
)                      
AS                      
BEGIN   

BEGIN TRANSACTION trans  
BEGIN TRY             
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @SponsorID BIGINT, @TicketForSale BIGINT, @KITMSMasterId_New BIGINT, @VmccId_New BIGINT, @BlockTic_New BIGINT,
@AvailableTic_New BIGINT, @ReturnVal VARCHAR(5000), @Flag VARCHAR(50) = 'Default', @Price DECIMAL(18,2),@AltId NVARCHAR(500), @BlockSeatCount INT
DECLARE @TempTable TABLE(BlockTic INT, AvailableTic INT, Status INT)
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY

IF EXISTS(SELECT * FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId AND EventTicketAttributeId = @VMCCId)  
BEGIN        
SELECT @SponsorID=SponsorId,@BlockTic = AllocatedTickets,@AvailableTic = RemainingTickets, @Price =Price FROM  
CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId    
 IF(@TicketQty < @AvailableTic+1)          
 BEGIN  
		 UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets - @TicketQty, 
		 RemainingTickets = RemainingTickets -@TicketQty  WHERE Id = @KITMSMasterId
		 SELECT @KITMSMasterId_New=Id,@BlockTic_New = AllocatedTickets,@AvailableTic_New=RemainingTickets FROM
		 CorporateTicketAllocationDetails WHERE SponsorId = @TransferSponsorId and EventTicketAttributeId = @VMCCId
		          
		 SET @BlockTic_New = @BlockTic_New+@TicketQty        
		 SET @AvailableTic_New = @AvailableTic_New+@TicketQty        
		         
		 IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WHERE SponsorId = @TransferSponsorId and EventTicketAttributeId = @VMCCId)        
		 BEGIN
			 SELECT @KITMSMasterId_New=Id,@BlockTic_New = AllocatedTickets,@AvailableTic_New=RemainingTickets FROM
			 CorporateTicketAllocationDetails WHERE SponsorId = @TransferSponsorId and EventTicketAttributeId = @VMCCId       
			UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets + @TicketQty, 
			RemainingTickets = RemainingTickets + @TicketQty  WHERE Id = @KITMSMasterId_New         
		 END        
		 ELSE        
		 BEGIN
			INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price, 
			IsEnabled, CreatedUtc, CreatedBy)                      
			VALUES(NEWID(),@VMCCId, @TransferSponsorId,@TicketQty,@TicketQty,@Price,1,GETDATE(),@AltId)
		 END

		DECLARE @tblBlockedSeatIds TABLE(MatchLayoutSectionSeatId BIGINT)
		INSERT INTO @tblBlockedSeatIds
		SELECT MatchLayoutSectionSeatId FROM MatchSeatTicketDetails A WITH(NOLOCK)
		INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
		WHERE A.SponsorId = @SponsorId AND A.EventTicketDetailId = @VmccId AND B.SeatTypeId =3
		AND A.SeatStatusId=1

		SELECT @BlockSeatCount = ISNULL(COUNT(*),0) FROM @tblBlockedSeatIds
		IF(@BlockSeatCount > 0)
		BEGIN
			UPDATE MatchSeatTicketDetails SET SponsorId= @TransferSponsorId WHERE MatchLayoutSectionSeatId IN (SELECT TOP (@TicketQty) MatchLayoutSectionSeatId 
			FROM @tblBlockedSeatIds)
		END

		 INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, TransferToCorporateTicketAllocationDetailId,AllocationOptionId, TotalTickets, 
		 Price, IsEnabled, CreatedUtc, CreatedBy)
		 VALUES(@KITMSMasterId,@KITMSMasterId_New,3,@TicketQty,@Price,1,GETDATE(),@AltId);

		 INSERT INTO @TempTable VALUES(@TicketQty, @AvailableTic, 1)
		 SELECT * FROM @TempTable
           
END   
ELSE              
BEGIN              
	  INSERT INTO @TempTable VALUES(@TicketQty, @AvailableTic, 0)
	  SELECT * FROM @TempTable  
END         
END 
COMMIT TRANSACTION trans  
END TRY  
BEGIN CATCH  
ROLLBACK TRANSACTION trans   
END CATCH                
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_UpdateSponsorDetails]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_UpdateSponsorDetails]   
(
	@SponsorId INT,  
	@SponsorName VARCHAR(500),   
	@AddressLine1 VARCHAR(100),   
	@AddressLine2 VARCHAR(100),   
	@City VARCHAR(30),   
	@State VARCHAR(30),  
	@Country VARCHAR(30),  
	@PostalCode VARCHAR(30),  
	@PCFirstName VARCHAR(30),  
	@PCLastName VARCHAR(30),  
	@PCGender VARCHAR(30),  
	@PCEmail VARCHAR(50),   
	@PCMobile VARCHAR(50),  
	@PCIDType VARCHAR(50),   
	@PCIDNo VARCHAR(50),   
	@SCFirstName VARCHAR(30),   
	@SCLastName VARCHAR(30),   
	@SCGender VARCHAR(30),   
	@SCEmail VARCHAR(50),   
	@SCMobile VARCHAR(30),    
	@SCIDType VARCHAR(50),   
	@SCIDNo VARCHAR(50),
	@Updatedby VARCHAR(200)
)  
AS   
BEGIN  
DECLARE @PhoneCode VARCHAR(10), @PhoneNumber VARCHAR(20), @ZipCodeId INT, @AltId NVARCHAR(200)
SELECT CHARINDEX('-',@PCMobile)
SET @PhoneCode = SUBSTRING(@PCMobile, 0, CHARINDEX('-',@PCMobile));
SET @PhoneNumber = SUBSTRING(@PCMobile, CHARINDEX('-',@PCMobile)+1, LEN(@PCMobile));
SELECT @ZipCodeId = ID FROM Zipcodes WHERE Postalcode = @PostalCode
SELECT @AltId = Altid FROm Users WHERE UserName = @Updatedby

UPDATE Sponsors SET SponsorName = @SponsorName,FirstName = @PCFirstName,LastName=@PCLastName,Email=@PCEmail,PhoneCode=@PhoneCode,
PhoneNumber =@PhoneNumber ,CompanyAddress = @AddressLine1,CompanyCity = @City,
CompanyState = @State,CompanyCountry = @Country,CompanyZipcodeId = @PostalCode,
IsEnabled=1,UpdatedUtc=GETDATE(),UpdatedBy = @AltId,IdType = @PCIDType, IdNumber = @PCIDNo
WHERE Id = @SponsorId

END
GO
/****** Object:  StoredProcedure [dbo].[KITMS_UpdateTransactionStausByVenue]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_UpdateTransactionStausByVenue]                                                        
(                                                                    
	 @TransId bigint,                                                                              
	 @PayConfNumber varchar(1000)='Venue Level',                                                                              
	 @ErrMsg varchar(500) = null ,                                      
	 @Flag varchar(50) = 'Default'                                                                             
)                                                                    
AS                                                                              
BEGIN                                                                     
	DECLARE @ReturnVal INT, @AltId NVARCHAR(500)
	SELECT  @AltId = CreatedBy FROM Transactions WITH(NOLOCK) WHERE Id =@TransId
	UPDATE Transactions SET ChannelId = 8, TransactionStatusId =8, UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id= @TransId
	UPDATE CorporateTransactionDetails SET IsEnabled =1, UpdatedBy = @AltId, UpdatedUtc = GETDATE()  WHERE TransactionId = @TransId
	SET @ReturnVal = 1
	SELECT   @ReturnVal                    
END 
GO
/****** Object:  StoredProcedure [dbo].[KITMS_ValidateSignInUser]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_ValidateSignInUser]    
(    
	@UserEmail nvarchar(250),    
	@Password nvarchar(250)    
)    
AS    
BEGIN    
 IF EXISTS(SELECT * FROM Users WITH(NOLOCK) WHERE UserName=@UserEmail AND Password = @Password AND IsEnabled = 1)      
 BEGIN    
   SELECT Id AS KT_UserId,FirstName AS UserFName,LastName AS UserLName, PhoneCode +'-'+ PhoneNumber AS Usermobile,
   UserName AS UserEmail,Password,IsEnabled AS Status,'KyazoongaUser' AS RoleId
   FROM Users WITH(NOLOCK)  WHERE UserName=@UserEmail AND Password = @Password AND IsEnabled = 1  
 END    
END 
GO
/****** Object:  StoredProcedure [dbo].[Web_Countries]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Web_Countries] --2238
AS
BEGIN
	SELECT * FROm Countries ORDER By Name ASC
END
GO
