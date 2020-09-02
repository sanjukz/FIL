CREATE proc [dbo].[KITMS_GetSponsorDetailsByVenue2017] --  3214, 607          
(                    
	@VenueCatId BIGINT, 
	@EventCatId BIGINT,   
	@VenueId BIGINT                    
)                    
AS                    
BEGIN                
CREATE TABLE #Events(EventId BIGINT, VmccId BIGINT )                    

INSERT INTO #Events 
SELECT B.EventDetailId, A.Id FROM EventTicketAttributes A WITH(NOLOCK)
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT Id FROM EventDetails WITH(NOLOCK) WHERE EventId=@EventCatId)

SELECT MS.Id AS SponsorId, MS.SponsorName                                                            
FROM Sponsors MS  WITH(NOLOCK)                                                           
INNER JOIN EventSponsorMappings S WITH(NOLOCK)                                                         
ON MS.Id = S.SponsorId                                                            
Where S.EventDetailId IN (SELECT EventId FROM #Events) AND MS.IsEnabled=1                                                         
AND  MS.Id NOT IN                                                             
(SELECT DISTINCT ST.SponsorId FROM CorporateTicketAllocationDetails ST WITH(NOLOCK) WHERE EventDetailId 
IN (SELECT EventId FROM #Events) AND EventTicketAttributeId IN (SELECT VmccId FROM #Events) and ST.IsEnabled = 1)                     
GROUP BY MS.Id, MS.SponsorName                                         

SELECT ST.SponsorId, SM.SponsorName, SUM(ST.AllocatedTickets) as SponsorBlocked, SUM(ST.RemainingTickets) as UnClassifiedBlocked
FROM CorporateTicketAllocationDetails ST  WITH(NOLOCK)
JOIN Sponsors SM WITH(NOLOCK) on SM.Id= ST.SponsorId
WHERE EventTicketAttributeId IN (SELECT VmccId FROM #Events) AND ST.IsEnabled = 1                     
GROUP BY ST.SponsorId, SM.SponsorName              

SELECT CONVERT(VARCHAR(MAX),ED.Name) + ' ( Available Tic ' + convert(varchar(200),ETA.RemainingTicketForSale) +' )' AS EventName FROM
EventDetails ED  WITH(NOLOCK)
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id= ETD.EventDetailId
INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON ETD.Id= ETA.EventTicketDetailId
WHERE ETD.TicketCategoryId = @VenueCatId AND ED.VenueId = @VenueId
   
SELECT ISNULL(SUM(ETA.RemainingTicketForSale),0) as AvailableTicket FROM
EventDetails ED  WITH(NOLOCK)
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id= ETD.EventDetailId
INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETD.Id= ETA.EventTicketDetailId
WHERE ETD.TicketCategoryId = @VenueCatId AND ED.VenueId = @VenueId AND ED.Id IN(SELECT Id FROM  EventDetails WITH(NOLOCK) WHERE EventId=@EventCatId)
   
SELECT 
(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2) AND STD.IsEnabled=1
INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0))  ttlUsedPaidCompSeats,

(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId = 2 AND STD.IsEnabled=1
INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0)) Complimentary,

(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2)
INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id
INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId
WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId),0))  BlockUsed

FROM EventTicketDetails ETD WITH(NOLOCK) 
INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON ETD.Id=EA.EventTicketDetailId AND ETD.TicketCategoryId = @VenueCatId
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.CurrencyId=CM.Id
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
WHERE ETD.TicketCategoryId = @VenueCatId

DROP TABLE #Events
END 