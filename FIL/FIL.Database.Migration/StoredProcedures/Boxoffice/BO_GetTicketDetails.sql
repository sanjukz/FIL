CREATE PROC [dbo].[BO_GetTicketDetails] --200991719          
(          
  @TransID BIGINT        
)         
AS          
BEGIN     
DECLARE @CountNot INT, @CountEntered INT       
IF EXISTS(SELECT Id FROM  MatchSeatTicketDetails WITH(NOLOCk) WHERE TransactionId=@TransID)  
BEGIN  
	SELECT @CountNot=COUNT(*) FROM MatchSeatTicketDetails WITH(NOLOCk) WHERE TransactionId=@TransID AND ISNULL(EntryStatus,0)=0    
	SELECT @CountEntered=COUNT(*) FROM MatchSeatTicketDetails WITH(NOLOCk) WHERE TransactionId=@TransID AND ISNULL(EntryStatus,0)=1    
    
	SELECT @CountNot AS NotEntered,@CountEntered AS Entered    
END  
ELSE  
BEGIN  
	SELECT 0 AS NotEntered,0 AS Entered    
END  
    
END   
  