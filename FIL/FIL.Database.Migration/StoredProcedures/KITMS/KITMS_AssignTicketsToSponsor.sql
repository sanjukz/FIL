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
SELECT  @AltId = AltId FROM Users WITH(NOLOCK) WHERE UserName =@UpdateBY
    
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