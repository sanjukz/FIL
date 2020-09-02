CREATE  PROCEDURE [dbo].[spGetSeasonPackgeDetails] --16967
(
	@EventTicketAttributeId BIGINT
)
AS
BEGIN
	DECLARE @EventTicketDetailId  BIGINT, @EventDetailId  BIGINT, @EventId  BIGINT, @TicketCategoryId  INT, @VenueId  INT

	SELECT @EventTicketDetailId  = EventTicketDetailId FROM  EventTicketAttributes WITH(NOLOCK) WHERE Id = @EventTicketAttributeId
	SELECT @EventDetailId  = EventDetailId, @TicketCategoryId = TicketCategoryId FROM  EventTicketDetails WITH(NOLOCK) WHERE 
	Id = @EventTicketDetailId
	SELECT @EventId  = EventId, @VenueId = VenueId FROM  EventDetails WITH(NOLOCK) WHERE Id = @EventDetailId

	SELECT ETA.Id AS TicketCategoryId, ED.Id AS EventId, ED.Name +' ('+ CONVERT(VARCHAR(17), ED.StartDateTime,113)+')'AS EventName, 
	TC.Name AS TicketCategoryName,
	SeasonPackagePrice AS PricePerTicket, CONVERT(VARCHAR(17), ED.StartDateTime,113) AS DisplayDateTime
	FROM EventTicketAttributes ETA WITH(NOLOCK)  
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON ETA.CurrencyId = CM.Id  
	INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id  
	INNER JOIN Cities CTM WITH(NOLOCK) ON VD.CityId = CTM.Id
	WHERE ED.EventId = @EventId AND ED.VenueId = @VenueId --AND ED.IsEnabled =1
	AND ETD.TicketCategoryId = @TicketCategoryId
	AND ED.EndDateTime >=GETUTCDATE()
	ORDER BY TC.Name ASC, ETA.Price DESC, StartDateTime ASC 
END
