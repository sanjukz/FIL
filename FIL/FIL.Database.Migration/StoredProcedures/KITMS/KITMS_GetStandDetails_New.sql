CREATE PROCEDURE [dbo].[KITMS_GetStandDetails_New] --2019,0
(                                                                              
	@EventId BIGINT,      
	@ID BIGINT=null                                                                              
)                                                                            
AS                                                                              
BEGIN

DECLARE @tblEventTicketCategory TABLE(Id BIGINT)
IF(@ID IS NOT NULL AND @ID<>0)
BEGIN
	INSERT INTO @tblEventTicketCategory
	SELECT TicketCategoryId FROm DynamicStadiumTicketCategoriesDetails WITH(NOLOCK) WHERE DynamicStadiumCoordinateId= @Id
END 
ELSE
BEGIN
    INSERT INTO @tblEventTicketCategory
	SELECT DISTINCT TicketCategoryId FROm EventTicketDetails WITH(NOLOCK) WHERE EventDetailId = @EventId
END 
 
SELECT  ED.EventId, ETA.Id AS VMCC_ID, CT.Code + ' ' + CONVERT(VARCHAR,CONVERT(DECIMAL(10,2),ETA.Price)) +' <br/> ' +
CTLocal.Code + ' ' + CONVERT(VARCHAR,CONVERT(DECIMAL(10,2),ETA.LocalPrice)) AS Price,
ED.IsEnabled AS EventStatus, TC.Name AS Category, TC.Name AS VenueCatName, ETA.AvailableTicketForSale AS TotalTics,
(ISNULL((SELECT SUM(VMT.TotalTickets) FROM Transactions T WITH(NOLOCK)                                       
  INNER JOIN TransactionDetails VMT WITH(NOLOCK) ON T.Id = VMT.TransactionId                                                                            
  where T.TransactionStatusId = 8 AND VMT.EventTicketAttributeId = ETA.Id),0)                                                                           
) AS SoldTic, 0 AS BlockedTic, ETA.RemainingTicketForSale As RemainingTics,
(ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                             
 FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK) INNER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id = KM.SponsorId                                  
 WHERE KM.EventTicketAttributeId = ETA.Id and KM.IsEnabled = 1 AND SM.SponsorTypeId=1),0)) AS SponsoredTickets,
 (ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                             
 FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK) INNER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id = KM.SponsorId                                  
 WHERE KM.EventTicketAttributeId = ETA.Id and KM.IsEnabled = 1 AND SM.SponsorTypeId=0),0)) AS SeatKills,

 (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.IsEnabled = 1                                     
  AND STA.EventTicketAttributeId = ETA.Id  AND STA.TransactingOptionId IN(1,2) AND STA.TransactionId IN 
  (SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id AND T.TransactionStatusId=8)),0))as ttlUsedPaidCompSeats,

  (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.IsEnabled = 1                                     
  AND STA.EventTicketAttributeId = ETA.Id  AND STA.TransactingOptionId IN(2) AND STA.TransactionId IN 
  (SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id AND T.TransactionStatusId=8)),0))as Complimentary,

  (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.EventTicketAttributeId = ETA.Id  
  AND STA.TransactingOptionId IN(1,2) AND STA.TransactionId IN 
  (SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id)),0)) AS BlockUsed,

  (ISNULL((SELECT SUM(TD.TotalTickets) TransactionId FROM TransactionDetails TD WITH(NOLOCK)
  INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = ETA.Id AND T.TransactionStatusId=8),0)) AS PublicSales,
  CASE ETA.IsEnabled WHEN 1 THEN 'Active' ELSE 'Inactive' END AS [Status],ETA.IsEnabled AS Status, ETA.LocalPrice AS PricePerTic
 FROM                                         
	EventTicketAttributes ETA WITH(NOLOCK)
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id       
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id                                                                  
	INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.CurrencyId=CT.Id
	INNER JOIN CurrencyTypes CTLocal WITH(NOLOCK) ON ETA.LocalCurrencyId=CTLocal.Id                                               
 WHERE                                                                               
 ED.Id = @EventId   AND ETD.TicketCategoryId IN(SELECT * FROM @tblEventTicketCategory)                                                    
END