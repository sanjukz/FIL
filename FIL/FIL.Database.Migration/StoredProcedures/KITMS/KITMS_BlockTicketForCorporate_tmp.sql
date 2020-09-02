CREATE PROC [dbo].[KITMS_BlockTicketForCorporate_tmp]     
(    
	 @CorpMapId BIGINT,    
	 @OrderQty INT,    
	 @ApprovedBy INT=13,    
	 @SeatIds NVARCHAR(MAX)=''    
)    
AS    
BEGIN    
    
BEGIN TRANSACTION trans      
BEGIN TRY     
    
DECLARE @CorpOrderId bigint , @EventId bigint, @VMCC_Id bigint, @AvailQty bigint, @SponsorName varchar(200)    
DECLARE @SponsorId int=0, @KM_Id bigint, @ReturnVal VARCHAR(200), @TicNumIds NVARCHAR(900), @IsBlockByOnlineUser INT=0    
DECLARE @CreatedBy VARCHAR(500),@Price DECIMAL(18,2), @AltId VARCHAR(500)     
SELECT @CreatedBy = UserName,@AltId = AltId FROM Users WITH(NOLOCK) WHERE Id =@ApprovedBy  
 IF(ISNULL(@SeatIds,'')<>'')    
 BEGIN     
 --Coming From Seat Selection Start    
 UPDATE CorporateRequestDetails SET MatchSeatTicketDetailIds=@SeatIds WHERE Id=@CorpMapId    
 --Coming From Seat Selection END  
 END  
  
 SELECT @EventId=ETD.EventdetailId, @VMCC_Id=CRD.EventTicketAttributeId, @AvailQty=ETA.RemainingTicketForSale,   
 @SponsorName=SponsorName+' - '+ FirstName+' '+ LastName,   
 @CorpOrderId=CR.Id,@TicNumIds=MatchSeatTicketDetailIds    
FROM CorporateRequests CR  WITH(NOLOCK)  
INNER JOIN  CorporateRequestDetails CRD WITH(NOLOCK)  ON CRD.CorporateRequestid = CR.Id  
INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON CRD.EventTicketAttributeId = ETA.Id  
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id  
WHERE CRD.Id = @CorpMapId  
    
 IF(ISNULL(@TicNumIds,'')<>'')    
 BEGIN    
 IF EXISTS(SELECT A.Id FROM MatchSeatticketDetails A WITH(NOLOCK)   
 INNER JOIN TransactionSeatDetails B WITH(NOLOCK) ON A.Id =B.MatchSeatTicketDetailId AND B.CreatedUtc >=DATEADD(mi, 15,   GETUTCDATE())  
 AND B.MatchSeatTicketDetailId IN (SELECT * FROM dbo.SplitString(@TicNumIds,','))  
 WHERE A.Id IN (SELECT * FROM dbo.SplitString(@TicNumIds,','))   
 AND A.SeatStatusId =1)    
  BEGIN    
    SET @IsBlockByOnlineUser=1    
    SET @ReturnVal = 'The seats you are trying to block are currently blocked by online user. please try again later to avoid duplicate seats assignment!';    
  END    
 END    
 IF (@IsBlockByOnlineUser=0)    
 BEGIN    
 IF(@AvailQty>=@OrderQty)   
 BEGIN   
  IF EXISTS (SELECT Id FROM Sponsors WITH(NOLOCK) WHERE IsEnabled=1 AND UPPER(RTRIM(LTRIM(SponsorName)))=UPPER(RTRIM(LTRIM(@SponsorName))))    
  BEGIN    
    SELECT @SponsorId=Id FROM Sponsors WITH(NOLOCK) WHERE IsEnabled=1 AND UPPER(RTRIM(LTRIM(SponsorName)))=UPPER(RTRIM(LTRIM(@SponsorName)))    
    UPDATE CorporateRequests SET SponsorId=@SponsorId where Id=@CorpOrderId     
  END    
  ELSE     
  BEGIN    
    INSERT INTO Sponsors(SponsorName,FirstName,LastName,Email,PhoneCode,PhoneNumber,CompanyAddress,CompanyCity,CompanyState,CompanyCountry,  
 CompanyZipcodeId,IsEnabled,CreatedUtc,CreatedBy,IdType,IdNumber,SponsorTypeId)   
    SELECT @SponsorName,FirstName,LastName,Email,PhoneCode,PhoneNumber,Address,City,State,Country,ZipCode,1, GETUTCDATE(),  
 @AltId, NULL,NULL,1 FROM CorporateRequests WITH(NOLOCK) WHERE Id=@CorpOrderId    
        
    SET @SponsorId=SCOPE_IDENTITY()    
    UPDATE CorporateRequests SET SponsorId=@SponsorId WHERE Id=@CorpOrderId    
  END    
  
  EXEC KITMS_AddMatchSponsor @EventId, @SponsorId, @CreatedBy  
  IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId=@VMCC_Id  
  AND IsEnabled = 1 AND SponsorId = @SponsorId)   
  BEGIN  
  SELECT @KM_Id=Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId=@VMCC_Id  
  AND IsEnabled = 1 AND SponsorId = @SponsorId  
  UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets+@OrderQty,   
  RemainingTickets = RemainingTickets+@OrderQty,  
  UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id = @KM_Id  
  END  
  ELSE  
  BEGIN  
 SELECT @Price = LocalPrice FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VMCC_Id    
 INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price,   
 IsEnabled, CreatedUtc, CreatedBy)                        
 VALUES(NEWID(),@VMCC_Id, @SponsorId,@OrderQty,@OrderQty,@Price,1,GETUTCDATE(),@AltId)  
 SET @KM_Id=SCOPE_IDENTITY()  
  END  
  UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @OrderQty WHERE Id=@VMCC_Id  
  
   ----------------------------------For Sponsor Seat Block Start-------------------------    
   IF(ISNULL(@TicNumIds,'')<>'')    
   BEGIN   
 UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE MatchLayoutSectionSeatId IN   
 (SELECT * FROM dbo.SplitString(@TicNumIds,',')) AND SeatStatusId=1  
  
 UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN (SELECT MatchLayoutSectionSeatId FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE  
 Id IN(SELECT * FROM dbo.SplitString(@TicNumIds,',')) AND SeatTypeId = 1)  
   END    
   ----------------------------------For Sponsor Seat Block END---------------------------    
    SELECT @Price = LocalPrice FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VMCC_Id 
   INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price,   
   IsEnabled, CreatedUtc, CreatedBy)  
   VALUES(@KM_Id,1,@OrderQty,@Price,1,GETUTCDATE(),@AltId);  
    
   --@ReturnVal msg format used in code(Success! ), so don't change it.    
   IF(ISNULL(@TicNumIds,'')<>'')    
   BEGIN    
    SET @ReturnVal = 'Success! ' + CONVERT(VARCHAR(100),@OrderQty) + ' seats blocked successfully.'        
   END    
   ELSE    
   BEGIN    
    SET @ReturnVal = 'Success! ' + CONVERT(VARCHAR(100),@OrderQty) + ' tickets blocked successfully.'        
   END    
   UPDATE CorporateTicketAllocationDetails SET CorporateRequestId =  @CorpOrderId, IsCorporateRequest =1 WHERE Id = @KM_Id  
   UPDATE CorporateRequestDetails    
   SET ApprovedStatus =1,ApprovedBy=@AltId,ApprovedTickets=@OrderQty,ApprovedUtc=GETUTCDATE() WHERE Id=@CorpMapId    
 END    
 ELSE    
 BEGIN    
  print ('not avail')    
  --@ReturnVal msg format used in code(Only  ), so don't change it.    
  --set @ReturnVal = 'Only '+ CONVERT(Varchar(100),@AvailQty) + ' Tickets Available for Blocking. Do you want to block ' + CONVERT(Varchar(100),@AvailQty) + '?'    
  SET @ReturnVal = 'Only '+ CONVERT(Varchar(100),@AvailQty) + ' tickets available for blocking!'    
 END    
 END    
    
 SELECT @ReturnVal    
    
    
COMMIT TRANSACTION trans      
END TRY      
BEGIN CATCH      
ROLLBACK TRANSACTION trans       
END CATCH    
    
END  
