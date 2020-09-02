CREATE PROC [dbo].[BO_ShowBarcodesForTransId_TEMP]   --210000404                
(                  
  @TransId BIGINT                
)                  
AS                  
BEGIN
	SELECT A.TransactionId as TransId, BarcodeNumber AS barcode, X.Name AS EventName, D.Name AS VenueCatName,
	CM.Name as City_Name, 'N/A' AS [TIcketStatus], 'N/A' AS [PacketStatus]  ,A.Id AS TicNumId
	FROM MatchSeatTicketDetails A WITH(NOLOCK)               
	INNER JOIN EventTicketDetails B WITH(NOLOCK)  ON A.EventTicketDetailId = B.Id    
	INNER JOIN EventDetails X WITH(NOLOCK)  ON B.EventDetailId = X.Id    
	INNER JOIN EventTicketAttributes C WITH(NOLOCK)  ON B.Id = C.EventTicketDetailId   
	INNER JOIN TicketCategories D WITH(NOLOCK)  ON B.TicketCategoryId = D.Id   
	INNER JOIN venues VD WITH(NOLOCK)  ON X.VenueId=VD.Id   
	INNER JOIN Cities CM WITH(NOLOCK)  ON VD.CityId=CM.Id  
	WHERE A.TransactionId =@TransId  
END   
  