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