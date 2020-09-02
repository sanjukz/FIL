CREATE PROC [dbo].[KITMS_CheckReprintTransId]    
(    
  @TransId BIGINT    
)    
AS    
BEGIN   
IF EXISTS(SELECT Id FROm Transactions WITH(NOLOCK) WHERE Id = @TransId AND TransactionStatusId=8) 
BEGIN
  SELECT ID AS EventTransId FROm Transactions WITH(NOLOCK) WHERE Id = @TransId AND TransactionStatusId=8
END   
ELSE
BEGIN
	SELECT 0 AS EventTransId
END 
END