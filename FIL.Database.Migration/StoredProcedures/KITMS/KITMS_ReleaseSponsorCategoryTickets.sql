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