CREATE PROC [dbo].[Bo_GetTicNumIdfromBarCode]          
(         
	@TransId BIGINT,      
	@barcode VARCHAR(500)          
)          
AS           
BEGIN 
IF(@barcode = '0' or @barcode = '') 
BEGIN        
 SELECT Id AS TicNumId FROM  MatchSeatTicketDetails WITH(NOLOCk) WHERE TransactionId=@TransId    
End
ELSE
BEGIN        
 SELECT Id AS TicNumId FROM  MatchSeatTicketDetails WITH(NOLOCk) WHERE TransactionId=@TransId AND BarcodeNumber = @barcode
END
END