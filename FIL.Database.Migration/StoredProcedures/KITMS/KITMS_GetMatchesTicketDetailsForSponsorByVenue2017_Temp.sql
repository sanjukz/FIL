CREATE PROC [dbo].[KITMS_GetMatchesTicketDetailsForSponsorByVenue2017_Temp]
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
SELECT A.Id FROM EventTicketAttributes A WITH(NOLOCK) 
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT Id FROM EventDetails WITH(NOLOCK) WHERE EventId=@EventCatId)

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
	SELECT @SumAvailableTic = ISNULL(SUM(RemainingTickets),0) FROM CorporateTicketAllocationDetails  A WITH(NOLOCK) 
	INNER JOIN EventTicketAttributes B WITH(NOLOCK) ON A.EventTicketAttributeId = B.Id
	INNER JOIN EventTicketDetails C WITH(NOLOCK) ON B.EventTicketDetailId = C.Id AND C.EventDetailId = @EventIdTemp
	WHERE EventTicketAttributeId
	IN (SELECT VmccId FROM @VmccId) AND SponsorId= @SponsorId 
	
	INSERT INTO @tblSonsorTransactDetails
	SELECT @EventIdTemp, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets For Transaction <b>' + 
	CONVERT(VARCHAR(200),@SumAvailableTic) +'</b> )' AS EventName,
	CASE WHEN  CAST(ED.StartDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status      
	FROM EventDetails ED WITH(NOLOCK) 
	WHERE ED.Id = @EventIdTemp
	ORDER BY StartDateTime ASC	
	
	SET @MatchCounter= @MatchCounter + 1
END

	INSERT INTO @tblSonsorReserveDetails
	SELECT ED.Id, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets <b>' + 
	CONVERT(VARCHAR(200),ISNULL(SUM(VMCC.RemainingTicketForSale),0)) +'</b> )' AS EventName,
	CASE WHEN  CAST(ED.StartDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status      
	FROM EventDetails ED WITH(NOLOCK) 
	INNER JOIN EventTicketDetails B WITH(NOLOCK) On ED.Id = b.EventDetailId
	INNER JOIN EventTicketAttributes VMCC WITH(NOLOCK) ON B.Id = VMCC.EventTicketDetailId
	WHERE B.TicketCategoryId = @VenueCatId
	AND ED.Id IN(SELECT EventId FROM @tblMatches)
	GROUP BY ED.Id,ED.Name,VMCC.RemainingTicketForSale, ED.StartDateTime
	ORDER BY StartDateTime ASC
	
	SELECT DISTINCT * FROM @tblSonsorReserveDetails
	SELECT DISTINCT * FROM @tblSonsorTransactDetails

END