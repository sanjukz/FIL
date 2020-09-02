CREATE PROCEDURE [dbo].[BO_EventUpdatePickUp]                
(                  
	@TransId BIGINT,                  
	@EventId BIGINT,                  
	@PickedBy VARCHAR(500),                
	@AuthLetter INT,                
	@IDNumber VARCHAR(500),                
	@IDTypr VARCHAR(500),             
	@OriginalTicketSerialNos VARCHAR(5000),          
	@BOUserName VARCHAR(200)             
)                  
AS                  
BEGIN                  
 IF(@EventId<>0)      
 BEGIN
	UPDATE TransactionDeliveryDetails                  
	SET DeliveryStatus = 1,                  
	DeliveryDate = GETUTCDATE(),                  
	DeliverdTo = @PickedBy  
	WHERE TransactionDetailId IN (SELECT Id FROM TransactionDetails WITH(NOLOCK) WHERE TransactionId=@TransId)
END
 ELSE      
   IF(@EventId=0)      
    BEGIN       
    UPDATE TransactionDeliveryDetails                  
    SET DeliveryStatus = 1,                  
     DeliveryDate = GETUTCDATE(),                  
     DeliverdTo = @PickedBy  
     WHERE TransactionDetailId IN (SELECT Id FROM TransactionDetails WITH(NOLOCK) WHERE TransactionId=@TransId)           
   END 
END   