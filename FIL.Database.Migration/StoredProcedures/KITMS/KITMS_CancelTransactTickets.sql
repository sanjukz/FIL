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
SELECT EventTicketAttributeId, TotalTickets FROm TransactionDetails WITH(NOLOCK) WHERE TransactionId = @TransId

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