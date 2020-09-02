CREATE PROC [dbo].[KITMS_GetMatchesForSponsor2017]      
(            
 @SponsorId BIGINT,            
 @VenueCatId BIGINT,      
 @EventCatId BIGINT,      
 @VenueId BIGINT       
)            
AS            
BEGIN            
      
DECLARE  @VmccId TABLE(VmccId BIGINT)       
      
INSERT INTO @VmccId      
  select ETA.Id 
  FROM EventTicketAttributes ETA WITH(NOLOCK) 
  INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.Id=ETA.EventTicketDetailId    
  where ETD.EventDetailId IN(SELECT Id FROM EventDetails WITH(NOLOCK) WHERE VenueId = @VenueId AND eventId =@EventCatId) 
  AND ETD.TicketCategoryId=@VenueCatId       
      
DECLARE @tblSonsorReserveDetails TABLE(EventId BIGINT,EventName VARCHAR(200), Status INT)      
DECLARE @tblSonsorTransactDetails TABLE(EventId BIGINT, EventName VARCHAR(200), Status INT)      
DECLARE @tblMatches Table (RowId INT IDENTITY(1,1), EventId BIGINT)      
      
INSERT INTO @tblMatches      
SELECT DISTINCT Id FROM EventDetails WITH(NOLOCK) WHERE VenueId = @VenueId AND EventId=@EventCatId      
      
DECLARE  @MatchCounter INT = 1, @MatchCount INT =0, @EventIdTemp INT =0,@SumAvailableTic INT = 0      
      
SELECT @MatchCount = ISNULL(COUNT(*),0) FROM @tblMatches      
WHILE(@MatchCounter<=@MatchCount)      
BEGIN      
 SELECT @EventIdTemp = EventId FROM @tblMatches WHERE RowId = @MatchCounter      
 SELECT @SumAvailableTic = ISNULL(SUM(RemainingTickets),0) FROM CorporateTicketAllocationDetails CTAD WITH(NOLOCK) 
 INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) On ETA.Id = CTAD.EventTicketAttributeId
 INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.Id=ETA.EventTicketDetailId AND EventDetailId = @EventIdTemp
 WHERE 
 EventTicketAttributeId IN(SELECT VmccId FROM @VmccId)    
  AND SponsorId= @SponsorId      
       
 INSERT INTO @tblSonsorTransactDetails      
 SELECT @EventIdTemp, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets For Transaction <b>' +       
 CONVERT(VARCHAR(200),@SumAvailableTic) +'</b> )' AS EventName,      
 CASE WHEN  CAST(ED.EndDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status            
 FROM EventDetails ED       
 WHERE ED.Id = @EventIdTemp      
 ORDER BY StartDateTime ASC       
       
 SET @MatchCounter= @MatchCounter + 1      
END      
      
 INSERT INTO @tblSonsorReserveDetails      
 SELECT ED.Id, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets <b>' +       
 CONVERT(VARCHAR(200),ISNULL(SUM(ETA.RemainingTicketForSale),0)) +'</b> )' AS EventName,      
 CASE WHEN  CAST(ED.EndDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status            
FROM EventDetails ED  WITH(NOLOCK)      
 INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id = ETD.EventDetailId       
 INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON ETA.EventTicketDetailId=ETD.Id       
 WHERE ETD.TicketCategoryId = @VenueCatId        
 AND ED.Id IN(SELECT EventId FROM @tblMatches)        
 GROUP BY ED.Id,ED.name,ETA.RemainingTicketForSale, ED.EndDateTime,ED.StartDateTime        
 ORDER BY ED.StartDateTime  ASC      
       
 SELECT DISTINCT * FROM @tblSonsorReserveDetails      
 SELECT DISTINCT * FROM @tblSonsorTransactDetails      
      
END 


