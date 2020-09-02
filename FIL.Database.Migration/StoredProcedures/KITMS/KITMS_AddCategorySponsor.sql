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
	SELECT  @AltId = AltId FROM Users WITH(NOLOCK) WHERE UserName =@BlockBy

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