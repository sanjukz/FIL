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