CREATE PROC [dbo].[BO_InsertRevertRequest] 
(                
	@TransId BIGINT,                
	@Retailer_id BIGINT,                
	@Comments VARCHAR(500)                  
)                
AS                
BEGIN              
IF EXISTS(SELECT Id from Transactions WITH(NOLOCK) WHERE ID=@TransId AND ChannelId=4 AND TransactionStatusId=8)                                
BEGIN         
	IF NOT EXISTS(SELECT * FROM BO_TransactionRevertRequest WITH(NOLOCK) WHERE  TransId=@TransId AND  Retailer_id = @Retailer_id)                  
	BEGIN              
		INSERT INTO BO_TransactionRevertRequest(TransId,Retailer_id,ReqDateTime,Comments,IsRevert) 
		VALUES(@TransId,@Retailer_id,GETUTCDATE(),@Comments,0)      
		SELECT 'Request Sent Successfully.' AS [Message]            
	END
	ELSE      
	BEGIN      
		SELECT 'Request already been successfully sent.' AS [Message]      
	END             
END      
ELSE      
BEGIN      
	SELECT 'Invalid confirmation no.' AS [Message]      
END       
END