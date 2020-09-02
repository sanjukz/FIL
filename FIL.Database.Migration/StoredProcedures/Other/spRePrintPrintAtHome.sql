CREATE PROC [dbo].[spRePrintPrintAtHome]  
(  
	@TransactionId BIGINT  
)  
AS  
BEGIN  
 IF EXISTS(SELECT Id FROM Transactions WITH(NOLOCK) WHERE Id = @TransactionId AND TransactionStatusId = 8 
 AND ChannelId =1)  
 BEGIN  
  UPDATE MatchSeatTicketDetails SET PrintStatusId = 1 WHERE TransactionId = @TransactionId  
  SELECT 1 AS Status  
 END  
 ELSE  
 BEGIN  
  SELECT 0 AS Status  
 END  
END
