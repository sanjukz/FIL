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
		 CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId = @TransferSponsorId and EventTicketAttributeId = @VMCCId
		          
		 SET @BlockTic_New = @BlockTic_New+@TicketQty        
		 SET @AvailableTic_New = @AvailableTic_New+@TicketQty        
		         
		 IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId = @TransferSponsorId and EventTicketAttributeId = @VMCCId)        
		 BEGIN
			 SELECT @KITMSMasterId_New=Id,@BlockTic_New = AllocatedTickets,@AvailableTic_New=RemainingTickets FROM
			 CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId = @TransferSponsorId and EventTicketAttributeId = @VMCCId       
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