CREATE PROC [dbo].[BO_InsertTransactionMaster]          
(          
	@UserId BIGINT,          
	@ClientIP VARCHAR(50),          
	@CurrencyId INT,          
	@TransactionDoneFrom VARCHAR(100),          
	@TransId BIGINT OUTPUT,      
	@EventID Bigint          
)          
AS          
BEGIN     
                                                        
DECLARE @AltId NVARCHAR(500);    
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE Id =@UserId    
  
INSERT INTO Transactions(ChannelId, CurrencyId, TotalTickets, GrossTicketAmount, DeliveryCharges, ConvenienceCharges, ServiceCharge,    
DiscountAmount, NetTicketAmount, TransactionStatusId, CreatedUtc, CreatedBy)    
VALUES(4,@CurrencyId,0,0,0,0,0,0,0,0,GETDATE(),@AltId)    
SET @TransId = SCOPE_IDENTITY()     
     
INSERT INTO TransactionPaymentDetails(TransactionId,PaymentOptionId,PaymentGatewayId,UserCardDetailId,RequestType,Amount,PayConfNumber,PaymentDetail,    
CreatedUtc,CreatedBy)    
VALUES(@TransId,NULL,NULL,NULL, null, null, 'Wallet','Boxoffice',GETDATE(),@AltId)    
RETURN @TransId                                        
                                                                            
END    