CREATE PROC [dbo].[KITMS_TransferTicketForCorporate_tmp]   
(  
	@CorpMapId BIGINT,  
	@OrderQty INT,  
	@FromKM_Id BIGINT,  
	@SeatIds NVARCHAR(MAX),  
	@ApprovedBy INT  
)  
As  
BEGIN  
  
BEGIN TRANSACTION trans    
BEGIN TRY      
  
DECLARE @CorpOrderId BIGINT  
DECLARE @EventId BIGINT  
DECLARE @VMCC_Id BIGINT  
DECLARE @AvailQty BIGINT  
DECLARE @SponsorName NVARCHAR(200)  
DECLARE @SponsorId INT=0  
DECLARE @OldSponsorID INT=0  
DECLARE @KM_Id BIGINT  
DECLARE @ReturnVal NVARCHAR(200)  
DECLARE @TicNumIds NVARCHAR(MAX)    
DECLARE @IsSeatAvail INT  
DECLARE @IsSeatsOk INT=1, @EventTicketDetailId BIGINT, @CreatedBy NVARCHAR(200), @AltId NVARCHAR(200),@Price DECIMAL(18,2)
SELECT @CreatedBy = UserName,@AltId = AltId FROM Users WITH(NOLOCK) WHERE Id =@ApprovedBy
  
SET @TicNumIds=@SeatIds  
  
SELECT @EventId=ETD.EventdetailId, @VMCC_Id=CRD.EventTicketAttributeId, 
@SponsorName=SponsorName+' - '+ FirstName+' '+ LastName, @CorpOrderId=CR.Id,@EventTicketDetailId = ETD.Id
FROM CorporateRequests CR  WITH(NOLOCK)
INNER JOIN  CorporateRequestDetails CRD WITH(NOLOCK)  ON CRD.CorporateRequestid = CR.Id
INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON CRD.EventTicketAttributeId = ETA.Id
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
WHERE CRD.Id = @CorpMapId

SELECT @Price = LocalPrice FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VMCC_Id    
 SELECT @AvailQty=KM.RemainingTickets,@OldSponsorID=SponsorId  
 FROM CorporateTicketAllocationDetails KM WITH(NOLOCK)  
 WHERE EventTicketAttributeId=@VMCC_Id AND Id=@FromKM_Id  
  
 SELECT @IsSeatAvail=ISNULL(Id,0) FROM MatchSeatticketDetails WITH(NOLOCK) WHERE SponsorId=@OldSponsorID AND EventTicketDetailId=@EventTicketDetailId  
 AND SeatStatusId =1
 IF(@IsSeatAvail<>0)  
 BEGIN  
  IF(ISNULL(@TicNumIds,'')='')  
  BEGIN  
   SET @IsSeatsOk=0  
   SET @ReturnVal = 'Tickets not transfered as there are seats associated with requested quantity you are trying to transfer'    
  END    
 END  
  
 IF(@IsSeatsOk=1)  
 BEGIN  
  IF(@AvailQty>=@OrderQty)  
 BEGIN  
  PRINT ('Avail')  
  IF EXISTS (SELECT * FROM Sponsors WITH(NOLOCK) WHERE IsEnabled=1 AND UPPER(RTRIM(LTRIM(SponsorName)))=
  UPPER(RTRIM(LTRIM(@SponsorName))))  
  BEGIN  
   SELECT @SponsorId=Id FROM Sponsors WITH(NOLOCK) WHERE IsEnabled=1 AND UPPER(RTRIM(LTRIM(SponsorName)))=
   UPPER(RTRIM(LTRIM(@SponsorName)))  
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
	  UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets-@OrderQty, 
	  RemainingTickets = RemainingTickets-@OrderQty,
	  UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id = @FromKM_Id
     
   IF(ISNULL(@TicNumIds,'')<>'')  
   BEGIN  
    UPDATE MatchSeatTicketDetails SET SponsorId=@SponsorId WHERE Id IN(SELECT * FROM SPLITSTRING(@TicNumIds,','))  AND SeatStatusId=1
   END  
  
   INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId,TransferToCorporateTicketAllocationDetailId, 
   AllocationOptionId, TotalTickets, Price, 
   IsEnabled, CreatedUtc, CreatedBy)
   VALUES(@FromKM_Id, @KM_Id,3,@OrderQty,@Price,1,GETUTCDATE(),@AltId);
     
   --@ReturnVal msg format used in code(Success! ), so don't change it.  
   SET @ReturnVal = 'Success! ' + CONVERT(Varchar(100),@OrderQty) + ' tickets blocked successfully.'      
   UPDATE CorporateRequestDetails  
   SET ApprovedStatus =1,ApprovedBy=@AltId,ApprovedTickets=@OrderQty,ApprovedUtc=GETUTCDATE() WHERE Id=@CorpMapId 
 END  
 ELSE  
 BEGIN  
  print ('not avail')  
  --@ReturnVal msg format used in code(Only  ), so don't change it.  
  SET @ReturnVal = 'Only '+ CONVERT(Varchar(100),@AvailQty) + ' tickets available for transfer!'  
 END  
 END  
 select @ReturnVal  
  
COMMIT TRANSACTION trans    
END TRY    
BEGIN CATCH    
ROLLBACK TRANSACTION trans     
END CATCH     
  
END  