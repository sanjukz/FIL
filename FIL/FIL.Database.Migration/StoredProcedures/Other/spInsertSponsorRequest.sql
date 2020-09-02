CREATE  PROC [dbo].[spInsertSponsorRequest]-- @retValue=1  
(  
--DECLARE  
	@EventCatId BIGINT=2047,  
	@EventId NVARCHAR(MAX)='118501',  
	@CorporateName NVARCHAR(200)='',  
	@EmailId NVARCHAR(500)='',  
	@MobileNo NVARCHAR(50)='', 
	@PhoneCode NVARCHAR(50)='',
	@RepresentativeFirstName NVARCHAR(200)='',  
	@RepresentativeLastName NVARCHAR(200)='',  
	@Address NVARCHAR(500)='',  
	@City NVARCHAR(500)='',  
	@State NVARCHAR(500)='',  
	@CountryId INT=0,  
	@CountryName NVARCHAR(500)='',  
	@ZipCode NVARCHAR(500)='',  
	@VMCCIds NVARCHAR(MAX)='436547,436548',  
	@TicNumIds NVARCHAR(MAX)='7305710,7305711,7305712,7305310,7305311',  
	@Quantity NVARCHAR(MAX)='3,2',  
	@sentOTP NVARCHAR(10)='',  
	@IsCompTicket INT=0,  
	@PickupRepresentativeFirstName NVARCHAR(200)='',  
	@PickupRepresentativeLastName NVARCHAR(200)='',
	@PickUpRepEmailId NVARCHAR(500)='',  
	@PickUpRepMobileNo NVARCHAR(50)='',
	@PickUpRepPhoneCode NVARCHAR(50)='',
	@Classification NVARCHAR(100)='',  
	@UserIpAddress NVARCHAR(100)='',  
	@LoginUserMail NVARCHAR(200)='',  
	@TicketTypeIds NVARCHAR(MAX)='',
	@Prices NVARCHAR(MAX)='',
	@retValue int OUTPUT  
)  
AS  
BEGIN  
  
DECLARE @VMCCTable TABLE (RowNo INT IDENTITY(1,1), VMCC_Id BIGINT,Event_Id BIGINT)
DECLARE @EventIdsTable TABLE (RowNo INT IDENTITY(1,1), Event_Id BIGINT)  
DECLARE @QuantityTable TABLE (RowNo INT IDENTITY(1,1), TicQuantity int)  
DECLARE @TicNumDetailTable TABLE (RowNo INT IDENTITY(1,1), TicNumId int)  
DECLARE @TicketTypeIdTable TABLE (RowNo INT IDENTITY(1,1), TicketTypeId int)  
DECLARE @PriceTable TABLE (RowNo INT IDENTITY(1,1), Price DECIMAL(18,2))  
   
 INSERT INTO @VMCCTable(VMCC_Id)    
 SELECT * FROM dbo.SplitString(@VMCCIds, ',')  
   
 INSERT INTO @EventIdsTable(Event_Id)    
 SELECT * FROM dbo.SplitString(@EventId, ',')   
   
 INSERT INTO @QuantityTable(TicQuantity)    
 SELECT * FROM dbo.SplitString(@Quantity, ',')   
  
 INSERT INTO @TicNumDetailTable(TicNumId)    
 SELECT * FROM dbo.SplitString(@TicNumIds, ',')   

INSERT INTO @TicketTypeIdTable(TicketTypeId)    
SELECT * FROM dbo.SplitString(@TicketTypeIds, ',') 

INSERT INTO @PriceTable(Price)    
SELECT * FROM dbo.SplitString(@Prices, ',') 
    
 DECLARE @Counter INT = 1, @RowCount INT, @VMCC BIGINT, @VMCCEventId BIGINT, @TicCOunt INT , @Price DECIMAL(18,2) 
 DECLARE @TotalCount INT, @TicNumIdForVMCC NVARCHAR(MAX)='', @RowTicNumId INT  
 
 SELECT @RowCount = COUNT(*) FROM @VMCCTable  
 DECLARE @CorporateId BIGINT, @AltId NVARCHAR(200), @ZipCodeId INT
  SELECT @AltId = Altid FROm Users WHERE UserName = @LoginUserMail

 INSERT INTO CorporateRequests(SponsorName,FirstName,LastName,Email,PhoneCode,PhoneNumber,Address,City,State,Country,ZipCode,
 PickupRepresentativeFirstName, PickupRepresentativeLastName,PickupRepresentativeEmail,PickupRepresentativePhoneCode,
 PickupRepresentativePhoneNumber,SponsorId, RequestOrderType,IsEnabled,CreatedUtc,CreatedBy,Classification)
 VALUES(RTRIM(LTRIM(@CorporateName)),@RepresentativeFirstName,@RepresentativeLastName, @EmailId,@PhoneCode,@MobileNo,@Address,@City,@State,@CountryName,
 @ZipCode,@PickupRepresentativeFirstName, @PickupRepresentativeLastName,@PickUpRepEmailId, @PickUpRepPhoneCode,@PickUpRepMobileNo,
 NULL,@IsCompTicket,1,GETUTCDATE(),@AltId,@Classification)
 SET @CorporateId = SCOPE_IDENTITY()

 SET @TotalCount=0  
 DECLARE @SeatForVMCCCount INT=1  
 DECLARE @TotalSeatCount INT=0  
 SELECT @TotalSeatCount=COUNT(*) FROM @TicNumDetailTable   
  
 DECLARE @TicNumVMCC BIGINT,@NewCorpOrderMapId BIGINT  
  
 DECLARE @SponsorName NVARCHAR(400)=RTRIM(LTRIM(@CorporateName))+' - '+@RepresentativeFirstName +' '+  @RepresentativeLastName
 DECLARE @VMCCAvailCount INT, @SponsorId BIGINT, @KM_Id BIGINT,  @TicketTypeId INT  
  
 WHILE (@Counter <= @RowCount)  
 BEGIN  
  SELECT @VMCC = VMCC_Id FROM @VMCCTable WHERE RowNo = @Counter
  SELECT @Price = Price FROM @PriceTable WHERE RowNo = @Counter  
  SELECT @TicketTypeId = TicketTypeId FROM @TicketTypeIdTable WHERE RowNo = @Counter    
  --SELECT @VMCCEventId=Event_Id FROM @EventIdsTable WHERE RowNo = @Counter  
  SELECT @VMCCEventId=EventDetailId FROM EventTicketAttributes A WITH(NOLOCK) 
  INNER JOIN EventTicketDetails B WITH(NOLOCK) On A.EventTicketDetailId =B.Id
  WHERE A.Id = @VMCC
  SELECT @TicCOunt=TicQuantity FROM @QuantityTable WHERE RowNo = @Counter  
    
  IF(@VMCC>0)  
  BEGIN  
   SET @SeatForVMCCCount=1  
   WHILE(@SeatForVMCCCount<=@TotalSeatCount)  
   BEGIN  
    SELECT @RowTicNumId=TicNumId FROM @TicNumDetailTable WHERE RowNo = @SeatForVMCCCount
	SELECT @TicNumVMCC=A.Id FROM EventTicketAttributes A WITH(NOLOCK) 
	INNER JOIN EventTicketDetails B WITH(NOLOCK) On A.EventTicketDetailId =B.Id
	INNER JOIN MatchSeatTicketDetails C WITH(NOLOCK) On B.Id =C.EventTicketDetailId
    WHERE C.Id = @RowTicNumId

    --print @RowTicNumId  
    IF (@TicNumVMCC=@VMCC)  
    BEGIN  
     SET @TicNumIdForVMCC=@TicNumIdForVMCC+','+CONVERT(NVARCHAR(10),@RowTicNumId)  
    END  
    SET @SeatForVMCCCount=@SeatForVMCCCount+1  
   END
        
   INSERT INTO CorporateRequestDetails(CorporateRequestId,RequestedTickets,ApprovedTickets,Price,ApprovedStatus,
   IsEnabled,CreatedUtc,CreatedBy,EventTicketAttributeId,MatchSeatTicketDetailIds, TicketTypeId)
   VALUES(@CorporateId,@TicCOunt,0,@Price,0,1,GETUTCDATE(), @AltId,@VMCC,@TicNumIdForVMCC, @TicketTypeId)
  
   SET @NewCorpOrderMapId=SCOPE_IDENTITY()  
     
   -------------Blocking Start--------------------------------------------------------------  
 --  --EXEC KITMS_BlockTicketForCorporate(@NewCorpOrderMapId,@TicCOunt,-1)  
 --  IF(@TicNumIdForVMCC<>'')  
 --  BEGIN  
 --   BEGIN TRANSACTION trans    
 --   BEGIN TRY   
    
	--SELECT @VMCCAvailCount=RemainingTicketForSale FROm EventTicketAttributes WHERE Id = @VMCC  
 --   IF(@VMCCAvailCount>=@TicCOunt)  
 --   BEGIN  
 --    IF EXISTS (SELECT * FROM Sponsors WHERE IsEnabled=1 AND UPPER(RTRIM(LTRIM(SponsorName)))=UPPER(RTRIM(LTRIM(@SponsorName))))  
 --    BEGIN  
 --     SELECT @SponsorId=Id FROM Sponsors WHERE IsEnabled=1 AND UPPER(RTRIM(LTRIM(SponsorName)))=UPPER(RTRIM(LTRIM(@SponsorName)))  
 --     UPDATE CorporateRequests SET SponsorId=@SponsorId,UpdatedBy=@AltId,UpdatedUtc=GETUTCDATE() WHERE Id=@CorporateId   
 --    END  
 --    ELSE   
 --    BEGIN  
 --      print('insert into sponsor')  
 --     INSERT INTO Sponsors(SponsorName,FirstName,LastName,Email,PhoneCode,PhoneNumber,CompanyAddress, CompanyCity,CompanyState,CompanyCountry,
	--  CompanyZipcodeId,	  IsEnabled,CreatedUtc,CreatedBy,SponsorTypeId,IdType,IdNumber)
	--  SELECT SponsorName, FirstName, LastName,LOWER(Email),PhoneCode, PhoneNumber,  Address, City,State,Country,
	--  ZipCode, 1,GETUTCDATE(), @AltId,1,'',''  FROM CorporateRequests WHERE Id=@CorporateId
 --      SET @SponsorId=SCOPE_IDENTITY()  
 --      UPDATE CorporateRequests SET SponsorId=@SponsorId,UpdatedBy=@AltId,UpdatedUtc=GETUTCDATE() WHERE Id=@CorporateId  
 --    END  
  
 --    IF NOT EXISTS(SELECT Id FROM EventSponsorMappings WHERE EventDetailId=@VMCCEventId AND SponsorId=@SponsorId)  
 --    BEGIN  
 --     INSERT INTO EventSponsorMappings(SponsorId,EventDetailId,IsEnabled,CreatedBy,CreatedUtc)  
 --     VALUES(@VMCCEventId,@SponsorId,1,@AltId,GETUTCDATE())  
 --    END  
  
 --    IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WHERE SponsorId=@SponsorId and EventTicketAttributeId=@VMCC)  
 --    BEGIN 
 --     SELECT @KM_Id=Id FROM CorporateTicketAllocationDetails WHERE SponsorId=@SponsorId and EventTicketAttributeId=@VMCC
 --     UPDATE CorporateTicketAllocationDetails SET AllocatedTickets+=@TicCOunt,RemainingTickets+=@TicCOunt WHERE Id=@KM_Id  
 --    END  
 --    ELSE  
 --    BEGIN
	--  SELECT @Price=A.LocalPrice FROM EventTicketAttributes A WHERE A.Id = @VMCC
	--  INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId,AllocatedTickets,RemainingTickets,
	--  Price,IsCorporateRequest,CorporateRequestId,IsEnabled,CreatedUtc,CreatedBy)
	--  VALUES(NEWID(),@VMCC, @SponsorId,@TicCOunt,@TicCOunt, @Price,1,@CorporateId,1, GETUTCDATE(),@AltId)
 --     SET @KM_Id=SCOPE_IDENTITY()  
 --    END  
  
 --    UPDATE EventTicketAttributes SET RemainingTicketForSale-=@TicCOunt WHERE Id=@VMCC  
 
 --    IF(ISNULL(@TicNumIdForVMCC,'')<>'')  
 --    BEGIN  
 --     UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE Id IN (SELECT * FROM dbo.SplitString(@TicNumIdForVMCC,','))
	--  UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN(SELECT MatchLayoutSectionSeatId FROM MatchSeatTicketDetails WHERE
	--  Id IN (SELECT * FROM dbo.SplitString(@TicNumIdForVMCC,',')))     
 --  END  
	-- INSERT INTO CorporateTicketAllocationDetailLogs (CorporateTicketAllocationDetailId,AllocationOptionId,TotalTickets,Price,
	-- IsEnabled,CreatedUtc,CreatedBy,TransferToCorporateTicketAllocationDetailId)   
	-- VALUES(@KM_Id,1,@TicCOunt,@Price,1,GETUTCDATE(),@AltId,NULL)  
     
 --    --@ReturnVal msg format used in code(Success! ), so don't change it.  
 --    --SET @ReturnVal = 'Success! ' + CONVERT(Varchar(100),@TicCOunt) + ' tickets blocked successfully.'      
     
 --    UPDATE CorporateRequestDetails  
 --    SET IsEnabled=1, ApprovedStatus=1,ApprovedBy=@AltId,ApprovedTickets=@TicCOunt,ApprovedUtc=GETUTCDATE()   
 --    WHERE Id=@NewCorpOrderMapId  
 --   END  
  
 --   COMMIT TRANSACTION trans    
 --   END TRY    
 --   BEGIN CATCH    
 --   ROLLBACK TRANSACTION trans     
 --   END CATCH  
 --  END  
   ---------------------Blocking End--------------------------------------------------------  
   SET @TicNumIdForVMCC=''  
   SET @TotalCount=@TotalCount+@TicCOunt  
  END  
  SET @Counter = @Counter + 1  
 END
 SET @retValue=@CorporateId  
 RETURN @retValue  
END 

