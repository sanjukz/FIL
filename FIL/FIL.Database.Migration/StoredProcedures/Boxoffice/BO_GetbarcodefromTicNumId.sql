CREATE PROCEDURE [dbo].[BO_GetbarcodefromTicNumId]   
(  
	@TicNumId BIGINT,  
	@TransId BIGINT  
)  
AS
BEGIN  
	SELECT BarcodeNumber AS barcode FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE Id=@TicNumId AND TransactionId=@TransId  
END  